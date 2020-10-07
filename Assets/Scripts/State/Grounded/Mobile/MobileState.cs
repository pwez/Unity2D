using UnityEngine;

namespace State.Grounded.Mobile {
	public abstract class MobileState : GroundedState {

		[Header("Ground Particles On Moving")] 
		public ParticleSystem movingParticles;

		public override void Enter(Player player) {
			base.Enter(player);
			if (movingParticles)
				movingParticles.Play();
		}
		
		public override void Exit(Player player) {
			base.Enter(player);
			if (movingParticles)
				movingParticles.Stop();
		}
	}
}