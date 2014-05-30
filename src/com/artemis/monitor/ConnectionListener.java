package com.artemis.monitor;

import net.dhleong.acl.OnConnectedListener;

public class ConnectionListener implements OnConnectedListener {

	public IArtemisEventListener listener = null;
	
	public ConnectionListener(IArtemisEventListener cListener){
		this.listener = cListener;
	}
	
	@Override
	public void onDisconnected(int errorCode) {
		this.listener.ConnectionLost(errorCode != 0);
	}

	@Override
	public void onConnected() {
		this.listener.Connected();
	}

}
