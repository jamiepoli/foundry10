using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	//Stores Gameobject that the trigger is currently colliding with, so I can grab object
	private GameObject collidingObject;
	//Reference to object the player currently is holding
	private GameObject objectInHand;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int) trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void SetCollidingObject(Collider col)
	{
		//Prevents GameObject from being a potential grab object, if player is already holding something or object has no rigidbody.
		if (collidingObject || !col.GetComponent<RigidBody>())
		{
			return;
		}
		//assigns object as potential grab object
		collidingObject = col.gameObject;

	}


	// When the trigger collider enters another, this sets up the other collider as a potential grab target
	public void OnTriggerEnter(Collider other)
	{
		SetCollidingObject(other);
	}

	// Similar to section one (// 1), but different
	// because it ensures that the target is set when the player holds a controller over an object for a while.
	public void OnTriggerStay(Collider other)
	{
		SetCollidingObject(other);
	}

	// When the collider exits an object, abandoning an ungrabbed target, 
	// this code removes its target by setting it to null.
	public void OnTriggerExit(Collider other)
	{
		if (!collidingObject)
		{
			return;
		}

		collidingObject = null;
	}

	private void GrabObject()
	{
		// Move the GameObject inside the player’s hand and remove it from the collidingObject variable.
		objectInHand = collidingObject;
		collidingObject = null;
		// Add a new joint that connects the controller to the object using the AddFixedJoint() method below.
		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
	}

	// Make a new fixed joint, add it to the controller, and then set it up so it doesn’t break easily.
	// Finally, you return it.
	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	private void ReleaseObject()
	{
		// Make sure there’s a fixed joint attached to the controller.
		if (GetComponent<FixedJoint>())
		{
			// Remove the connection to the object held by the joint and destroy the joint.
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());
			// Add the speed and rotation of the controller when the player releases the object, 
			// so the result is a realistic arc.
			objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
		}
		// Remove the reference
		objectInHand = null;
	}

	// Update is called once per frame
	void Update () {
		// When the player squeezes the trigger and there’s a potential grab target, this grabs it.
		if (Controller.GetHairTriggerDown())
		{
			if (collidingObject)
			{
				GrabObject();
			}
		}

		// If the player releases the trigger and there’s an object 
		// attached to the controller, this releases it.
		if (Controller.GetHairTriggerUp())
		{
			if (objectInHand)
			{
				ReleaseObject();
			}
		}
	}
}
