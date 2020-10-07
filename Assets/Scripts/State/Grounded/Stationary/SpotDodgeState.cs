using UnityEngine;

namespace State.Grounded.Stationary {
	public class SpotDodgeState : GroundedState {

		private float counter;
		
		public override void Enter(Player player) {
			base.Enter(player);
			counter = 0f;
		}

		public override void Resume(Player player) {
			var stateMachine = player.stateMachine;
			var physics = player.physics;
			physics.ApplyGravity();
			if (counter < animationClip.length) {
				counter += Time.deltaTime;
				if (counter >= animationClip.length) {
					if (Input.GetKey(KeyCode.Y))
						stateMachine.Toggle<GuardingState>();
					else
						stateMachine.Toggle<IdleState>();
				}
			}
		}
	}
}