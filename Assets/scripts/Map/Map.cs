using System;
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;
using UnityEngine;

public class Map : MonoBehaviour {

    public int SizeX { get; internal set; }
    public int SizeZ { get; internal set; }

//    private Graph searchGraph = new Graph();

    public Room getStartingRoom() {
        return (Room) rooms[0];
    }

    private List<Room> rooms;
    private TileInfo[,] tiles;

    // Use this for initialization
    void Start () {
        SizeX = 0;
        SizeZ = 0;
	}

    internal void addRoom(Room r)  {
        if( null == rooms ) {
            rooms = new List<Room>();
        }
        rooms.Add(r);

    }

    internal Room findRoomContainingCoords( Vector3 coords ) {
        for ( int i = 0 ; i < rooms.Count ; i++ ) {
            if ( rooms[i].areCoordsInRoom(coords) ) {
                return rooms[ i ];
            }
        }
        return null;
    }

    internal int getNumberOfRooms() {
        return rooms != null ? rooms.Count : 0 ;
    }
    
    internal void initMap() {
        TileInfo currentTile = null;
        tiles = new TileInfo[SizeX,SizeZ];
        Debug.Log("Map size: (" + SizeX + "," + SizeZ + ")");
        for( int x = 0 ; x < SizeX ; x++ ) {
            for( int z = 0 ; z < SizeZ ; z++ ) {
                String s = x + "." + z;
                VertexOld v = new VertexOld( s );
                currentTile = new TileInfo(x,z,v);
//                searchGraph.addVertex( v );
                tiles[x,z] = currentTile;
            }
        }

    }

    internal Room getRoomByCenter( Vector3 roomCenter ) {
        for ( int i = 0 ; i < getNumberOfRooms() ; i++ ) {
            if( rooms[i].getCenter() == roomCenter ) {
                return rooms[ i ];
            }
        }
        return null;
    }

    internal void addTile( int x , int z , MapTile tile ) {
        if( null == tiles ) {
            initMap();
        }
        tiles[x,z].Tile = tile;
    }

    internal Vertex[] getRoomCenterVertices() {
        int roomCount = this.getNumberOfRooms();
        int centerX = 0;
        int centerZ = 0;

        Vertex[] roomCenters = new Vertex[ roomCount ];
        for ( int i = 0 ; i < roomCount ; i++ ) {
            Room r = (Room)rooms[ i ];
            centerX = r.MinXCoord + r.XSize / 2;
            centerZ = r.MinZCoord + r.ZSize / 2;
            roomCenters[ i ] = new Vertex(centerX,centerZ);
        }
        return roomCenters;
    }

    internal MapTile getTile( int x , int z ) {
        return tiles[ x , z ].Tile;
    }
}

// 06/09/2018
/*
    internal void initPathfindingGraph()
    {
        TileInfo tile;
        for( int x = 1; x < SizeX-1; x++ ) {
            for ( int z = 1 ; z < SizeZ -1 ; z++ ) {
                tile = tiles[ x , z ];
                // todo: 8 neighbors.
                connectToNeighbour( tile, mapDirection.none, mapDirection.up );
                connectToNeighbour( tile , mapDirection.none , mapDirection.down );
                connectToNeighbour( tile , mapDirection.left , mapDirection.none );
                connectToNeighbour( tile , mapDirection.right , mapDirection.none );
            }
        }
    }
    private void connectToNeighbour( TileInfo tile, int leftOrRight,int upOrDown) {
        TileInfo neighbour = tiles[tile.X + leftOrRight,tile.Z + upOrDown];
        //TODO: make weight adjustable from unity.
        int weight = 1;
        // if both have tiles, this means they are both in a room so movement is easier
        if ( tile.Tile != null && neighbour.Tile != null ) {
            weight = 0;
        }
        searchGraph.addWeightedEdge( tile.Vertex.getName() , neighbour.Vertex.getName() , weight );
    }
    
    private struct mapDirection {
        public const int up = -1;
        public const int down = 1;
        public const int left = -1;
        public const int right = 1;
        public const int none = 0;
    }
    */
