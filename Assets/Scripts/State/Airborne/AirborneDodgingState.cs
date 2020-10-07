using System.Collections;
using State.Grounded.Mobile;
using State.Grounded.Stationary;
using UnityEngine;

namespace State.Airborne {
	public class AirborneDodgingState : AirborneState {

		private float counter;

		public override void Enter(Player player) {
			base.Enter(player);
			counter = 0f;
		}

		public override void Resume(Player player) {
			var input = player.input;
			var stateMachine = player.stateMachine;
			var physics = player.physics;
			physics.ApplyGravity();
			if (input.left) {
				var acceleration = physics.velocity.x < 0 ? horizontalAcceleration : horizontalDeceleration;
				physics.AccelerateX(input.x * acceleration, -physics.maxVelocity.x);
			}
			else if (input.right) {
				var acceleration = physics.velocity.x > 0 ? horizontalAcceleration : horizontalDeceleration;
				physics.AccelerateX(input.x * acceleration, physics.maxVelocity.x);
			}
			if (counter < animationClip.length) {
				counter += Time.deltaTime;
				if (counter >= animationClip.length) {
					stateMachine.Toggle<FallingState>();
					return;
				}
			}
			if (physics.isGrounded) {
				if (Mathf.Abs(physics.velocity.x) > 0)
					stateMachine.Toggle<WalkingState>();
				else
					stateMachine.Toggle<IdleState>();
			}
		}

		public override void Exit(Player player) {
			base.Exit(player);
			spriteRenderer.enabled = true;
		}

	}
}