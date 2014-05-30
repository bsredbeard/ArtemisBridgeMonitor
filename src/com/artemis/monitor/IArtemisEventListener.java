package com.artemis.monitor;

public interface IArtemisEventListener {
	public abstract void Connected();
	
	public abstract void ConnectionLost(boolean badDisconnect);

	public abstract void GameEnded();

	public abstract void DamConMemberDeath();

	public abstract void Damaged();
	
	public abstract void FrontShield(double currentStrength, double maxStrength);

	public abstract void Impulse(double scale);
	
	public abstract void JumpCountdownBegin();

	public abstract void JumpInitiated();

	public abstract void RearShield(double currentStrength, double maxStrength);

	public abstract void ReceiveMessage(String from, String message, int priority);

	public abstract void RedAlert(boolean enabled);

	public abstract void ReverseEngines(boolean enabled);

	public abstract void Shields(boolean enabled);

	public abstract void ShipNames(String[] shipNames);

	public abstract void Warp(int factor);
}
