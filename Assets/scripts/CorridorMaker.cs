using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using UnityEngine;

public class CorridorMaker : MonoBehaviour {
    private Map theMap = null;
    public MapTile mapTilePF;

    private CorridorMaker() { }

    public CorridorMaker( Map map) {
        theMap = map;
    }

    public void generateCorridorsFromEdges( List<Edge> edges,  TriangleNet.Mesh mesh, Map map ) {
        
        foreach ( Edge edge in edges ) {
            Vector3 firstRoomCenter = HelperFunctions.VertexToVector3(mesh.vertices[ edge.P0 ]);
            Vector3 secondRoomCenter = HelperFunctions.VertexToVector3( mesh.vertices[ edge.P1 ]);
            Room firstRoom = map.findRoomContainingCoords( firstRoomCenter );
            Room secondRoom = map.findRoomContainingCoords( secondRoomCenter );
            if( firstRoom != null && secondRoom != null && firstRoom != secondRoom) {
                carveCorridor( firstRoom , secondRoom, map );
            }
        }
    }

    private void carveCorridor( Room firstRoom , Room secondRoom,Map map ) {
        // get all the xs and zs
        int startingRoomCenterX = (int)firstRoom.getCenter().x;
        int startingRoomCenterZ = (int)firstRoom.getCenter().z;
        int endingRoomCenterX = (int)secondRoom.getCenter().x;
        int endingRoomCenterZ = (int)secondRoom.getCenter().z;
/*
        // figure out what direction are we carving.
        Vector3 leftOrRight = Vector3.zero;
        if ( startingRoomCenterX  > endingRoomCenterX ) {
            leftOrRight = Vector3.left;
        } else if( startingRoomCenterX   < endingRoomCenterX ) {
            leftOrRight = Vector3.right;
        }

        // and again for up and down
        Vector3 upOrDown = Vector3.zero;
        if ( startingRoomCenterZ > endingRoomCenterZ ) {
            upOrDown = Vector3.back;
        } else if ( startingRoomCenterZ < endingRoomCenterZ ) {
            upOrDown = Vector3.forward;
        }
*/
        // start at first room outer bounds and dig all the way to second room center.
        //        int startingX = startingRoomCenterX + ( ( (int)( firstRoom.XSize / 2 ) + 1 ) * (int)leftOrRight.x) ;
        int currentXCoord = (startingRoomCenterX > endingRoomCenterX) ? 
            startingRoomCenterX - (int)( firstRoom.XSize / 2 ) :
            startingRoomCenterX + (int)( firstRoom.XSize / 2 );
        int currentZCoord = startingRoomCenterZ;
/*        int currentZCoord = ( startingRoomCenterZ > endingRoomCenterZ ) ?
            startingRoomCenterZ - (int)( firstRoom.ZSize / 2 ) :
            startingRoomCenterZ + (int)( firstRoom.ZSize / 2 );
*/
        while ( currentXCoord != endingRoomCenterX ) {
            MapTile tile = GameObject.Instantiate( mapTilePF );
            //            tile.transform.position = new Vector3( currentXCoord , 0.0f , startingRoomCenterZ );
            //          map.addTile( currentXCoord , startingRoomCenterZ , tile );
            tile.transform.position = new Vector3( currentXCoord , 0.0f , currentZCoord );
            map.addTile( currentXCoord , currentZCoord , tile );
            //startingX += (int)leftOrRight.x;
            currentXCoord += ( currentXCoord < endingRoomCenterX ) ? 1 : -1;
        }

        // start at first room outer bounds and dig all the way to second room center.
//        int startingZ = startingRoomCenterZ + (( (int)( firstRoom.ZSize / 2 ) + 1 )  *(int)upOrDown.z);
        while ( currentZCoord != endingRoomCenterZ ) {
            MapTile tile = GameObject.Instantiate( mapTilePF );
            //            tile.transform.position = new Vector3( startingRoomCenterX , 0.0f , currentZCoord );
            //            map.addTile( startingRoomCenterX , currentZCoord , tile );
            tile.transform.position = new Vector3( currentXCoord , 0.0f , currentZCoord );
            map.addTile( currentXCoord , currentZCoord , tile );
            currentZCoord += (currentZCoord < endingRoomCenterZ) ? 1 : -1;
        }

    }
}

/*  private Graph graph;
    private WeightedGraphSearch djikstra = new WeightedGraphSearch();

    private Map theMap;
    public Map TheMap
    {
        get  { return theMap; }
        set  { theMap = value;}
    }

    public CorridorMaker(Graph g) {
        this.graph = g;
    }


    public CorridorMaker(Map m) {
        this.theMap = m;
    }
    
    private bool initializeGraph()
    {
        int sizeX = theMap.SizeX;
        int sizeZ = theMap.SizeZ;
        // simply create an x*z sized graph of vertices
        for ( int x = 0; x < sizeX; x++) {
            for (int z = 0; z < sizeZ; z++) {
                string vertexName = x + "." + z;
                if (!graph.addVertex(vertexName) ) {
                    return false;
                }
            }
        }
        return true;
    }
}*/
