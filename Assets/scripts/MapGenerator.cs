using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public int minMapSizeX = 32;
    public int maxMapSizeX=128;
    
    public int minMapSizeZ = 32;
    public int maxMapSizeZ=128;
    

    public int maxNumberOfRooms = 30;
    public int minRoomSizeX = 3;
    public int maxRoomSizeX = 10;
    public int minRoomSizeZ = 3;
    public int maxRoomSizeZ = 10;
    public int roomGenerationRetries = 500;

    public Map mapPrefab;
    public MapTile mapTilePrefab;
    public Room roomPrefab;
    public CorridorMaker corridorMakerPrefab;

    private Map theMap;
    private CorridorMaker corridorMaker = null;


    public void Update() {
    }


    public Map genereateRandomMap() {

        if (theMap != null) {
            Destroy(this.theMap);
        }

        theMap = Instantiate(mapPrefab) ;
        theMap.SizeX  = UnityEngine.Random.Range(minMapSizeX, maxMapSizeX);
        theMap.SizeZ = UnityEngine.Random.Range(minMapSizeZ, maxMapSizeZ);
        // randomly generate rooms but discard any room that overlaps existing ones.
        int roomRetries =0;
        while( roomRetries < this.roomGenerationRetries && theMap.getNumberOfRooms() < maxNumberOfRooms) {
            Room r = generateRandomRoom();
            Vector3 halfRoomSize = new Vector3(r.XSize/2f,1.0f,r.ZSize/2f);
            Collider[] hitColliders  =  Physics.OverlapBox(r.transform.localPosition, halfRoomSize);
            if( hitColliders.Length > 1 ) {
                Destroy(r);
            } else {
                generateRoomTiles(r) ;
                theMap.addRoom(r);
            }
            roomRetries++;
        }

        if( corridorMaker == null ) {
            corridorMaker = Instantiate( corridorMakerPrefab );
            corridorMaker.setMap( theMap );
            corridorMaker.init();
        }
        corridorMaker.generateCorridors();
        return theMap;
    }
    private void generateRoomTiles(Room r)
    {
         // create room tiles
        // TODO: fix tile coords.
        for ( int x = 0; x <  r.XSize; x++) {
            for( int z = 0; z < r.ZSize; z++ ) {
                Vector3 relativePos = new Vector3( x+0.5f,0.0f, z+0.5f); // TODO: ??? why +0.5 works?
                Vector3 offset = new Vector3(-r.XSize/2f,0,-r.ZSize/2f);
                MapTile tile = GameObject.Instantiate(mapTilePrefab);
                tile.transform.parent = r.transform;
                tile.transform.localPosition = relativePos + offset;
                //TODO: find a better solution for who holds the tiles
                theMap.addTile( r.MinXCoord + x , r.MinZCoord + z , tile );
            }
        }
    }

    private Room generateRandomRoom() {
        Room r = (Room)Instantiate(roomPrefab);
        r.transform.parent = theMap.transform;
        // randomly pick room size. 
        r.XSize = UnityEngine.Random.Range(minRoomSizeX, maxRoomSizeX);
        r.ZSize = UnityEngine.Random.Range(minRoomSizeZ, maxRoomSizeZ);
        // randomly pick room location. 
        r.MinXCoord = UnityEngine.Random.Range(1, theMap.SizeX - r.XSize - 1);
        r.MinZCoord = UnityEngine.Random.Range(1, theMap.SizeZ - r.ZSize - 1);
        r.transform.position = new Vector3Int(r.MinXCoord+(r.XSize/2),0,r.MinZCoord+(r.ZSize/2));
        // setup the box collider for the room's floor
        r.updateCollider();
        return r;
    }
}