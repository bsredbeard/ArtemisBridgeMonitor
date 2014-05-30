package com.artemis.monitor;

import java.util.Hashtable;
import java.util.List;

import net.dhleong.acl.PacketListener;
import net.dhleong.acl.net.GameOverPacket;
import net.dhleong.acl.net.comms.CommsIncomingPacket;
import net.dhleong.acl.net.eng.EngGridUpdatePacket;
import net.dhleong.acl.net.eng.EngGridUpdatePacket.DamconStatus;
import net.dhleong.acl.net.eng.EngGridUpdatePacket.GridDamage;
import net.dhleong.acl.net.helm.JumpStatusPacket;
import net.dhleong.acl.net.player.MainPlayerUpdatePacket;
import net.dhleong.acl.net.setup.AllShipSettingsPacket;
import net.dhleong.acl.util.BoolState;
import net.dhleong.acl.world.Artemis;
import net.dhleong.acl.world.ArtemisPlayer;
//import com.mentalspike.artemis.mock.IArtemisEventListener;

public class GameListener {
	public boolean shields = false;
	public boolean redAlert = false;
	public boolean reverse = false;
	public Hashtable<String, Float> damage = new Hashtable<String, Float>();
	public Hashtable<Integer, Integer> damcon = new Hashtable<Integer, Integer>();
	public float shieldFrontStrength = -1;
	public float shieldFrontMax = -1;
	public float shieldRearStrength = -1;
	public float shieldRearMax = -1;
	public int warpFactor = 0;
	public float impulseSpeed = 0;
	public IArtemisEventListener listener = null;
	
	public GameListener(IArtemisEventListener cListener){
		this.listener = cListener;
	}
	
	public boolean boolStatus(BoolState state, boolean current){
		if(BoolState.isKnown(state)){
			return state.getBooleanValue();
		}
		return current;
	}
	
	@PacketListener
	public void onGameOver(GameOverPacket packet){
		this.listener.GameEnded();
	}
	@PacketListener
	public void onMainPlayerUpdate(MainPlayerUpdatePacket packet){
		ArtemisPlayer player = (ArtemisPlayer)packet.getObjects().get(0);
		
		boolean currentShieldState = this.boolStatus(player.getShieldsState(), this.shields);
		if(currentShieldState != this.shields){
			this.shields = currentShieldState;
			this.listener.Shields(this.shields);
		}
		
		boolean raState = this.boolStatus(player.getRedAlertState(), this.redAlert);
		if(raState != this.redAlert){
			this.redAlert = raState;
			this.listener.RedAlert(this.redAlert);
		}
		
		boolean revState = this.boolStatus(player.getReverseState(), this.reverse);
		if(revState != this.reverse){
			this.reverse = revState;
			this.listener.ReverseEngines(this.reverse);
		}
		
		boolean frontShieldsChanged = false;
		float sfv = player.getShieldsFront();
		if(sfv >= 0 && this.shieldFrontStrength != sfv){
			if(sfv == 0 || sfv > 0.1){ //filter noise
				if(this.shieldFrontStrength > -1){
					frontShieldsChanged = true;
				}
				this.shieldFrontStrength = sfv;
			}
		}
		float sfm = player.getShieldsFrontMax();
		if(sfm >= 0 && this.shieldFrontMax != sfm){
			if(this.shieldFrontMax > -1){
				frontShieldsChanged = true;
			}
			this.shieldFrontMax = sfm;
		}
		
		if(frontShieldsChanged){
			this.listener.FrontShield((double)this.shieldFrontStrength, (double)this.shieldFrontMax);
		}
		
		boolean rearShieldsChanged = false;
		float srv = player.getShieldsRear();
		if(srv >= 0 && this.shieldRearStrength != srv){
			if(srv==0 || srv > 0.1){ //filter noise
				if(this.shieldRearStrength > -1){
					rearShieldsChanged = true;
				}
				this.shieldRearStrength = srv;
			}
		}
		
		float srm = player.getShieldsRearMax();
		if(srm >= 0 && this.shieldRearMax != srm){
			if(this.shieldRearMax > -1){
				rearShieldsChanged = true;
			}
			this.shieldRearMax = srv;
		}
		
		if(rearShieldsChanged){
			this.listener.RearShield((double)this.shieldRearStrength, (double)this.shieldRearMax);
		}
		
		float impulse = player.getImpulse();
		if(impulse >= 0 && impulse != this.impulseSpeed){
			this.impulseSpeed = impulse;
			this.listener.Impulse((double)impulse);
		}
		
		int warp = player.getWarp();
		if(warp >= 0 && warp != this.warpFactor){
			this.warpFactor = warp;
			this.listener.Warp(warp);
		}
	}
	@PacketListener
	public void onAllShipSettings(AllShipSettingsPacket packet){
		String[] shipNames = new String[Artemis.SHIP_COUNT];
		for(int jk=0; jk < Artemis.SHIP_COUNT; jk++){
			shipNames[jk] = packet.getShipName(jk);
		}
		this.listener.ShipNames(shipNames);
	}
	@PacketListener
	public void onJumpStatus(JumpStatusPacket packet){
		if(packet.isCountdown()){
			this.listener.JumpCountdownBegin();
		} else {
			this.listener.JumpInitiated();
		}	
	}
	@PacketListener
	public void onIncomingComm(CommsIncomingPacket packet){
		this.listener.ReceiveMessage(packet.getFrom(), packet.getMessage(), packet.getPriority());
	}
	@PacketListener
	public void onEngGridUpdate(EngGridUpdatePacket packet){
		List<GridDamage> damageInfo = packet.getDamage();
		String coord = null;
		float dmgVal = 0;
		boolean receivedDamage = false;
		for(GridDamage dmg : damageInfo){
			//convert the coordinate to a string for easier human readability
			coord = dmg.coord.toString();
			
			if(this.damage.containsKey(coord)){
				//get the existing damage value
				dmgVal = this.damage.get(coord);
				if(dmgVal < dmg.damage){
					//damage has increased, the ship has been damaged
					this.damage.put(coord, dmg.damage);
					receivedDamage = true;
				} else if(dmgVal > dmg.damage){
					//DAMCON teams are repairing the damage
					if(dmg.damage > 0){
						//repairing damage
						this.damage.put(coord, dmg.damage);
					} else {
						//fully repaired
						this.damage.remove(coord);
					}
				}
			} else if(dmg.damage > 0) {
				//took some new damage
				this.damage.put(coord, dmg.damage);
				receivedDamage = true;
			}
		}
		if(receivedDamage){
			this.listener.Damaged();
		}
		
		boolean dcDecreased = false;
		List<DamconStatus> dcStatusList = packet.getDamcons();
		
		int teamNum, teamCount, prevTeamCount;
		for(DamconStatus dc : dcStatusList){
			teamNum = dc.getTeamNumber();
			teamCount = dc.getMembers();
			if(this.damcon.containsKey(teamNum)){
				prevTeamCount = this.damcon.get(teamNum);
//				if(prevTeamCount != teamCount){
//					System.out.println("Damcon Team " + teamNum + " changed from " + prevTeamCount + " to " + teamCount);
					if(teamCount < prevTeamCount){
						dcDecreased = true;
					}
//				}
			}/* else {
				System.out.println("Damcon Team " + teamNum + " starting with " + teamCount);
			}*/
			this.damcon.put(teamNum, teamCount);
		}
		
		if(dcDecreased){
			this.listener.DamConMemberDeath();
		}
	}
}
