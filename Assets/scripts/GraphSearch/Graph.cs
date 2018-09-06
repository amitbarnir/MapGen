using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * A Simple graph class that holds edges and vertices and allows manipulation of the graph. 
 * The class uses a HashMap that maps Vertices to their name. this is in order to make it easier 
 * to test the class. In a real life scenario we'd probably have some other UID that identifies 
 * the vertex (IP or MAC address, coordinates, etc...) as the map key.
 * @author Amit Barnir
 *
 */
public class Graph {


	/**
	 * a hashtable holding the graph vertices. 
	 * was: Hashtable<String,Vertex>
	 */
	private Hashtable vertices ;
	
	/**
	 * a set holding the the graph edges.
	 */
	private HashSet<DjikGraphEdge> edges ;
	
	/**
	 * default constructor.
	 */
	public Graph() {
		// instantiate private members.
		this.vertices = new Hashtable();
		this.edges = new HashSet<DjikGraphEdge>();
	}
	
	/**
	 * Add a new vertex to the graph 
	 * @param vertexName name of vertex
	 * @return true is succeeded, false if failed.
	 */
	public Boolean addVertex(String vertexName) {
		// if v already exists in graph, return false
		if( vertices.ContainsKey( vertexName ) ) {
			return ( false );
		}
		// otherwise try adding it
		VertexOld v = new VertexOld(vertexName);
		vertices.Add(vertexName, v);
		return ( true );
	}

    public Boolean addVertex( VertexOld v  ) {
        // if v already exists in graph, return false
        if ( vertices.ContainsKey( v.getName() ) )  {
            return ( false );
        }
        // otherwise try adding it
        vertices.Add( v.getName() , v );
        return ( true );
    }
    /**
	 * Remove a vertex from the graph
	 * @param vertexName - vertex to remove
	 * @return true if removed, false if couldn't remove
	 */
    public Boolean removeVertex(String vertexName) {
		// if v doesn't exist in graph return false
		if( ! vertices.ContainsKey( vertexName ) ) {
			return ( false );
		}

		// if it does exist, iterate over the edges connected to it and remove them
		VertexOld v = getVertex( vertexName );
		ICollection edgesToNeighbors = v.getConnectedEdges();
		
		foreach ( DjikGraphEdge e in edgesToNeighbors ) {
			removeEdge( e );
		}
		// and finally, once disconnected from its neighbors 
		// and all the connecting edges were removed, remove the vertex
		vertices.Remove( vertexName );
		return ( true );
	}
	
	/**
	 * Add a new weighted edge to the graph
	 * @param firstVertexName The first vertex this edge connects 
	 * @param secondVertexName The second vertex this edge connects
	 * @param weight the weight of the edge
	 * @return true if edge was added, false otherwise
	 */
	public Boolean addWeightedEdge(String firstVertexName, String secondVertexName,int weight) {
		// if the vertices don't exist in graph, can't connect them.
		if( ! vertices.ContainsKey(firstVertexName) || ! vertices.ContainsKey(secondVertexName) )
			return( false );
		// also, don't allow self-loops... 
		if( firstVertexName.Equals(secondVertexName) )
			return( false );
        // otherwise we are in the clear. connect the vertices via a new edge and return true.
        try {
            DjikGraphEdge e = new DjikGraphEdge( vertices[ firstVertexName ] as VertexOld , vertices[ secondVertexName ] as VertexOld , weight );
            edges.Add( e );
        } catch ( Exception ex ) {
            Debug.Log( "Could not connect " + firstVertexName + " and " + secondVertexName );
        }
		return( true );
	}

	/**
	 * add new unweighed edge to the graph.
	 * This simple implementation passes a default value of 1 to unweighed edges.
	 * In a real world application more it would probably be wiser have different subclasses inherit from 
	 * Graph (WeightedGraph, UnweightedGraph, DirectedGraph, etc...) or find a design pattern that suits 
	 * this specific situation...
	 * @param firstVertexName The first vertex this edge connects
	 * @param secondVertexName The second vertex this edge connects
	 * @return true if edge was added, false otherwise
	 */
	public Boolean addEdge(String firstVertexName, String secondVertexName) {
		return( addWeightedEdge(firstVertexName, secondVertexName, 1));
	}
	
	/**
	 * Removes an edge from the graph
	 * @param e Edge to remove
	 * @return true if edge was removed, false if it wasn't
	 */
	public Boolean removeEdge(DjikGraphEdge e) {
		// if the edge doesn't exist in the graph we can't remove it.
		if(! edges.Contains(e) ) {
			return( false );
		}
		// it does exist. make sure the vertices delete each other as neighbors and remove.
		e.disconnectNeighbors();
		edges.Remove(e);
		return( true );
	}
	
	/**
	 * get the specified vertex.
	 * @param vertexName the vertex to retrieve
	 * @return the Vertex identified by name. null if doesn't exist. 
	 */
	public VertexOld getVertex(String vertexName) {
		return vertices[vertexName] as VertexOld;
	}
	
	/**
	 * Check if two vertices in the graph are connected by an edge.
	 * @param firstVertexName The first vertex to check
	 * @param secondVertexName The second vertex to check
	 * @return true if connected, false if not.
	 */
	public Boolean checkIfNeighbors(String firstVertexName,String secondVertexName) {
		// if either vertex doesn't exist in graph, they aren't neighbors.
		if( ! vertices.ContainsKey(firstVertexName) || ! vertices.ContainsKey(secondVertexName)) {
			return ( false );
		}
		// ask  one of them if it is connected to the other and propagate its answer.
		VertexOld v1 = vertices[firstVertexName] as VertexOld;
		VertexOld v2 = vertices[secondVertexName] as VertexOld;
		return ( v1.hasNeighbor(v2) ) ;
	}
	
	/**
	 * get all edges coming out of a vertex. 
	 * @param vertexName the vertex to get edges of
	 * @return collection of edges connecting this vertex to its neighbors.
	 */
	public ICollection<DjikGraphEdge> getEdgesforVertex(String vertexName) {
		// if the vertex doesn't exist, return an empty list. otherwise return connected edges.
		VertexOld v = vertices[vertexName] as VertexOld; 
		return ( v  == null) ? null :  ( v.getConnectedEdges() as ICollection<DjikGraphEdge>);
	}
	
	/**
	 * getter for the edges of this graph.
	 * @return 
	 */
	public HashSet<DjikGraphEdge> getEdges() {
		// TODO: shallow or deep copy?
		return (  new  HashSet<DjikGraphEdge>(this.edges) ); 
	}
}


