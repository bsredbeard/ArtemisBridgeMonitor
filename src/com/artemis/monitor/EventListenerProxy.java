package com.artemis.monitor;

import java.util.ArrayList;

public class EventListenerProxy implements IArtemisEventListener {

	public ArrayList<IArtemisEventListener> listeners = new ArrayList<IArtemisEventListener>();
	
	public void AddListener(IArtemisEventListener listen){
		if(listen!=null && !this.listeners.contains(listen)){
			this.listeners.add(listen);
		}
	}
	
	public void RemoveListener(IArtemisEventListener listen){
		this.listeners.remove(listen);
	}
	
	@Override
	public void Connected(){
		for(IArtemisEventListener l : this.listeners){
			l.Connected();
		}
	}
	
	@Override
	public void ConnectionLost(boolean badDisconnect) {
		for(IArtemisEventListener l : this.listeners){
			l.ConnectionLost(badDisconnect);
		}
	}

	@Override
	public void DamConMemberDeath() {
		for(IArtemisEventListener l : this.listeners){
			l.DamConMemberDeath();
		}
	}

	@Override
	public void Damaged() {
		for(IArtemisEventListener l : this.listeners){
			l.Damaged();
		}
	}

	@Override
	public void FrontShield(double arg0, double arg1) {
		for(IArtemisEventListener l : this.listeners){
			l.FrontShield(arg0, arg1);
		}
	}

	@Override
	public void GameEnded() {
		for(IArtemisEventListener l : this.listeners){
			l.GameEnded();
		}
	}

	@Override
	public void Impulse(double arg0) {
		for(IArtemisEventListener l : this.listeners){
			l.Impulse(arg0);
		}
	}

	@Override
	public void JumpCountdownBegin() {
		for(IArtemisEventListener l : this.listeners){
			l.JumpCountdownBegin();
		}
	}

	@Override
	public void JumpInitiated() {
		for(IArtemisEventListener l : this.listeners){
			l.JumpInitiated();
		}
	}

	@Override
	public void RearShield(double arg0, double arg1) {
		for(IArtemisEventListener l : this.listeners){
			l.RearShield(arg0, arg1);
		}
	}

	@Override
	public void ReceiveMessage(String arg0, String arg1, int arg2) {
		for(IArtemisEventListener l : this.listeners){
			l.ReceiveMessage(arg0, arg1, arg2);
		}
	}

	@Override
	public void RedAlert(boolean arg0) {
		for(IArtemisEventListener l : this.listeners){
			l.RedAlert(arg0);
		}
	}

	@Override
	public void ReverseEngines(boolean arg0) {
		for(IArtemisEventListener l : this.listeners){
			l.ReverseEngines(arg0);
		}
	}

	@Override
	public void Shields(boolean arg0) {
		for(IArtemisEventListener l : this.listeners){
			l.Shields(arg0);
		}
	}

	@Override
	public void ShipNames(String[] arg0) {
		for(IArtemisEventListener l : this.listeners){
			l.ShipNames(arg0);
		}
	}

	@Override
	public void Warp(int arg0) {
		for(IArtemisEventListener l : this.listeners){
			l.Warp(arg0);
		}
	}

}
