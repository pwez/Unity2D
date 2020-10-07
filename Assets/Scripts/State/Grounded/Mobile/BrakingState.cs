using State.Airborne.Jumping;
using State.Grounded.Stationary;
using UnityEngine;

namespace State.Grounded.Mobile {
	public class BrakingState : MobileState {

		[Header("Deceleration")]
		public float brakeDeceleration;

		public override void Resume(Player player) {
			base.Resume(player);
			var physics = player.physics;
			var input = player.input;
			var stateMachine = player.stateMachine;
			if (input.commandPressed)
				stateMachine.Toggle<SideJumpingState>();
			else if (input.dx == -physics.movingDirection)
				physics.AccelerateX(input.x * brakeDeceleration, 0f);
			else if (Mathf.Abs(physics.velocity.x) > 0f)
				stateMachine.Toggle<WalkingState>();
			else
				stateMachine.Toggle<IdleState>();
		}

	}
}