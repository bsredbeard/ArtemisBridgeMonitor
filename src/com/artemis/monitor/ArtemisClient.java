package com.artemis.monitor;

import java.io.IOException;

import net.dhleong.acl.ThreadedArtemisNetworkInterface;
import net.dhleong.acl.enums.BridgeStation;
import net.dhleong.acl.net.setup.ReadyPacket;
import net.dhleong.acl.net.setup.SetShipPacket;
import net.dhleong.acl.net.setup.SetStationPacket;

public class ArtemisClient {

	public EventListenerProxy eventProxy = new EventListenerProxy();
	private ThreadedArtemisNetworkInterface server = null;
	
	public void AddListener(IArtemisEventListener listener){
		this.eventProxy.AddListener(listener);
	}
	
	public void RemoveListener(IArtemisEventListener listener){
		this.eventProxy.RemoveListener(listener);
	}
	
	
	public boolean Connect(String host, int port){
		boolean connected = false;
		if(!this.IsConnected()){
			try {
				this.server = new ThreadedArtemisNetworkInterface(host, port);
				this.server.setOnConnectedListener(new ConnectionListener(this.eventProxy));
				this.server.addPacketListener(new GameListener(this.eventProxy));
				
				this.server.start();
				this.server.send(new SetStationPacket(BridgeStation.OBSERVER, true));
				this.server.send(new ReadyPacket());
				connected = true;
			} catch (IOException e) {
				//do not care about the actual exception
			}
		}
		
		return connected;
	}
	
	public boolean IsConnected(){
		return this.server!=null && this.server.isConnected();
	}
	
	public void Disconnect(){
		if(this.server != null && this.server.isConnected()){
			this.server.stop();
		}
	}
	
	public void SetShip(int shipIndex){
		this.server.send(new SetShipPacket(shipIndex));
		this.server.send(new SetStationPacket(BridgeStation.OBSERVER, true));
		this.server.send(new ReadyPacket());
	}
}
