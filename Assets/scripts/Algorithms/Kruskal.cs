using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using TriangleNet;
using TriangleNet.Geometry;
using UnityEngine;

namespace Kruskal
{
    public class KEdge
    {
        public KVertex V1 { get; set; }
        public KVertex V2 { get; set; }
        public float weight { get; set; }
        public Edge edge { get; set; }
        public KEdge(KVertex src, KVertex dest, float wt , Edge e )
        {
            V1 = src;
            V2 = dest;
            weight = wt;
            edge = e;
        }
        public KEdge()
        {

        }
    }
    public class KVertex
    {
        public string Label { get; set; }
    }
    public class KGraph
    {
        public List<KEdge> Edgecoll = null;
        public KVertex[] vertcoll = null;

        public KGraph(int size)
        {
            vertcoll = new KVertex[size];
        }
    }
    class KSubsets
    {
        public KVertex parent { get; set; }
        public int rank { get; set; }
    }


    class Kruskal
    {
        //TODO: sort this mess and only use one graph / vertex/ edge type.
        // currently, programming time is the scarce resourece so I am doing a quick 
        // integration but this should be done in a normal fashion
        public List<Edge> run( TriangleNet.Mesh mesh ) {
            int vertexCount = mesh.vertices.Count;
            KGraph objGraph = new KGraph( vertexCount );
            KVertex[] vertcoll = objGraph.vertcoll;
            KEdge[] result = new KEdge[ vertexCount ];
            List<KEdge> edgecoll = new List<KEdge>();

            KEdge objEdge;
            KVertex v = null;
            
            // create mesh vertices and label them with x.y coords
            for ( int i = 0 ; i < vertexCount ; i++ ) {
                v = new KVertex();
                String coordsLabel = mesh.Vertices.ElementAt( i ).X.ToString() 
                    + "." +         mesh.Vertices.ElementAt( i ).Y.ToString();                         ;
                v.Label = coordsLabel;
                vertcoll[ i ] = v;
            }

            // for each edge
            foreach ( Edge e in mesh.Edges ) {
                int firstVertexIndex = -1;
                int secondVertexIndex = -1;
                // construct the label using the coords
                Vertex v0 = mesh.vertices[ e.P0 ];
                Vertex v1 = mesh.vertices[ e.P1 ];
                String coordsLabel1 = v0.X.ToString() + "." + v0.Y.ToString();
                String coordsLabel2 = v1.X.ToString() + "." + v1.Y.ToString();
                // then find the vertex in the vertex array
                // TODO: change the array into dictionary for quicker access.
                for ( int i = 0 ; i < vertexCount ; i++ ) {
                        if( coordsLabel1.Equals(vertcoll[i].Label) ) {
                            firstVertexIndex = i;
                        }

                        if( coordsLabel2.Equals(vertcoll[i].Label)) {
                            secondVertexIndex = i;
                        }
                }
                // if one of the vertices is missing, something went horribly wrong.
                if(firstVertexIndex == -1 || secondVertexIndex == -1 || firstVertexIndex == secondVertexIndex) {
                        throw new ArgumentException( "Could not found Edge's Vertices" );
                }
                // calculate distance between the vertices and use it as edge weight.
                Vector3 p0 = new Vector3( (float)v0.x , 0.0f , (float)v0.y );
                Vector3 p1 = new Vector3( (float)v1.x , 0.0f , (float)v1.y );
                float weight = Vector3.Distance( p0 , p1 );
                objEdge = new KEdge( vertcoll[ firstVertexIndex ] , vertcoll[ secondVertexIndex ] , weight,e );
                edgecoll.Add( objEdge );
            }
            // sort edges by weight
            objGraph.Edgecoll = edgecoll.ToList().OrderBy( p => p.weight ).ToList();
            KSubsets[] sub = new KSubsets[ vertexCount ];
            KSubsets subobj;
            for ( int i = 0 ; i < vertexCount ; i++ ) {
                subobj = new KSubsets();
                subobj.parent = vertcoll[ i ];
                subobj.rank = 0;
                sub[ i ] = subobj;
            }
            int k = 0;
            int eCounter = 0;
            while ( eCounter < vertexCount - 1 ) {
                objEdge = objGraph.Edgecoll.ElementAt( k );
                KVertex x = find( sub , objEdge.V1 , Array.IndexOf( objGraph.vertcoll , objEdge.V1 ) , objGraph.vertcoll );
                KVertex y = find( sub , objEdge.V2 , Array.IndexOf( objGraph.vertcoll , objEdge.V2 ) , objGraph.vertcoll );
                if ( x != y ) {
                    result[ eCounter ] = objEdge;
                    Union( sub , x , y , objGraph.vertcoll );
                    eCounter++;
                }
                k++;
            }
            List<Edge> retval = new List<Edge>();
            for ( int i = 0 ; i < eCounter ; i++ ) {
                retval.Add(result[i].edge);
                Console.WriteLine( "edge from src:{0} to dest:{1} with weight:{2}" , result[ i ].V1.Label , result[ i ].V2.Label , result[ i ].weight );
            }
            return retval;
        }

/*
        static void Main(string[] args)
        {
            int k = 1;
            int vert =7 ;
            int e=0;
            KGraph objGraph = new KGraph(vert);
            KVertex[] vertcoll = objGraph.vertcoll;
            KEdge[] result=new KEdge[vert];
             
      List< KEdge> edgecoll = new List<KEdge>();
            KEdge objEdge = new KEdge();

            for (int i = 0; i < vert; i++)
            {
                for (int j = i; j < vert; j++)
                {
                    if (i != j)
                    {
                        Console.WriteLine("KEdge weight from src{0} to destn{1}", i, j);
                        int wt = int.Parse(Console.ReadLine());
                        if (wt == 0) continue;
                        objEdge = new KEdge(vertcoll[i], vertcoll[j], wt,null);
                        edgecoll.Add(objEdge);
                        k++;
                    }
                }
            }
        
          //edgecoll.ToList().OrderBy(p => p.weight).ToList();
            
        objGraph.Edgecoll = edgecoll.ToList().OrderBy(p => p.weight).ToList();//edgecoll.OrderBy(g=>g.weight).ToList();
            
            KSubsets[] sub = new KSubsets[vert];
            KSubsets subobj;
            for (int i = 0; i < vert; i++)
            {
                subobj = new KSubsets();
                subobj.parent = vertcoll[i];
                subobj.rank = 0;
                sub[i] = subobj;
            }
            k = 0;
            while (e < vert - 1)
            {
                objEdge = objGraph.Edgecoll.ElementAt(k);
                KVertex x = find(sub, objEdge.V1,Array.IndexOf(objGraph.vertcoll,objEdge.V1),objGraph.vertcoll);
                KVertex y = find(sub, objEdge.V2, Array.IndexOf(objGraph.vertcoll, objEdge.V2), objGraph.vertcoll);
                if (x != y)
                {
                    result[e] = objEdge;
                    Union(sub, x, y, objGraph.vertcoll);
                    e++;
                }
                k++;
             
                
            }
           
            for (int i = 0; i < e; i++)
            {
                Console.WriteLine("edge from src:{0} to dest:{1} with weight:{2}",result[i].V1.Label,result[i].V2.Label,result[i].weight);
            }
            return;
        }
*/
        private static void Union(KSubsets[] sub, KVertex xr, KVertex yr, KVertex[] vertex)
        {
            KVertex x=  find(sub,xr,Array.IndexOf(vertex,xr),vertex);
            KVertex y = find(sub, yr, Array.IndexOf(vertex, yr), vertex);

            if (sub[Array.IndexOf(vertex, x)].rank < sub[Array.IndexOf(vertex, y)].rank)
            {
                sub[Array.IndexOf(vertex, x)].parent = y;
            }
            else if (sub[Array.IndexOf(vertex, x)].rank > sub[Array.IndexOf(vertex, y)].rank)
            {
                sub[Array.IndexOf(vertex, y)].parent = x;
            }
            else
            {
                sub[Array.IndexOf(vertex, y)].parent = x;
                sub[Array.IndexOf(vertex, x)].rank++;
            }
        }

        private static KVertex find(KSubsets[] sub, KVertex vertex, int k, KVertex[] vertdic)
        {
            if (sub[k].parent != vertex)
            {
                sub[k].parent = find(sub, sub.ElementAt(k).parent, Array.IndexOf(vertdic, sub.ElementAt(k).parent), vertdic);// find(sub, vertex, Array.IndexOf(vertdic,vertex),vertdic);//sub.Select(j => j.parent).Where(v => v.Label == vertex.Label).FirstOrDefault();
            }
        
            return  sub[k].parent;
        }
    }
}

 