using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public MapGenerator mapGeneratorPrefab;
	public PlayerController playerPrefab;

	public CameraController theCamera;
	private MapGenerator mapGenInstance = null;
    private CorridorMaker corridorMaker = null;
	private Map theMap;
	
	private PlayerController thePlayer;

	void Start () {
		mapGenInstance = Instantiate(mapGeneratorPrefab);
		theMap = mapGenInstance.genereateRandomMap();
//        corridorMaker = new CorridorMaker(theMap);
        Vector3 startingRoomPos = theMap.getStartingRoom().transform.position;
		thePlayer = Instantiate(playerPrefab);
		thePlayer.transform.position = startingRoomPos;
		theCamera.transform.position = new Vector3(theMap.SizeX/2,30.0f,theMap.SizeZ/2) ;
		theCamera.transform.localRotation = Quaternion.Euler(45.0f,0.0f,0.0f) ;
		
		
	}
}
