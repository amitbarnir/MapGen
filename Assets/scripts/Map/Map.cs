using System;
using System.Collections;
using UnityEngine;

public class Map : MonoBehaviour {

    
    private struct mapDirection {
        public const int up = -1;
        public const int down = 1;
        public const int left = -1;
        public const int right = 1;
        public const int none = 0;
    }

    public int SizeX { get; internal set; }
    public int SizeZ { get; internal set; }

    private Graph searchGraph = new Graph();

    public Room getStartingRoom() {
        return (Room) rooms[0];
    }

    private ArrayList rooms;
    private TileInfo[,] tiles;

    // Use this for initialization
    void Start () {
        SizeX = 0;
        SizeZ = 0;
	}

    internal void addRoom(Room r)  {
        if( null == rooms ) {
            rooms = new ArrayList();
        }
        rooms.Add(r);

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
                Vertex v = new Vertex( s );
                currentTile = new TileInfo(x,z,v);
                searchGraph.addVertex( v );
                tiles[x,z] = currentTile;
            }
        }

    }

    internal void addTile( int x , int z , MapTile tile ) {
        if( null == tiles ) {
            initMap();
        }
        tiles[x,z].Tile = tile;
    }

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
}
