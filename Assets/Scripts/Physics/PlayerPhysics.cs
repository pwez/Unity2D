using Physics.Collisions;
using UnityEngine;

namespace Physics {
	public class PlayerPhysics : Physics {
		
		[HideInInspector] public CollisionDetection collisionDetection;
		[HideInInspector] public ControllerInput controllerInput;

		public override void Awake() {
			base.Awake();
			collisionDetection = GetComponent<CollisionDetection>();
			controllerInput = GetComponent<ControllerInput>();
		}
		
		private void Start() {
			facingDirection = 1;
			movingDirection = wallDirection = ledgeDirection = 0;
		}

		public override void Simulate() {
			collisionDetection.Translate(velocity * Time.deltaTime);
			var collisionInfo = collisionDetection.collisionInfo;

			if (collisionInfo.below || collisionInfo.above) {
				velocityBeforeCollision.y = velocity.y;
				velocity.y = 0f;
			}
			if (collisionInfo.left || collisionInfo.right) {
				velocityBeforeCollision.x = velocity.x;
				velocity.x = 0f;
				if (controllerInput.dx != 0)
					wallDirection = collisionInfo.left ? -1 : 1;
				if (collisionInfo.ledge)
					ledgeDirection = collisionInfo.left ? -1 : 1;
			}

			isGrounded = collisionInfo.below;
			if (isGrounded) {
				coyoteTimeCounter = coyoteTime;
				ledgeDirection = 0;
			}

			movingDirection = velocity.x > 0 ? 1 : velocity.x < 0 ? -1 : 0;
			if (movingDirection == -wallDirection)
				wallDirection = 0;
		}

		public override void ApplyGravity() {
			if (simulating && gravityEnabled)
				velocity.y -= gravity * Time.deltaTime;
		}
		
		public override void ApplyGravity(float grv) {
			if (simulating && gravityEnabled) 
				velocity.y -= grv * Time.deltaTime;
		}

		public override void AccelerateX(float acceleration, float boundingValue) {
			velocity.x += acceleration * Time.deltaTime;
			velocity.x = acceleration > 0 ? 
				Mathf.Min(velocity.x, boundingValue) : 
				Mathf.Max(velocity.x, boundingValue);
		}
	}
}