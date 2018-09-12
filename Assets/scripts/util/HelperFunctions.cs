using TriangleNet.Geometry;
using UnityEngine;

public class HelperFunctions  {

    public static Vector3 VertexToVector3(Vertex v ) {
        return new Vector3( (float)v.X , 0.0f , (float)v.Y );
    }

}
