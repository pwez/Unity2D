using Physics.Collisions;
using UnityEngine;

namespace Physics {
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(CollisionDetection))]
	public abstract class Physics : MonoBehaviour, IPhysics {

		[Header("Config")]
		public bool simulating = true;
		public bool gravityEnabled = true;
		
		[Header("State")]
		public bool isGrounded;
		public bool canClimb;
		public bool canLift;

		[Header("Direction")] 
		public int facingDirection;
		public int movingDirection;
		public int wallDirection;
		public int ledgeDirection;
		
		[Header("Constants")]
		public float gravity;
		public float friction;
		public float drag;

		[Header("Velocity")]
		public Vector2 maxVelocity;
		[HideInInspector] public Vector2 velocity;
		[HideInInspector] public Vector2 velocityBeforeCollision;
		
		[Header("Buffers")]
		public float jumpBuffer;
		[HideInInspector] public float jumpCounter;
		public float coyoteTime;
		[HideInInspector] public float coyoteTimeCounter;
		
		[HideInInspector] public Rigidbody2D rigidbody2d;
		[HideInInspector] public BoxCollider2D boxCollider;

		public virtual void Awake() {
			rigidbody2d = GetComponent<Rigidbody2D>();
			boxCollider = GetComponent<BoxCollider2D>();
		}
		
		public abstract void Simulate();

		public abstract void ApplyGravity();

		public abstract void ApplyGravity(float grv);
		
		public abstract void AccelerateX(float acceleration, float boundingValue);

	}
}