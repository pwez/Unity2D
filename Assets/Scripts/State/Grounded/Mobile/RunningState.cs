using State.Airborne.Jumping;
using State.Grounded.Stationary;
using State.Grounded.Stationary.Crouching;
using UnityEngine;

namespace State.Grounded.Mobile {
	public class RunningState : MobileState {
		public override void Enter(Player player) {
			base.Enter(player);
			var physics = player.physics;
			var input = player.input;
			if (input.dx == -physics.facingDirection && input.dx != -physics.movingDirection)
				FlipScale(player);
		}

		public override void Resume(Player player) {
			base.Resume(player);
			var input = player.input;
			var physics = player.physics;
			var stateMachine = player.stateMachine;
			if (physics.wallDirection != 0 && input.dx == physics.wallDirection)
				stateMachine.Toggle<IdleState>();
			else if (input.left) {
				if (physics.velocity.x > 0f)
					stateMachine.Toggle<BrakingState>();
				else
					physics.velocity.x = -physics.maxVelocity.x;
			}
			else if (input.right) {
				if (physics.velocity.x < 0f)
					stateMachine.Toggle<BrakingState>();
				else
					physics.velocity.x = physics.maxVelocity.x;
			}
			else {
				// TODO come back to the nested if conditions
				if (physics.velocity.x > 0f) {
					physics.AccelerateX(-physics.friction, 0f);
					if (Mathf.Abs(physics.velocity.x) < 0f) 
						stateMachine.Toggle<IdleState>();
				}
				else if (physics.velocity.x < 0f) {
					physics.AccelerateX(physics.friction, 0f);
					if (Mathf.Abs(physics.velocity.x) < 0f) 
						stateMachine.Toggle<IdleState>();
				}
				else
					stateMachine.Toggle<IdleState>();
			}
		}

	}
}