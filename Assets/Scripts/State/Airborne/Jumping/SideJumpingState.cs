using State.Grounded.Stationary;
using State.Interactive;
using State.WallStates;
using UnityEngine;

namespace State.Airborne.Jumping {
	public class SideJumpingState : JumpingState {
		
		[Header("Horizontal Motion")]
		public float horizontalSpeed;
		public float maxHorizontalSpeed;
		public float minHorizontalSpeed;

		public override void Enter(Player player) {
			base.Enter(player);
			var direction = player.physics.facingDirection;
			player.physics.velocity = new Vector2(horizontalSpeed * -direction, maxJumpSpeed);
		}

		public override void Resume(Player player) {
			var input = player.input;
			var physics = player.physics;
			var stateMachine = player.stateMachine;
			physics.ApplyGravity(gravity);
			if (physics.isGrounded)
				stateMachine.Toggle<HardLandingState>();
			else if (physics.wallDirection != 0 && input.dx == physics.wallDirection) {
				FlipScale(player);
				stateMachine.Toggle<WallStickingState>();
			}
			else {
				if (input.left) {
					if (physics.velocity.x > 0)
						physics.AccelerateX(-horizontalDeceleration, minHorizontalSpeed);
					else if (physics.velocity.x < 0)
						physics.AccelerateX(-horizontalAcceleration, -maxHorizontalSpeed);
				}
				else if (input.right) {
					if (physics.velocity.x > 0)
						physics.AccelerateX(horizontalAcceleration, maxHorizontalSpeed);
					else if (physics.velocity.x < 0)
						physics.AccelerateX(horizontalDeceleration, -minHorizontalSpeed);
				}
				else if (input.up && physics.canClimb)
					stateMachine.Toggle<ClimbingState>();
			}
		}

	}
}