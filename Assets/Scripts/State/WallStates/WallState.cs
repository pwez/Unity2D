using State.Airborne;
using State.Airborne.Jumping;
using State.Grounded.Stationary;
using UnityEngine;

namespace State.WallStates {
	public abstract class WallState : BaseState {
		
		[Header("Jump & Release")]
		public float wallHorizontalLaunchSpeed;
		public float wallHorizontalReleaseSpeed;

		[Header("Wall Particles")] 
		public ParticleSystem wallParticles;
		
		private int wallDirection;
		
		public override void Enter(Player player) {
			base.Enter(player);
			CheckToFlipLocalScale(player);
			player.transform.rotation = Quaternion.identity;
			var physics = player.physics;
			physics.velocity = Vector2.zero;
			wallDirection = physics.wallDirection;
			if (wallParticles)
				wallParticles.Play();
		}
		
		public override void Resume(Player player) {
		 	var physics = player.physics;
			var stateMachine = player.stateMachine;
			var input = player.input;
			if (physics.isGrounded)
				stateMachine.Toggle<IdleState>();
			else if (wallDirection != 0) {
				if (input.dx != wallDirection) {
					physics.wallDirection = 0;
					physics.velocity.x = -wallDirection * wallHorizontalReleaseSpeed;
					stateMachine.Toggle<FallingState>();
				}
				else if (input.commandPressed) {
					if (physics.velocity.y < 0)
						stateMachine.Toggle<WallPushoffState>();
					else {
						physics.wallDirection = 0;
						physics.velocity.x = -wallDirection * wallHorizontalLaunchSpeed;
						FlipScale(player);
						stateMachine.Toggle<BaseJumpingState>();
					}
				}
			}
		}
		
		public override void Exit(Player player) {
			if (wallParticles)
				wallParticles.Stop();
		}

	}
}