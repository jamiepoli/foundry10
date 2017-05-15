using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLaserPointer : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
