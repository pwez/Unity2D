using State.Airborne.Jumping;
using State.Grounded.Mobile;
using State.Grounded.Stationary.Crouching;
using UnityEngine;

namespace State.Grounded.Stationary {
	public class HardLandingState : StationaryState {
		
		private float counter;
		
		public override void Enter(Player player) {
			base.Enter(player);
			counter = 0;
		}

		public override void Resume(Player player) {
			base.Resume(player);
			if (counter < animationClip.length) {
				counter += Time.deltaTime;
				if (counter >= animationClip.length)
					player.stateMachine.Toggle<IdleState>();
			}
			else if (counter <= animationClip.length * 0.75f) {
				var input = player.input;
				if (input.left || input.right)
					player.stateMachine.Toggle<WalkingState>();
				else if (input.down)
					player.stateMachine.Toggle<CrouchingEnterState>();
				else if (input.commandPressed)
					player.stateMachine.Toggle<JumpingState>();
			}
		}

	}
}