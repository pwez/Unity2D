using State.Airborne;
using State.Airborne.Jumping;
using State.Grounded.Mobile;
using State.Grounded.Stationary;
using UnityEngine;

namespace State.Interactive {
	public class ClimbingState : BaseState {
		
		public float horizontalSpeed;
		public float verticalSpeed;
		
		public override void Enter(Player player) {
			base.Enter(player);
			player.physics.velocity = Vector2.zero;
			player.physics.isGrounded = false;
		}

		public override void Resume(Player player) {
			base.Resume(player);
			var physics = player.physics;
			var input = player.input;
			var stateMachine = player.stateMachine;

			if (!physics.canClimb)
				stateMachine.Toggle<FallingState>();
			else if (physics.isGrounded) {
				if (Mathf.Abs(physics.velocity.x) > 0)
					stateMachine.Toggle<WalkingState>();
				else 
					stateMachine.Toggle<IdleState>();
			}
			
			if (input.up || input.down) {
				ContinueAnimation();
				physics.velocity.y = (input.up ? 1 : -1) * verticalSpeed;
			}
			else 
				physics.velocity.y = 0f;
			
			if (input.left || input.right) {
				ContinueAnimation();
				physics.velocity.x = (input.right ? 1 : -1) * horizontalSpeed;
			}
			else 
				physics.velocity.x = 0f;
			
			if (!input.up && !input.down && !input.left && !input.right)
				ContinueAnimation(false);

			// ReSharper disable once InvertIf
			if (input.commandPressed) {
				if (physics.velocity.y < 0)
					stateMachine.Toggle<FallingState>();
				else
					stateMachine.Toggle<JumpingState>();
			}
		}
	}
}