using UnityEngine;

namespace State.Airborne.Jumping {
	public abstract class JumpingState : AirborneState {
		
		[Header("Jump Particles")] 
		public ParticleSystem jumpParticles;
		
		[Header("Jump Parameters")]
		public float timeToHeight;
		public float maxHeight;
		[HideInInspector] public float maxJumpSpeed;
		[HideInInspector] public float gravity;

		private void Start() {
			CalculateJumpParameters();
		}

		private void CalculateJumpParameters() {
			gravity = 2 * maxHeight / Mathf.Pow (timeToHeight, 2);
			maxJumpSpeed = gravity * timeToHeight;
		}
		
		public override void Enter(Player player) {
			base.Enter(player);
			CalculateJumpParameters();
			var physics = player.physics;
			physics.gravity = gravity;
			physics.velocity.y = maxJumpSpeed;
			if (jumpParticles)
				jumpParticles.Play();
		}

		public override void Resume(Player player) {
			base.Resume(player);
			if (player.physics.velocity.y < 0)
				player.stateMachine.Toggle<FallingState>();
		}
	}
}