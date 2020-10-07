using State.Airborne;
using UnityEngine;

namespace State.Ledge {
	public class LedgeHangingState : LedgeState {
		
		public override void Enter(Player player) {
			base.Enter(player);
			var physics = player.physics;
			physics.velocity = Vector2.zero;
		}

		public override void Resume(Player player) {
			base.Resume(player);
			var input = player.input;
			var state = player.stateMachine;
			if (input.commandPressed)
				state.Toggle<LedgeJumpRecoverState>();
			else if (input.up)
				state.Toggle<LedgeClimbRecoverState>();
			else if (input.down) {
				player.physics.ledgeDirection = 0;
				state.Toggle<FallingState>();
			}
		}

	}
}