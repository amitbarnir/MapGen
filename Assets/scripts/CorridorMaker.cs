using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: do i need this? nope
public class CorridorMaker {
}

/*    private Graph graph;
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
