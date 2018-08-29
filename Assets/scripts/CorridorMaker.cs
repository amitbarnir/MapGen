using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorMaker {

    private Map theMap;
    public Map TheMap
    {
        get
        {
            return theMap;
        }

        set
        {
            theMap = value;
        }
    }
    private Graph graph = new Graph();
    private WeightedGraphSearch djikstra = new WeightedGraphSearch();

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

    private bool createRoomTileMap()
    {
        return true;
    }
    
    

}
