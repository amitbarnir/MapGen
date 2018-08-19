using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	private int minXCoord;
	private int minZCoord;

	private int xSize;
	private int zSize ;
	private MapTile[,] roomTiles;

    public int MinXCoord { get; internal set; }
    public int MinZCoord { get; internal set; }
    public int XSize { get; internal set; }
    public int ZSize { get; internal set; }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    internal void addTile(int x, int z, MapTile tile)
    {
		if ( null == roomTiles ) {
			roomTiles = new MapTile[XSize,ZSize] ;
		}
        roomTiles[x,z] = tile;
    }

    internal void updateCollider()
    {
		BoxCollider collider = GetComponent<BoxCollider>();
//		Vector3 center = new Vector3(MinXCoord + (XSize/2f),0.0f,MinZCoord+(ZSize/2f));
//		Vector3 center = new Vector3( (XSize/2f),0.0f,(ZSize/2f));
		Vector3 size = new Vector3(XSize,0.0f,ZSize);
		collider.size = size;
//		collider.center = center ;
    }
 	
	 void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
//       Gizmos.color = Color.green;
//		Vector3 roomSize = new Vector3(XSize,0.0f,ZSize);
//		BoxCollider bc = GetComponent<BoxCollider>();
//      Gizmos.DrawWireCube(GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size);
	}
}
