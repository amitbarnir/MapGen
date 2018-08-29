using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo {

	private MapTile tile;
	private Vertex vertex;
	private int x;
	private int z;

	public TileInfo(int x, int z, Vertex v) {
		this.x = x;
		this.z = z;
		this.tile = null;
		this.vertex = v;
	}

	public TileInfo(int x, int z, MapTile tile, Vertex v) {
		this.x = x;
		this.z = z;
		this.tile = tile;
		this.vertex = v;
	}

    public MapTile Tile {
        get  { return tile ; }
        set  {tile = value; }
    }

    public int X {
        get { return x; }
        set { x = value; }
    }

    public int Z {
        get { return z;}
        set {z = value;}
    }

    public Vertex Vertex {
        get { return vertex;}
        set { vertex = value;}
    }

    public String toString() {
		return x + "." + z;
	}
}
