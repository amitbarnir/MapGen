using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo {

	private MapTile tile;
	private VertexOld vertex;
	private int x;
	private int z;

	public TileInfo(int x, int z, VertexOld v) {
		this.x = x;
		this.z = z;
		this.tile = null;
		this.vertex = v;
	}

	public TileInfo(int x, int z, MapTile tile, VertexOld v) {
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

    public VertexOld Vertex {
        get { return vertex;}
        set { vertex = value;}
    }

    public String toString() {
		return x + "." + z;
	}
}
