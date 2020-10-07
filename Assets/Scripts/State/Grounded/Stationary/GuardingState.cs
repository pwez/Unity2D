using UnityEngine;

namespace State.Grounded.Stationary {
	
	public class GuardingState : GroundedState {
		
		public override void Resume(Player player) {
			base.Resume(player);
			var stateMachine = player.stateMachine;
			if (!Input.GetKey(KeyCode.Y) || Input.GetKeyUp(KeyCode.Y))
				stateMachine.Toggle<IdleState>();
			else if (player.input.down)
				stateMachine.Toggle<SpotDodgeState>();
		}
		
	}
}