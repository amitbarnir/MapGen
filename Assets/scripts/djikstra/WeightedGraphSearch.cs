using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeightedGraphSearch {

	/**
	 * holds weight to target from each unvisited node
	 * was: Hashtable< Vertex, Integer >
	 */
	private Hashtable weightToTarget;
	
	/**
	 * maps each node in the path to the previous node. used to construct the final path
	 * was: Hashtable< Vertex, Vertex >
	 */
	private Hashtable predecessor;
	
	/**
	 * set of vertices we already visited in the search
	 * was: HashSet< Vertex >
	 */
	private HashSet< Vertex > closedVertices;
	
	/**
	 * set of vertices we still need to visit in the search
	 */
	private HashSet< Vertex > openVertices;
	
	/**
	 * linked list holding the path from the source vertex to the target vertex 
	 */
	private LinkedList< Vertex > path ;

	/**
	 * default constructor
	 */
	public WeightedGraphSearch() {
		closedVertices = new HashSet< Vertex >();
		openVertices = new HashSet< Vertex >();
		weightToTarget = new Hashtable();
		predecessor = new Hashtable();
		path = new LinkedList<Vertex>();
	}
	
	/**
	 * runs a search to find a path between two vertices in the graph
	 * @param source the vertex we start the search from 
	 * @param dest the vertex we wish to reach
	 * @return list of vertices if a path exists. empty list if no path was found.
	 */
	public LinkedList< Vertex > searchGraph(Vertex source, Vertex dest) {
		// initialize search by putting the source vertex in the unvisited nodes list. 
		weightToTarget[source] = 0;
		openVertices.Add(source);
		// while there are nodes we haven't visited. 
		while( openVertices.Count != 0 ) {
			// get the vertex with minimal edge weight 
			Vertex v = getMinWeighingVertex(openVertices);
			// move it from unvisited to visited
			closedVertices.Add(v);
			openVertices.Remove(v);
			// go over the vertex's neighbors and search for a more optimal route
			findMinimalDistance(v);
		}
		
		// Once done, populate the list with the path and return
		return ( buildPath(dest) );
	}

	/**
	 * helper function that builds a linked list of vertices representing the shortest 
	 * (or most lightweight) path to destination. 
	 * @param dest the vertex we tried to reach.
	 * @return list of vertices if a path exists. empty list if no path was found.
	 */
	private LinkedList<Vertex> buildPath(Vertex dest) {
		// tidy up before we begin, just in case...
		path.Clear();
		Vertex next = dest;
		// if there is no path return an empty list 
		if( predecessor[next] == null ) {
			return( path );
		}
		// otherwise, keep adding the next node to the path  
		path.AddLast(next);
		while(predecessor[next] != null ) {
			next = (Vertex)predecessor[next];
			path.AddLast(next);
		}
		// don't forget to reverse it (we searched from target to source)...
		// Collections.reverse(path);
		// todo: currently dont need to reverse but some day maybe
		return ( path );
	}
	
	/**
	 * helper function that scans neighbors for a shorter path to a vertex
	 * @param v the vertex whose neighbors we will scan
	 */
	private void findMinimalDistance(Vertex v) {
		ICollection neighbors = v.getNeighbors();
		// check all neighbors of this vertex.
		foreach ( Vertex neighbor in neighbors ) {
			// if there is a shorter path to vertex v via this neighbor
			if( getWeightofPathtoVertex(neighbor) > getWeightofPathtoVertex(v) + getEdgeWeight(v,neighbor) ) {
				// update the distance
				weightToTarget[neighbor] = getWeightofPathtoVertex(v) + getEdgeWeight(v,neighbor);
				// mark the neighbor as the previous step in path to source vertex
				predecessor[neighbor] =  v;
				// and add neighbor to unvisited list.
				openVertices.Add(neighbor);
			}
		}
	}

	/**
	 * helper function that returns the weight of the edge between two vertices
	 * @param v1 the first vertex
	 * @param v1 the first vertex
	 * @return weight of edge between vertices
	 */
	private int getEdgeWeight(Vertex v1, Vertex v2) {
		return ( v1.getEdgeWeighToNeighbor(v2) );
	}
	
	/**
	 * helper function that finds the minimum weighing edge in a set.
	 * @param vertices list of nodes to check
	 * @return the vertex with the minimal weight
	 */
	private Vertex getMinWeighingVertex(HashSet<Vertex> vertices) {
		Vertex minWeightVertex = null;
		foreach ( Vertex v in vertices ) {
			if( minWeightVertex == null ) {
				minWeightVertex = v;
			} else {
				if(  getWeightofPathtoVertex(v) < getWeightofPathtoVertex(minWeightVertex) ) {
					minWeightVertex = v;
				}
			}
		}
		return( minWeightVertex );
	}

	/**
	 * helper function that returns the weight to vertex
	 * @param v vertex to check distance to.
	 * @return the weight to the vertex of max value of integer if it wasn't added yet.
	 */
	private int getWeightofPathtoVertex(Vertex v) {
		if ( !weightToTarget.ContainsKey(v) ) {
			return int.MaxValue;
		} else {
			return (int) weightToTarget[v];
		 }
		
/*		
		int distance = (int) weightToTarget[v];
		if( distance == null ) { 
			return ( int.MaxValue );
		} else {
			return ( distance ) ;
		}
*/
	} 
	
}

