using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawWireFrame : MonoBehaviour {

	public GameObject target;
  	public bool _bIsSelected = true;	
	void OnDrawGizmos()
   {
     if (_bIsSelected)
       OnDrawGizmosSelected() ;
   }

      void OnDrawGizmosSelected()
   {
     Gizmos.color = Color.yellow;
     Gizmos.DrawSphere(transform.position, 0.1f);  //center sphere
     if (GetComponent<Renderer>() != null)
       Gizmos.DrawWireCube(transform.position, GetComponent<Renderer>().bounds.size);
   }
}
