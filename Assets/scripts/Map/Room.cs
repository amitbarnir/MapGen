using System;
using UnityEngine;

public class Room : MonoBehaviour {

    public int MinXCoord { get; internal set; }
    public int MinZCoord { get; internal set; }
    public int XSize { get; internal set; }
    public int ZSize { get; internal set; }

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
}
