using UnityEngine;

namespace Interactive {
	public class Climbable : MonoBehaviour {
		private void OnTriggerEnter2D(Collider2D other) {
			var playerPhysics = other.GetComponent<Physics.Physics>();
			if (playerPhysics) playerPhysics.canClimb = true;
		}

		private void OnTriggerExit2D(Collider2D other) {
			var playerPhysics = other.GetComponent<Physics.Physics>();
			if (playerPhysics) playerPhysics.canClimb = false;
		}
	}
}
