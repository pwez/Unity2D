using State.Attack.Special;
using State.Grounded.Mobile;
using State.Grounded.Stationary.Crouching;
using State.Interactive;
using UnityEngine;

namespace State.Grounded.Stationary {
	public class IdleState : StationaryState {

		public override void Resume(Player player) {
			base.Resume(player);
			var input = player.input;
			var physics = player.physics;
			var stateMachine = player.stateMachine;
			if (input.left || input.right) {
				if (physics.wallDirection != 0 && input.dx == physics.wallDirection)
					return;
				if (Mathf.Abs(input.x) >= 1f)
					stateMachine.Toggle<RunningState>();
				else
					stateMachine.Toggle<WalkingState>();
			}
			else if (input.down)
				stateMachine.Toggle<CrouchingEnterState>();
			else if (input.up) {
				if (physics.canClimb)
					stateMachine.Toggle<ClimbingState>();
				else
					stateMachine.Toggle<LookingUpState>();
			}
			else if (Input.GetKey(KeyCode.Y))
				stateMachine.Toggle<GuardingState>();
		}
	}
}