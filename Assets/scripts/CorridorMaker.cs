using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using UnityEngine;
using Random = UnityEngine.Random;

public class CorridorMaker : MonoBehaviour {
    
    public MapTile mapTilePF;
    public int minimumCorridorWidth = 2;
    public int maximumCorridorWidth = 4;
    private Map theMap = null;
    private TriangleNet.Mesh mesh = null;
    private List<Edge> minimumSpanningTree = null;


    private CorridorMaker() { }

    public void generateCorridorsFromEdges() {
        
        foreach ( Edge edge in minimumSpanningTree ) {
            Vector3 firstRoomCenter = HelperFunctions.VertexToVector3(mesh.vertices[ edge.P0 ]);
            Vector3 secondRoomCenter = HelperFunctions.VertexToVector3( mesh.vertices[ edge.P1 ]);
            Room firstRoom = theMap.findRoomContainingCoords( firstRoomCenter );
            Room secondRoom = theMap.findRoomContainingCoords( secondRoomCenter );
            if( firstRoom != null && secondRoom != null && firstRoom != secondRoom) {
                carveCorridor( firstRoom , secondRoom);
            }
        }
    }

    private void carveCorridor( Room firstRoom , Room secondRoom ) {
        int corridorWidth = Random.RandomRange( minimumCorridorWidth , maximumCorridorWidth );
        
        // get all the xs and zs
        int startingRoomCenterX = (int)firstRoom.getCenter().x;
        int startingRoomCenterZ = (int)firstRoom.getCenter().z;
        int endingRoomCenterX = (int)secondRoom.getCenter().x;
        int endingRoomCenterZ = (int)secondRoom.getCenter().z;

        // start at first room outer bounds and dig all the way to second room center.
        int currentXCoord = (startingRoomCenterX > endingRoomCenterX) ? 
            startingRoomCenterX - (int)( firstRoom.XSize / 2 ) :
            startingRoomCenterX + (int)( firstRoom.XSize / 2 );
        int currentZCoord = startingRoomCenterZ;

        while ( currentXCoord != endingRoomCenterX  ) {
            for ( int z = -corridorWidth/2 ; z <= corridorWidth/2 ; z++ ) {
                MapTile tile = GameObject.Instantiate( mapTilePF );
                tile.transform.position = new Vector3( currentXCoord , 0.0f , currentZCoord + z );
                theMap.addTile( currentXCoord  , currentZCoord + z , tile );
            }
            currentXCoord += ( currentXCoord < endingRoomCenterX ) ? 1 : -1;
        }
        //TODO: add one collider for this whole corridor segment

        // start at first room outer bounds and dig all the way to second room center.
        while ( currentZCoord != endingRoomCenterZ  ) {
            for ( int x = -corridorWidth / 2 ; x <= corridorWidth / 2 ; x++ ) {
                MapTile tile = GameObject.Instantiate( mapTilePF );
                tile.transform.position = new Vector3( currentXCoord + x, 0.0f , currentZCoord );
                theMap.addTile( currentXCoord + x , currentZCoord , tile );
            }
            currentZCoord += (currentZCoord < endingRoomCenterZ) ? 1 : -1;
        }
        //TODO: add one collider for this whole corridor segment
    }

    internal void setMap( Map map ) {
        this.theMap = map;
    }

    internal void init() {
        // create polygon where each room center is a vertex.
        Polygon polygon = new Polygon();
        Vertex[] roomCenters = theMap.getRoomCenterVertices();
        for ( int i = 0 ; i < roomCenters.Length ; i++ ) {
            polygon.Add( roomCenters[ i ] );
        }
        // triangulate it using Delaunay's triangulation
        TriangleNet.Meshing.ConstraintOptions options =
            new TriangleNet.Meshing.ConstraintOptions() { ConformingDelaunay = false };

        mesh = (TriangleNet.Mesh)polygon.Triangulate( options );
        // build a minimum spanning tree of the graph represented by the mesh.
        Kruskal.Kruskal mspBuilder = new Kruskal.Kruskal();
        minimumSpanningTree = mspBuilder.run( mesh );
    }

    internal void generateCorridors() {
        generateCorridorsFromEdges();
    }

    public void OnDrawGizmos() {
        if ( mesh == null ) {
            return;
        }
        Gizmos.color = Color.red;
        foreach ( Edge e in mesh.Edges ) {
            Vertex v0 = mesh.vertices[ e.P0 ];
            Vertex v1 = mesh.vertices[ e.P1 ];
            Vector3 p0 = new Vector3( (float)v0.x , 0.0f , (float)v0.y );
            Vector3 p1 = new Vector3( (float)v1.x , 0.0f , (float)v1.y );
            Gizmos.DrawLine( p0 , p1 );
        }

        if ( minimumSpanningTree == null ) {
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
}
