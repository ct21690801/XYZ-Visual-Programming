using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

using Nodeplay.Nodes;
using Nodeplay.Core;

/// <summary>
/// object representing an input port.
/// </summary>
public class ConnectorModel : BaseModel
{
		public  PortModel PStart { get; set; }
		public  PortModel PEnd { get; set; }
		public  ConnectorView View { get; set; }
		public GameObject UIsubgeo;	
		public Color modifierColor {get;protected set;}
		
		public delegate void ConnectorConnectionChangeHandler (object sender,EventArgs e);
		public event ConnectorConnectionChangeHandler ConnectorDisconnected;
		public event ConnectorConnectionChangeHandler ConnectorConnected;
		
		protected virtual void OnConnectorDisconnected (EventArgs e)
		{
			if (ConnectorDisconnected != null)
				ConnectorDisconnected (this, e);
			
		}
		
		protected virtual void OnConnectorConnected (EventArgs e)
		{
		if (ConnectorConnected != null)
			ConnectorConnected (this, e);
		}

		protected override void OnEnable ()
		{
				//create a connector view
            View = this.gameObject.AddComponent<ConnectorView>();
				View.Model = this;
		modifierColor = Color.black;
		}

		
		protected override void Update ()
		{
		
		}
		
		void OnDestroy(){


	}
	
	// Handles a port connect event, which in turn will tell the other port to connect to this connector
	
	protected void OnPortConnect(object sender, ConnectorModel e){

		OnConnectorConnected(EventArgs.Empty);

		
	}
	// Handles a port disconnect event

		protected void OnPortDisconnect(object sender, ConnectorModel e){

		if (e != this){
			return;
		}
		var p = sender as PortModel;
		//unregister all listeners
		p.PropertyChanged -= View.HandlePortChanges;

		
		p.PortDisconnected-= OnPortDisconnect;

		
		p.PortConnected -=OnPortConnect;

		
		
		ConnectorConnected-= p.ConnectorConnectEventHandler;

		
		ConnectorDisconnected-= p.ConnectorDisconnectEventHandler;

		//set the start and end to null
		//PStart = null;
		//PEnd = null;
		//send event that the connector is about to destroy itself
		OnConnectorDisconnected(EventArgs.Empty);
		GameObject.Destroy(this.gameObject);
		
	}
		public virtual void init (PortModel start, PortModel end)
		{
		
				PStart = start;
				PEnd = end;
				Debug.Log (start.NickName + " is the start");
				Debug.Log (end.NickName + " is the end");
				// hook listeners on the connector view to the ports
				View.EndPort = PEnd;
				View.StartPort = PStart;
				start.PropertyChanged += View.HandlePortChanges;
				end.PropertyChanged += View.HandlePortChanges;
				
				start.PortDisconnected+= OnPortDisconnect;
				end.PortDisconnected += OnPortDisconnect;
				
				start.PortConnected +=OnPortConnect;
				end.PortConnected +=OnPortConnect;

				// these ports are registered as listeners to connnection events
				// this lets the connector tell all ports about events occuring on the other
				//TODO simplify this logic
				ConnectorConnected+= start.ConnectorConnectEventHandler;
				ConnectorConnected+= end.ConnectorConnectEventHandler;
				
				ConnectorDisconnected+= start.ConnectorDisconnectEventHandler;
				ConnectorDisconnected+= end.ConnectorDisconnectEventHandler;

				//setup different colors for UI depending on type of object


				
		}
		//TODO add method to verify the ports that this connector connects
        
    public override GameObject BuildSceneElements()
        {

            GameObject UI = new GameObject();
            UI.transform.localPosition = this.gameObject.transform.position;
            UI.transform.parent = this.gameObject.transform;
			UIsubgeo = Resources.Load("connector_sub") as GameObject;
            var geo = View.redraw(UIsubgeo);
        //need to set these parents explicity since on first run redraw wont be able to 
        // nest these inside UI as UI is not created yet
            geo.ForEach(x => x.transform.parent = UI.transform);
		if (PStart.Owner is CreateVariable || PStart.Owner is GetVariable || PStart.Owner is SetVariable)
		{
			modifierColor = Color.green;
		}

            return UI;

        }

}