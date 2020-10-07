using State.Airborne.Jumping;
using State.Attack.Special;
using State.Grounded.Mobile;
using State.Grounded.Stationary;
using State.Interactive;
using State.Ledge;
using State.WallStates;
using UnityEngine;

namespace State.Airborne {
	public abstract class AirborneState : MotionState {

		[Header("Airborne Acceleration")] 
		public float horizontalAcceleration;
		public float horizontalDeceleration;
		
		public override void Enter(Player player) {
			base.Enter(player);
			player.physics.isGrounded = false;
		}

		public override void Resume(Player player) {
			base.Resume(player);
			var input = player.input;
			var stateMachine = player.stateMachine;
			var physics = player.physics;
			physics.ApplyGravity();
			if (physics.isGrounded) {
				if (physics.jumpCounter > 0) {
					physics.jumpCounter = 0f;
					stateMachine.Toggle<BaseJumpingState>();
				}
				else if (!input.left && !input.right && physics.velocityBeforeCollision.y <= -physics.maxVelocity.y)
					stateMachine.Toggle<HardLandingState>();
				else if (Mathf.Abs(physics.velocity.x) >= physics.maxVelocity.x)
					stateMachine.Toggle<RunningState>();
				else if (Mathf.Abs(physics.velocity.x) > 0f)
					stateMachine.Toggle<WalkingState>();
				else
					stateMachine.Toggle<IdleState>();
			}
			else if (physics.canClimb && input.up)
				stateMachine.Toggle<ClimbingState>();
			else if (physics.ledgeDirection != 0)
				stateMachine.Toggle<LedgeHangingState>();
			else if (physics.wallDirection != 0 && input.dx == physics.wallDirection)
				stateMachine.Toggle<WallStickingState>();
			else if (input.left) {
				var acceleration = physics.velocity.x < 0 ? horizontalAcceleration : horizontalDeceleration;
				physics.AccelerateX(input.x * acceleration, -physics.maxVelocity.x);
			}
			else if (input.right) {
				var acceleration = physics.velocity.x > 0 ? horizontalAcceleration : horizontalDeceleration;
				physics.AccelerateX(input.x * acceleration, physics.maxVelocity.x);
			}
			else {
				if (Mathf.Abs(physics.velocity.x) > 0f) {
					var dragDeceleration =
						physics.velocity.x > 0 ? -physics.drag :
						physics.velocity.x < 0 ? physics.drag : 0f;
					physics.AccelerateX(dragDeceleration, 0f);
				}
			}
			
			if (Input.GetKeyDown(KeyCode.Y)) {
				stateMachine.Toggle<AirborneDodgingState>();
				return;
			}
			if (input.commandPressed)
				physics.jumpCounter = physics.jumpBuffer;
			if (physics.jumpCounter > 0f) {
				physics.jumpCounter -= Time.deltaTime;
				if (physics.jumpCounter < 0f)
					physics.jumpCounter = 0f;
			}
		}

		public override void Exit(Player player) {}
		
	}
}