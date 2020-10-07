using State.Airborne;
using UnityEngine;

namespace State.Ledge {
	public class LedgeJumpRecoverState : LedgeState {

		public float horizontalSpeed;
		public float height;
		public float timeToHeight;
		private float gravity;
		private float maxJumpSpeed;
		private int ledgeDirection;
		
		private void Start() {
			gravity = 2 * height / Mathf.Pow (timeToHeight, 2);
			maxJumpSpeed = gravity * timeToHeight;
		}

		public override void Enter(Player player) {
			base.Enter(player);
			var physics = player.physics;
			ledgeDirection = physics.ledgeDirection;
			physics.ledgeDirection = 0;
			physics.velocity.y = maxJumpSpeed;
			physics.gravity = gravity;
		}

		public override void Resume(Player player) {
			base.Resume(player);
			var physics = player.physics;
			physics.ApplyGravity();
			physics.velocity.x = horizontalSpeed * ledgeDirection;
			if (physics.velocity.y < 0)
				player.stateMachine.Toggle<FallingState>();
		}

		public override void Exit(Player player) {
			base.Exit(player);
			player.physics.ledgeDirection = 0;
		}
	}
}