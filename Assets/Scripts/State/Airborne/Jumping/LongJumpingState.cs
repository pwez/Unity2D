using State.Grounded.Stationary;
using State.WallStates;
using UnityEngine;

namespace State.Airborne.Jumping {
	public class LongJumpingState : JumpingState {
		
		[Header("Horizontal Motion")]
		public float maxHorizontalSpeed;
		public float minHorizontalSpeed;
		
		public override void Resume(Player player) {
			var input = player.input;
			var physics = player.physics;
			var stateMachine = player.stateMachine;
			physics.ApplyGravity(gravity);
			if (physics.isGrounded)
				stateMachine.Toggle<HardLandingState>();
			else if (physics.wallDirection != 0 && input.dx == physics.wallDirection)
				stateMachine.Toggle<WallStickingState>();
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
			}
		}
	}
}