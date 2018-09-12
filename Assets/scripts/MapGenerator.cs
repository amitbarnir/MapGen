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
    private TriangleNet.Mesh mesh = null;
    private List<Edge> minimumSpanningTree = null;
    private CorridorMaker corridorMaker = null;


    public void Update() {
    }

    public void OnDrawGizmos() {
        if ( mesh == null ) {
            return;
        }
        Gizmos.color = Color.red;
        foreach ( Edge e in mesh.Edges ) {
            Vertex v0 = mesh.vertices[ e.P0 ] ;
            Vertex v1 = mesh.vertices[ e.P1 ];
            Vector3 p0 = new Vector3( (float)v0.x ,0.0f, (float)v0.y );
            Vector3 p1 = new Vector3( (float)v1.x , 0.0f , (float)v1.y );
            Gizmos.DrawLine( p0 , p1 );
        }
        
        if( minimumSpanningTree  == null) {
            return;
        }
        Gizmos.color = Color.green;
        foreach ( Edge edge in minimumSpanningTree ) {
            Vertex v0 = mesh.vertices[ edge.P0 ];
            Vertex v1 = mesh.vertices[ edge.P1 ];
            Vector3 p0 = new Vector3( (float)v0.x , 0.0f , (float)v0.y );
            Vector3 p1 = new Vector3( (float)v1.x , 0.0f , (float)v1.y );
            Gizmos.DrawLine( p0 , p1 );
        }
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
        // create polygon where each room center is a vertex.
        Polygon polygon = new Polygon();
        Vertex[] roomCenters = theMap.getRoomCenterVertices();
        for ( int i = 0 ; i < roomCenters.Length ; i++ ) {
            polygon.Add( roomCenters[ i ] );
        }
        // triangulate it using Delaunay's triangulation
        TriangleNet.Meshing.ConstraintOptions options =
            new TriangleNet.Meshing.ConstraintOptions() { ConformingDelaunay = false };

        mesh = (TriangleNet.Mesh)polygon.Triangulate(options);
        // build a minimum spanning tree of the graph represented by the mesh.
        Kruskal.Kruskal mspBuilder = new Kruskal.Kruskal();
        minimumSpanningTree = mspBuilder.run(mesh);
        
        if( corridorMaker == null ) {
            corridorMaker = Instantiate( corridorMakerPrefab );
        }
        corridorMaker.generateCorridorsFromEdges( minimumSpanningTree , mesh,theMap );
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
                MapTile tile = Instantiate(mapTilePrefab);
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


/* 10/09/2018
    private bool someRoomsAreNotReachable() {
        WeightedGraphSearch djikstra = new WeightedGraphSearch();
        VertexOld[] roomCenters = theMap.getRoomCenterVertices();
        for ( int i = 0 ; i < roomCenters.Length ; i++ ) {
            for ( int j = i + 1 ; j < roomCenters.Length ; j++ ) {
                if( djikstra.searchGraph(roomCenters[i],roomCenters[j]).Count == 0) {
                    return true;
                }
            }
        }
        return false;
    }

    private void generateShortestCorridor() {
        WeightedGraphSearch djikstra = new WeightedGraphSearch();
        Vertex[] roomCenters = theMap.getRoomCenterVertices();
        LinkedList<VertexOld> minRoute     = null;
        LinkedList<VertexOld> tempRoute    = null;
        int minRouteLength = 0;
        // go over all room to room permutations
        for ( int i = 0 ; i < roomCenters.Length ; i++ ) {
            for ( int j = i+1 ; j < roomCenters.Length ; j++ ) {
                tempRoute = djikstra.searchGraph( roomCenters[ i ] , roomCenters[ j ] );
                // and find the shortest unconnected path from room to room
                if ( tempRoute.Count < minRouteLength ) {
                    minRouteLength = tempRoute.Count;
                    minRoute = tempRoute;
                }
            }
        }
        carveCorridor( minRoute );

    }

private void carveCorridor( LinkedList<VertexOld> minRoute ) {
    foreach ( VertexOld vertex in minRoute ) {
        //TODO: this is retarded. find a more efficient way of doing this
        char[] delimeter = { '.' };
        String[] coords = vertex.getName().Split( delimeter ) ;

        int x = Int32.Parse( coords[ 0 ] );
        int z = Int32.Parse( coords[ 1 ] );
        if ( theMap.getTile(x,z) != null ) {
            MapTile tile = Instantiate( mapTilePrefab );
            //tile.transform.parent = r.transform;
            tile.transform.position = new Vector3( x , 0.0f , z );
            //TODO: find a better solution for who holds the tiles
            theMap.addTile( x , z , tile );
        }
    }
}
*/
