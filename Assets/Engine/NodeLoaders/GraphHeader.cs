using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using Nodeplay.Utils;
using UnityEngine;

namespace Nodeplay.Engine
{
	public class GraphHeader
	{
		private GraphHeader() { }
		
		public static bool FromXmlDocument(
			XmlDocument xmlDoc, string path, bool isTestMode, out GraphHeader workspaceInfo)
		{
			try
			{
				UnityEngine.Debug.Log("<color=orange>file load:</color>" + " constructing graph header by xml:" + path );

				string funName = null;
				float cx = 0;
				float cy = 0;
				float cz = 0;
				float zoom = 1.0f;
				string id = "";
				string category = "";
				string description = "";
				string version = "";
				
				var topNode = xmlDoc.GetElementsByTagName("Graph");
				
				// legacy support
				//if (topNode.Count == 0)
				//{
			//		topNode = xmlDoc.GetElementsByTagName("dynWorkspace");
			//	}
				
				// load the header
				foreach (XmlNode node in topNode)
				{
					foreach (XmlAttribute att in node.Attributes)
					{
						if (att.Name.Equals("X"))
							cx = float.Parse(att.Value, CultureInfo.InvariantCulture);
						else if (att.Name.Equals("Y"))
							cy = float.Parse(att.Value, CultureInfo.InvariantCulture);
						else if (att.Name.Equals("Z"))
							cz = float.Parse(att.Value, CultureInfo.InvariantCulture);
						//else if (att.Name.Equals("zoom"))
						//	zoom = double.Parse(att.Value, CultureInfo.InvariantCulture);
						else if (att.Name.Equals("Name"))
							funName = att.Value;
						else if (att.Name.Equals("ID"))
							id = att.Value;
						else if (att.Name.Equals("Category"))
							category = att.Value;
						else if (att.Name.Equals("Description"))
							description = att.Value;
						else if (att.Name.Equals("Version"))
							version = att.Value;
					}
				}
				
				// we have a dyf and it lacks an ID field, we need to assign it
				// a deterministic guid based on its name.  By doing it deterministically,
				// files remain compatible
				if (string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(funName) && funName != "Home")
				{
					id = GuidUtility.Create(GuidUtility.UrlNamespace, funName).ToString();
				}
				
				workspaceInfo = new GraphHeader
				{
					ID = id,
					Name = funName,
					X = cx,
					Y = cy,
					Zoom = zoom,
					FileName = path,
					Category = category,
					Description = description,
					Version = version
				};
				return true;
			}
			catch (Exception ex)
			{

				UnityEngine.Debug.Log(ex.Message + ":" + ex.StackTrace);
				
				//TODO(Steve): Need a better way to handle this kind of thing. -- MAGN-5712
				if (isTestMode)
					throw; // Rethrow for NUnit.
				
				workspaceInfo = null;
				return false;
			}
		}
		
		public string Version { get; private set; }
		public string Description { get; private set; }
		public string Category { get; private set; }
		public double X { get; private set; }
		public double Y { get; private set; }
		public double Zoom { get; private set; }
		public string Name { get; private set; }
		public string ID { get; private set; }
		public string FileName { get; private set; }
		
		public bool IsCustomNodeWorkspace
		{
			get { return !string.IsNullOrEmpty(ID); }
		}
	}
}