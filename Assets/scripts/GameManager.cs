using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public MapGenerator mapGeneratorPrefab;
	public PlayerController playerPrefab;

	public CameraController theCamera;
	private MapGenerator mapGenInstance = null;
	private Map theMap;
	
	private PlayerController thePlayer;

	void Start () {
		mapGenInstance = Instantiate(mapGeneratorPrefab);
		theMap = mapGenInstance.genereateRandomMap();
		Vector3 startingRoomPos = theMap.getStartingRoom().transform.position;
		thePlayer = Instantiate(playerPrefab);
		thePlayer.transform.position = startingRoomPos;
		thePlayer.transform.Translate(0.0f,4.0f,0.0f);
		theCamera.transform.parent = thePlayer.transform;
		theCamera.transform.localPosition = new Vector3(0.0f,-10.0f,0.0f);
		theCamera.transform.localRotation = Quaternion.Euler(30.0f,0.0f,0.0f);
		
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
