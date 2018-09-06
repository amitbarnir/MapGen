using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGraphSearch : MonoBehaviour {

	private static readonly String NODE_A = "A";
	private static readonly String NODE_B = "B";
	private static readonly String NODE_C = "C";
	private static readonly String NODE_D = "D";
	private static readonly String NODE_E = "E";
	private static readonly String NODE_F = "F";
	private static readonly String NODE_G = "G";
	private static readonly String NODE_H = "H";
	private static readonly String NODE_I = "I";
	
	private Graph graph1;
	private Graph graph2;
	private Graph graph3;

	// Use this for initialization
	void Start () {
		prepareGraph1() ;
		prepareGraph2();
		prepareGraph3();
	}

    private void prepareGraph1()
    {
        graph1 = new Graph();
		graph1.addVertex(NODE_A);
		graph1.addVertex(NODE_B);
		graph1.addVertex(NODE_C);
		graph1.addVertex(NODE_D);
		graph1.addVertex(NODE_E);
		
		graph1.addWeightedEdge(NODE_A, NODE_B,1);
		graph1.addWeightedEdge(NODE_A, NODE_C, 1);
		graph1.addWeightedEdge(NODE_B, NODE_D, 2);
		graph1.addWeightedEdge(NODE_C, NODE_D, 1);
    }

    private void prepareGraph2()
    {
        graph2 = new Graph();
		graph2.addVertex(NODE_A);
		graph2.addVertex(NODE_B);
		graph2.addVertex(NODE_C);
		graph2.addVertex(NODE_D);
		graph2.addVertex(NODE_E);
		graph2.addVertex(NODE_F);
		
		// this time use no weights for edges.
		graph2.addEdge(NODE_A, NODE_B);
		graph2.addEdge(NODE_A, NODE_C);
		graph2.addEdge(NODE_B, NODE_D);
		graph2.addEdge(NODE_C, NODE_E);
		graph2.addEdge(NODE_D, NODE_E);
    }

	private void prepareGraph3()
    {
		graph3 = new Graph();
		graph3.addVertex(NODE_A);
		graph3.addVertex(NODE_B);
		graph3.addVertex(NODE_C);
		graph3.addVertex(NODE_D);
		graph3.addVertex(NODE_E);
		graph3.addVertex(NODE_F);
		graph3.addVertex(NODE_G);
		graph3.addVertex(NODE_H);
		graph3.addVertex(NODE_I);
		
		graph3.addWeightedEdge(NODE_A, NODE_B,3);
		graph3.addWeightedEdge(NODE_A, NODE_C, 5);
		graph3.addWeightedEdge(NODE_A, NODE_D, 29);
		graph3.addWeightedEdge(NODE_B, NODE_D, 7);
		graph3.addWeightedEdge(NODE_B, NODE_E, 5);
		graph3.addWeightedEdge(NODE_B, NODE_F, 11);
		graph3.addWeightedEdge(NODE_C, NODE_E, 4);
		graph3.addWeightedEdge(NODE_E, NODE_G, 11);
		graph3.addWeightedEdge(NODE_F, NODE_G, 3);
		graph3.addWeightedEdge(NODE_G, NODE_H, 7);
		graph3.addWeightedEdge(NODE_H, NODE_I, 3);
		        
    }
 	
   
    // Update is called once per frame
    void Update () {
		testGraph1();
		testGraph2();
		testGraph3();


		
	}

    private void testGraph3()
    {
   		WeightedGraphSearch dijkstra = new WeightedGraphSearch();
		LinkedList<VertexOld> path = new LinkedList<VertexOld>();
		path = dijkstra.searchGraph(graph3.getVertex(NODE_A), graph3.getVertex(NODE_I) );
		Debug.Assert( path.Count == 6 );
		Debug.Assert(path.Last.Value == graph3.getVertex(NODE_A));
		Debug.Assert(path.Last.Previous.Value == graph3.getVertex(NODE_B));
		Debug.Assert(path.Last.Previous.Previous.Value == graph3.getVertex(NODE_F));
		Debug.Assert(path.Last.Previous.Previous.Previous.Value == graph3.getVertex(NODE_G));
		Debug.Assert(path.First.Next.Value == graph3.getVertex(NODE_H));
		Debug.Assert(path.First.Value == graph3.getVertex(NODE_I));
		
    }

    private void testGraph1()
    {
   		WeightedGraphSearch dijkstra = new WeightedGraphSearch();
		LinkedList<VertexOld> path = new LinkedList<VertexOld>();
		path = dijkstra.searchGraph(graph1.getVertex(NODE_A), graph1.getVertex(NODE_B) );
		Debug.Assert( path.Count != 0 );
		// make sure we found a path between indirectly connected nodes.
		path = dijkstra.searchGraph(graph1.getVertex(NODE_A), graph1.getVertex(NODE_D) );
		Debug.Assert(  path.Count !=0 );
		// make sure we found no path between non-connected nodes
		path = dijkstra.searchGraph(graph1.getVertex(NODE_A), graph1.getVertex(NODE_E));
		Debug.Assert( path.Count == 0 );
    }

	private void testGraph2()
    {
        WeightedGraphSearch dijkstra = new WeightedGraphSearch();
		LinkedList<VertexOld> path = new LinkedList<VertexOld>();

		path = dijkstra.searchGraph(graph2.getVertex(NODE_A), graph2.getVertex(NODE_E) );
		// did we find the shortest possible path?
		Debug.Assert( path.Count == 3 );
		Debug.Assert(path.Last.Value == graph2.getVertex(NODE_A));
		Debug.Assert(path.Last.Previous.Value == graph2.getVertex(NODE_C));
		Debug.Assert(path.First.Value == graph2.getVertex(NODE_E));
		// we shouldn't find path between non-connected vertices
		path = dijkstra.searchGraph(graph2.getVertex(NODE_A), graph2.getVertex(NODE_F) );
		Debug.Assert( path.Count ==0 );
    }
}
