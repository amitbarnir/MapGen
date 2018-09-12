using System;
using UnityEngine;

public class Room : MonoBehaviour {

	private int xSize;
	private int zSize ;
//	private MapTile[,] roomTiles;

    public int MinXCoord { get; internal set; }
    public int MinZCoord { get; internal set; }
    public int XSize { get; internal set; }
    public int ZSize { get; internal set; }
    /*
    internal void addTile(int x, int z, MapTile tile)
    {
		if ( null == roomTiles ) {
			roomTiles = new MapTile[XSize,ZSize] ;
		}
        roomTiles[x,z] = tile;
    }
    */
    internal void updateCollider()
    {
		BoxCollider collider = GetComponent<BoxCollider>();
		Vector3 size = new Vector3(XSize,0.0f,ZSize);
		collider.size = size;
    }

    public Vector3 getCenter() {
        return new Vector3Int( MinXCoord + ( XSize / 2 ) , 0 , MinZCoord + ( ZSize / 2 ) );
    }

    internal bool areCoordsInRoom( Vector3 coords ) {
        if( coords.x >= this.MinXCoord && coords.x <= this.MinXCoord + this.XSize &&
            coords.z >= this.MinZCoord && coords.z <= this.MinZCoord + this.ZSize ) {
            return true;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
//       Gizmos.color = Color.green;
//		Vector3 roomSize = new Vector3(XSize,0.0f,ZSize);
//		BoxCollider bc = GetComponent<BoxCollider>();
//      Gizmos.DrawWireCube(GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size);
	}
}
