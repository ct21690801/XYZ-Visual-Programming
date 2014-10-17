// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;


public class ConnectorView
{

		public List<GameObject> TemporaryGeometry;
		
		public ConnectorView (Vector3 startpoint, Vector3 endpoint)
		{
			
				var range = Enumerable.Range (0, 100).Select (i => i / 100F).ToList ();
				var points = range.Select (x => Vector3.Slerp (startpoint, endpoint, x)).ToList ();
			
			
				var spheres = points.Select (x => {
						var y = GameObject.CreatePrimitive (PrimitiveType.Sphere);
						y.transform.position = x;
						return y;}).ToList ();

				TemporaryGeometry = spheres;
			
			
		}
		
}
