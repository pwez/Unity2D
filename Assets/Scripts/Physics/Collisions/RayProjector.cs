using UnityEngine;

namespace Physics.Collisions {
	public abstract class RayProjector : MonoBehaviour {

		[Header("Ray Cast Config")]
		public float skinWidth = 0.015f;
		public float distanceBetweenRays = 0.25f;

		[Header("Bounds Config")] 
		public Bounds currentBounds;
		private Vector2 currentOffset;
		public CollisionBounds[] collisionBounds;
		
		[Header("Debug")] 
		public bool debug;
		public float debugLength;
		public Color raysXColor;
		public Color raysYColor;
		public Color boxColor;
		
		protected int horizontalRayCount;
		protected int verticalRayCount;
		protected float horizontalRaySpacing;
		protected float verticalRaySpacing;
		
		public RayOrigins rayOrigins;
		protected BoxCollider2D boxCollider;
		
		public virtual void Awake() {
			boxCollider = GetComponent<BoxCollider2D>();
			ToggleBounds(0);
		}
		
		public virtual void Start() {
			CalculateRaySpacing();
		}

		public virtual void Update() {
			UpdateRayOrigins();
			
			if (!debug) return;
			DrawCornerRays();
			DrawHorizontalRays();
			DrawVerticalRays();
		}

		public void ToggleBounds(int index) {
			currentBounds = collisionBounds[index].bounds;
			currentBounds.center = collisionBounds[index].offset;
			currentOffset = collisionBounds[index].offset;
			boxCollider.size = currentBounds.size;
			boxCollider.offset = currentOffset;
			CalculateRaySpacing();
		}

		public void ToggleBounds(CollisionBounds bounds) {
			currentBounds = bounds.bounds;
			currentBounds.center = bounds.offset;
			currentOffset = bounds.offset;
			boxCollider.size = currentBounds.size;
			boxCollider.offset = currentOffset;
			CalculateRaySpacing();
		}
		
		private void CalculateRaySpacing() {
			var colliderBounds = currentBounds;
			colliderBounds.Expand(-2 * skinWidth);
			var size = colliderBounds.size;
			var width = size.x;
			var height = size.y;
			horizontalRayCount = Mathf.RoundToInt(height / distanceBetweenRays);
			verticalRayCount = Mathf.RoundToInt(width / distanceBetweenRays);
			horizontalRaySpacing = height / (horizontalRayCount - 1);
			verticalRaySpacing = width / (verticalRayCount - 1);
		}

		protected void UpdateRayOrigins() {
			var element = transform;
			var angle = element.rotation.eulerAngles.z * Mathf.Deg2Rad;
			var bounds = new Bounds(currentBounds.center, currentBounds.size);
			bounds.Expand(-2 * skinWidth);
			var extents = bounds.extents;
			rayOrigins.topLeft = RotateVector(-extents.x, extents.y, angle);
			rayOrigins.topRight = RotateVector(extents.x, extents.y, angle);
			rayOrigins.bottomLeft = RotateVector(-extents.x, -extents.y, angle);
			rayOrigins.bottomRight = RotateVector(extents.x, -extents.y, angle);
		}

		public struct RayOrigins {
			public Vector2 topLeft;
			public Vector2 topRight;
			public Vector2 bottomLeft;
			public Vector2 bottomRight;
		}

		protected Vector2 RotateVector(float x, float y, float angle) {
			var vertex = new Vector2(x, y);
			var vertexRotationX = vertex.x * Cos(angle) - vertex.y * Sin(angle);
			var vertexRotationY = vertex.x * Sin(angle) + vertex.y * Cos(angle);
			return new Vector2(vertexRotationX, vertexRotationY) + (Vector2) transform.position;
		}
		
		private static float Cos(float angle) {
			return Mathf.Cos(angle);
		}
		
		private static float Sin(float angle) {
			return Mathf.Sin(angle);
		}
		
		private void DrawCornerRays() {
			var element = transform;
			var up = element.up;
			var right = element.right;
			
			Debug.DrawRay(rayOrigins.topLeft + currentOffset, up * debugLength, Color.yellow);
			Debug.DrawRay(rayOrigins.topLeft + currentOffset, -right * debugLength, Color.yellow);
			
			Debug.DrawRay(rayOrigins.bottomLeft + currentOffset, -up * debugLength, Color.green);
			Debug.DrawRay(rayOrigins.bottomLeft + currentOffset, -right * debugLength, Color.green);
			
			Debug.DrawRay(rayOrigins.topRight + currentOffset, up * debugLength, Color.red);
			Debug.DrawRay(rayOrigins.topRight + currentOffset, right * debugLength, Color.red);
			
			Debug.DrawRay(rayOrigins.bottomRight + currentOffset, -up * debugLength, Color.cyan);
			Debug.DrawRay(rayOrigins.bottomRight + currentOffset, right * debugLength, Color.cyan);
		}

		private void DrawHorizontalRays() {
			var length = debugLength + skinWidth;
			var destination = transform.right;
			for(var x = 0; x < horizontalRayCount; x++) {
				var origin = rayOrigins.bottomRight;
				origin +=(Vector2) transform.up * (horizontalRaySpacing * x);
				origin += currentOffset;
				Debug.DrawRay(origin, destination * length, raysXColor);
			}
			destination = -transform.right;
			for(var x = 0; x < horizontalRayCount; x++) {
				var origin = rayOrigins.bottomLeft;
				origin +=(Vector2) transform.up * (horizontalRaySpacing * x);
				origin += currentOffset;
				Debug.DrawRay(origin, destination * length, raysXColor);
			}
		}

		private void DrawVerticalRays() {
			var lengthY = debugLength + skinWidth;
			var destinationY = transform.up;
			for(var y = 0; y < verticalRayCount; y++) {
				var origin = rayOrigins.topLeft;
				origin += (Vector2) transform.right * (verticalRaySpacing * y);
				origin += currentOffset;
				Debug.DrawRay(origin, destinationY * lengthY, raysYColor);
			}
			destinationY = -transform.up;
			for(var y = 0; y < verticalRayCount; y++) {
				var origin = rayOrigins.bottomLeft;
				origin += (Vector2) transform.right * (verticalRaySpacing * y);
				origin += currentOffset;
				Debug.DrawRay(origin, destinationY * lengthY, raysYColor);
			}
		}

		private void OnDrawGizmos() {
			if (!debug) return;
			var element = transform;
			var rotationMatrix = Matrix4x4.TRS(element.position, element.rotation, element.lossyScale);
			Gizmos.matrix = rotationMatrix; 
			Gizmos.color = boxColor;
			Gizmos.DrawWireCube(currentBounds.center, currentBounds.size);
		}
	}
}