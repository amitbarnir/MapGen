using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform thePlayer = null;
	public float smoothTime = 0.3f;
	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	public void findPlayer () {
		 GameObject playerGO = GameObject.FindGameObjectsWithTag("Player")[0];
		 thePlayer = playerGO.transform;
	}
	
	// Update is called once per frame
	void Update () {
	 	// Define a target position above and behind the target transform
        Vector3 targetPosition = thePlayer.TransformPoint(new Vector3(0, 5, -10));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);	
	}
}
