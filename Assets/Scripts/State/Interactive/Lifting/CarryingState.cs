using UnityEngine;

namespace State.Interactive.Lifting {
	public class CarryingState : BaseState {
		
		[Header("Horizontal Motion")]
		public float deceleration;

		public override void Resume(Player player) {
			base.Resume(player);
			var input = player.input;
			var physics = player.physics;
			physics.ApplyGravity();
			if (physics.isGrounded) {
				if (Input.GetKeyDown(KeyCode.K))
					player.stateMachine.Toggle<ThrowingState>();
				else if (input.left)
					physics.velocity.x = -physics.maxVelocity.x * 0.65f;
				else if (input.right)
					physics.velocity.x = physics.maxVelocity.x * 0.65f;
				else {
					if (physics.velocity.x > 0)
						physics.AccelerateX(-deceleration, 0);
					else if (physics.velocity.x < 0)
						physics.AccelerateX(deceleration, 0);
				}
			}
		}

	}
}