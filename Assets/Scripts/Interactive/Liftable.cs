using UnityEngine;

namespace Interactive {
	public class Liftable : MonoBehaviour {
		private void OnCollisionEnter2D(Collision2D other) {
			var playerPhysics = other.gameObject.GetComponent<Physics.Physics>();
			if (playerPhysics) playerPhysics.canLift = true;
		}

		private void OnCollisionExit2D(Collision2D other) {
			var playerPhysics = other.gameObject.GetComponent<Physics.Physics>();
			if (playerPhysics) playerPhysics.canLift = false;
		}
	}
}