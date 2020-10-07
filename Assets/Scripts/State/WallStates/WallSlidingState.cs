using UnityEngine;

namespace State.WallStates {
	public class WallSlidingState : WallState {
		
		[Header("Wall Slide Parameters")]
		public float maxWallSlideSpeed;
		public float wallSlideAcceleration;
		public float wallSlideDeceleration;
		public float wallFriction;

		public override void Resume(Player player) {
			base.Resume(player);
			var physics = player.physics;
			var input = player.input;
			if (input.down) {
				physics.velocity.y -= wallSlideAcceleration * Time.deltaTime;
				physics.velocity.y = Mathf.Max(physics.velocity.y, -maxWallSlideSpeed);
			}
			else if (input.up && physics.velocity.y < 0f)
				physics.velocity.y += wallSlideDeceleration * Time.deltaTime;
			else if (physics.velocity.y < 0f)
				physics.velocity.y += wallFriction * Time.deltaTime;
			else if (physics.velocity.y >= 0f)
				player.stateMachine.Toggle<WallStickingState>();
		}
		
	}
}