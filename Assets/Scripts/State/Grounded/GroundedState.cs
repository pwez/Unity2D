using State.Airborne;
using State.Airborne.Jumping;
using UnityEngine;

namespace State.Grounded {
	public abstract class GroundedState : MotionState {
		
		public override void Enter(Player player) {
			base.Enter(player);
			player.physics.isGrounded = true;
		}

		public override void Resume(Player player) {
			base.Resume(player);
			var stateMachine = player.stateMachine;
			var physics = player.physics;
			physics.ApplyGravity();
			if (physics.isGrounded) {
				if (physics.jumpCounter > 0f || player.input.commandPressed) {
					player.physics.jumpCounter = 0f;
					player.physics.coyoteTimeCounter = 0f;
					player.stateMachine.Toggle<BaseJumpingState>();
				}
			}
			else {
				if (physics.coyoteTimeCounter > 0) {
					physics.velocity.y = 0f;
					physics.coyoteTimeCounter -= Time.deltaTime;
					if (physics.jumpCounter > 0f || player.input.commandPressed) {
						player.physics.jumpCounter = 0f;
						player.physics.coyoteTimeCounter = 0f;
						player.stateMachine.Toggle<BaseJumpingState>();
					}
				}
				else if (physics.velocity.y <= 0) {
					physics.coyoteTimeCounter = 0f;
					stateMachine.Toggle<FallingState>();
				}
			}
		}

	}
}