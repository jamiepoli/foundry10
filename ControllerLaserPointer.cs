using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLaserPointer : MonoBehaviour {
	private SteamVR_TrackedObject trackedObj;
	// Reference to laser's prefab
	public GameObject laserPrefab;
	// Reference to laser instance
	private GameObject laser;
	// Transform component
	private Transform laserTransform;
	// Position the laser hits
	private Vector3 hitPoint; 

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}
	
	
	void Awake()
	{
    		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	private void ShowLaser(RaycastHit hit)
	{
    	// Show laser
    	laser.SetActive(true);
    	// Posn laser between point and controller
    	laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
    	// Point laser at point
    	laserTransform.LookAt(hitPoint);
    	laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
        hit.distance);
	}
	
	// Use this for initialization
	void Start () {
		laser = Instantiate(laserPrefab);
		laserTransform = laser.transform;
		
	}
	
	// Update is called once per frame
	void Update () {
		// if controller is held down, shoot raycast from controller
	if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
	{
    		RaycastHit hit;

    	// show laser, store point which laser raycast hits
    	if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100))
    		{
        		hitPoint = hit.point;
        		ShowLaser(hit);
    		}
	}
	else // otherwise hide laser when touchpad is released
		{
    		laser.SetActive(false);
		}	
	}
}
