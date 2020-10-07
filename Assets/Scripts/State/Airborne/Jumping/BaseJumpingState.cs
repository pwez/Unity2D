using System.Collections;
using UnityEngine;

namespace State.Airborne.Jumping {
	public class BaseJumpingState : JumpingState {
		
		public float minHeight;
		private float minJumpSpeed;

		[Header("Jump Squeeze")] 
		public Vector2 squeezeAmount = new Vector2(0.5f, 1.2f);
		public float squeezeDuration = 0.1f;
		
		public override void Enter(Player player) {
			base.Enter(player);
			minJumpSpeed = Mathf.Sqrt (2 * gravity * minHeight);
			StartCoroutine(Squeeze(squeezeAmount, squeezeDuration));
		}

		public override void Resume(Player player) {
			base.Resume(player);
			var physics = player.physics;
			var input = player.input;
			if (!input.commandHeld || input.commandReleased) {
				if (physics.velocity.y > minJumpSpeed)
					physics.velocity.y = minJumpSpeed;
			}
		}
		
	}
}