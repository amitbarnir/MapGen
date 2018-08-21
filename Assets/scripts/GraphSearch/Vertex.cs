using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The Vertex class represents a vertex (node) in the graph.
 *  
 * @author amit barnir
 *
 */

public class Vertex  {

	/**
	 * identifier of this vertex. 
	 */
	private String name;
	
	/**
	 * a hash table of neighbors of this vertex and the edge between them. 
	 * was : Hashtable<Vertex,Edge> 
	 */
	private Hashtable neighbors;

	/**
	 * constructor 
	 * @param name The name of this node
	 */
	public Vertex(String name) {
		this.name = name;
		this.neighbors = new Hashtable();
	}
	
	/**
	 * Gets vertex name
	 * @return the name representing this node.
	 */
	public String getName() {
		return name;
	}
	
	/**
	 * Sets vertex name.   
	 * @param name the new name for the node.
	 * 
	 */
	public void setName(String name) {
		this.name = name;
	}
	
	/**
	 * Check if vertex is directly connected to another vertex. 
	 * @param v the vertex to test.
	 * @return true if the vertices are directly connected. false otherwise
	 */
	public bool hasNeighbor(Vertex v) {
		return( neighbors.ContainsKey(v) );
	}
	
	/**
	 * Adds a neighbor
	 * @param v Neighboring vertex to add
	 * @param e The edge connecting the vertices
	 * @return true if succeeded connecting. false otherwise.
	 */
	public void addNeighbor(Vertex v,DjikGraphEdge e) {
		neighbors.Add(v,e);
	}
	
	/**
	 * get all edges connected to this vertex
	 * @return a collection of edges connected to this vertex
	 */
	public ICollection getConnectedEdges() {
		return( neighbors.Values );
	}
	
	/**
	 * get the neighbors of this vertex.
	 * @return a set of neighboring vertices.
	 */
	public ICollection getNeighbors() {
		return( neighbors.Keys );
	}
	
	/** 
	 * Removes a neighbor of this vertex. 
	 * @param v the vertex to remove
	 * @return true if succeeded, false if failed to remove
	 */
	public void removeNeighbor(Vertex v) {
		 neighbors.Remove(v);
	}

	/**
	 * get the weight of the edge to a neighbor.
	 * @param target The neighboring vertex 
	 * @return the weight of the edge between this vertex and the neighboring vertex. or 0 otherwise.
	 */
	public int getEdgeWeighToNeighbor(Vertex target) {
		if( ! neighbors.ContainsKey(target) ) {
		return 0;
	} else {
		DjikGraphEdge e = (DjikGraphEdge)  neighbors[target];
		return e.getWeight();
		}
	}
}
