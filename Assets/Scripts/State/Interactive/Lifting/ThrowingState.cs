using State.Airborne;
using State.Grounded.Mobile;
using State.Grounded.Stationary;
using UnityEngine;

namespace State.Interactive.Lifting {
	public class ThrowingState : BaseState {

		[Header("Throw Animations")] 
		public AnimationClip throwUpward;

		[Header("Throw Parameters")] 
		public Transform liftPosition;
		public bool applyGravityOnThrow;
		public Vector2 maxThrowVelocity;
		public float throwDuration;

		public AnimationClip throwAnimationClip;
		public Vector2 throwVelocity;
		
		private float counter;
		
		public override void Enter(Player player) {
			throwAnimationClip = animationClip;
			var input = player.input;
			var physics = player.physics;
			if (physics.isGrounded)
				physics.velocity.x = 0f;
			
			if (input.right)
				throwVelocity = new Vector2(maxThrowVelocity.x, 3f);
			else if (input.left)
				throwVelocity = new Vector2(-maxThrowVelocity.x, 3f);
			else if (input.up) {
				throwAnimationClip = throwUpward;
				throwVelocity = new Vector2(0, maxThrowVelocity.y);
			}
			else if (!physics.isGrounded) {
				if (input.down)
					throwVelocity = new Vector2(0, -maxThrowVelocity.y);
			}
			else
				throwVelocity = new Vector2(physics.facingDirection * maxThrowVelocity.x, 3f);

			if (throwAnimationClip && animator) {
				animator.enabled = true;
				animator.Play(throwAnimationClip.name);
			}
			throwDuration = throwAnimationClip.length;
			counter = 0f;
		}

		public void Throw() {
			var objectToThrow = liftPosition.transform.GetChild(0);
			var objectToThrowBody = objectToThrow.GetComponent<Rigidbody2D>();
			objectToThrowBody.constraints = RigidbodyConstraints2D.FreezeRotation;
			objectToThrow.parent = null;
			objectToThrowBody.velocity = throwVelocity;
		}
		
		public override void Resume(Player player) {
			var physics = player.physics;
			if (physics.isGrounded || applyGravityOnThrow)
				physics.ApplyGravity();
			if (counter < throwDuration) {
				counter += Time.deltaTime;
				if (counter >= throwDuration) {
					// Throw();
					var stateMachine = player.stateMachine;
					if (physics.isGrounded) {
						if (Mathf.Abs(physics.velocity.x) > 0)
							stateMachine.Toggle<WalkingState>();
						else
							stateMachine.Toggle<IdleState>();
					}
					else
						stateMachine.Toggle<FallingState>();
				}
			}
		}

	}
}