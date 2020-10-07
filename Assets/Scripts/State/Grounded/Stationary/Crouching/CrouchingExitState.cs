using State.Grounded.Mobile;
using UnityEngine;

namespace State.Grounded.Stationary.Crouching {
	public class CrouchingExitState : CrouchingState {
		
		private float counter;

		public override void Enter(Player player) {
			base.Enter(player);
			counter = 0;
		}

		public override void Resume(Player player) {
			base.Resume(player);
			var stateMachine = player.stateMachine;
			var input = player.input;
			if (input.left || input.right)
				stateMachine.Toggle<WalkingState>();
			else if (counter < animationClip.length) { 
				counter += Time.deltaTime;
				if (counter >= animationClip.length)
					stateMachine.Toggle<IdleState>();
			}
		}

	}
}