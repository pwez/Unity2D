using State.Airborne.Jumping;
using State.Grounded.Stationary;
using UnityEngine;

namespace State.Airborne {
	public class BackflipState : JumpingState {
		
		[Header("Speed Parameters")] 
		public float horizontalSpeed;
		public float maxHorizontalSpeed;
		public float minHorizontalSpeed;

		[Header("Squeeze Parameters")] 
		public Vector2 squeezeAmount;
		public float squeezeDuration;
		
		public override void Enter(Player player) {
			base.Enter(player);
			var physics = player.physics;
			var direction = physics.facingDirection;
			physics.velocity = new Vector2(horizontalSpeed * -direction, maxJumpSpeed);
			StartCoroutine(Squeeze(squeezeAmount, squeezeDuration));
		}

		public override void Resume(Player player) {
			var input = player.input;
			var physics = player.physics;
			var stateMachine = player.stateMachine;
			physics.ApplyGravity(gravity);
			if (physics.isGrounded)
				stateMachine.Toggle<HardLandingState>();
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