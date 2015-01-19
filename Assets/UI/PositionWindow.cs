﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.ComponentModel;
using System;

namespace Nodeplay.UI
{


	public class PositionWindow : MonoBehaviour
	{

		public GameObject Model_GO;

		void Start()
		{
			Model_GO = this.GetComponentInParent<NodeModel>().gameObject;
			//subscribe to the model changes
			Model_GO.GetComponent<NodeModel>().PropertyChanged += NodePropertyChangeEventHandler;
		}

		/// this handler is used to respond to changes on the node
		// when the node is modified in some way we update the windows position
		public void NodePropertyChangeEventHandler(object sender, EventArgs args)
		{
			GenerateBounds(Model_GO.transform.Cast<Transform>().Select(t => t.gameObject).ToList());

		}

		public void GenerateBounds(List<GameObject> toBound)
		{
			Vector3 center = Vector3.zero;
			var allrenderers = toBound.SelectMany(x => x.GetComponentsInChildren<MeshRenderer>()).ToList();
			var totalBounds = allrenderers[0].bounds;
			foreach (Renderer ren in allrenderers)
			{
				center = center + ren.gameObject.transform.position;
				totalBounds.Encapsulate(ren.bounds);

			}
			center = center / (allrenderers.Count);
			var newPoint = center - new Vector3(0, totalBounds.size.y*1.5f, 0);
			this.gameObject.transform.position = newPoint;
			
		}

		
	}
}
