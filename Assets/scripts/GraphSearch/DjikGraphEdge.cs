using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The Edge class represents an edge in the graph.
 * It stores the two connected vertices and encapsulates the methods used to connect and disconnect them. 
 * it also holds the weight of the edge.
 * 
 * The class is meant to be light and disposable: 
 * you can only provide the vertices and weight upon creation as allowing to re-set member data 
 * would only cause unnecessary complications.    
 * @author Amit Barnir
 *
 */
public class DjikGraphEdge {
	/**
	 * The first vertex this edge connects.
	 */
	private VertexOld v1 ;

	/**
	 * The second vertex this edge connects.
	 */
	private VertexOld v2 ;
	
	/**
	 * The weight of traversing this edge.
	 */
	private int weight;
	
	/**
	 * Constructor for creating a weighted edge
	 * @param v1 The first vertex to connect
	 * @param v2 The second vertex to connect
	 * @param weight the weight of the edge
	 */

	public DjikGraphEdge(VertexOld v1,VertexOld v2, int weight) {
		this.v1 = v1;
		this.v2 = v2;
		this.weight = weight;
		// make sure the vertices register each other as neighbors.
		connectNeighbors(v1, v2);
	}

	/**
	 * Get the first Vertex
	 * @return the first vertex
	 */
	public VertexOld getFirstVertex() {
		return ( v1 );
	}
	
	/**
	 * Get the second vertex
	 * @return the second vertex
	 */
	public VertexOld getSecondVertex() {
		return ( v2 );
	}
	
	/**
	 * Get the edge weight
	 * @return the weight of the edge
	 */
	public int getWeight() {
		return weight;
	}

	/**
	 * Disconnect the two vertices
	 */
	public void disconnectNeighbors() {
		v1.removeNeighbor(v2);
		v2.removeNeighbor(v1);
	}
	
	/**
	 * helper function to connect two vertices
	 * @param v1 first vertex to connect
	 * @param v2 second vertex to connect
	 */
	private void connectNeighbors(VertexOld v1, VertexOld v2) {
		v1.addNeighbor(v2, this) ;
		v2.addNeighbor(v1, this) ;
	}
}
