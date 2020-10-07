using State.Grounded.Stationary;
using UnityEngine;

namespace State.Grounded.Mobile {
	public class WalkingState : MobileState {
		
		public override void Enter(Player player) {
			base.Enter(player);
			var physics = player.physics;
			if (player.input.dx == -physics.facingDirection && player.input.dx != -physics.movingDirection)
				FlipScale(player);
		}
		
		public override void Resume(Player player) {
			base.Resume(player);
			var physics = player.physics;
			var input = player.input;
			var stateMachine = player.stateMachine;
			if (physics.wallDirection != 0 && input.dx == physics.wallDirection)
				stateMachine.Toggle<IdleState>();
			else if (!input.left && !input.right) {
				if (physics.velocity.x > 0f)
					physics.AccelerateX(-physics.friction, 0f);
				else if (physics.velocity.x < 0f)
					physics.AccelerateX(physics.friction, 0f);
				else
					stateMachine.Toggle<IdleState>();
			}
			else if (Mathf.Abs(input.x) >= 1f)
				stateMachine.Toggle<RunningState>();
			else
				physics.velocity.x = input.x * physics.maxVelocity.x;
		}
		
	}
}