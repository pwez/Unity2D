using UnityEngine;

namespace State.Interactive.Lifting {
	public class LiftingState : BaseState {

		[Header("Lift Parameters")] 
		public Transform liftPosition;
		public LayerMask liftLayers;
		public float liftDuration;
		
		private float counter;
		
		public override void Enter(Player player) {
			base.Enter(player);
			player.physics.velocity.x = 0f;
			liftDuration = audioClip.length;
			counter = 0f;
		}

		public override void Resume(Player player) {
			if (counter < liftDuration) {
				counter += Time.deltaTime;
				if (counter >= liftDuration)
					player.stateMachine.Toggle<CarryingState>();
			}
		}

		public override void Exit(Player player) {
			base.Exit(player);
			PositionLiftedObjectAbove();
			player.physics.canLift = false;
			player.physics.isGrounded = false;
		}
		
		private void PositionLiftedObjectAbove() {
			var element = transform;
			var position = element.position;
			var ray = Physics2D.Raycast(position, -element.up, Mathf.Infinity, liftLayers);
			if (ray && ray.collider.tag.Equals("Lift")) {
				var objectToLift = ray.collider.gameObject;
				objectToLift.transform.position = liftPosition.position;
				objectToLift.transform.parent = liftPosition.transform;
				Physics2D.IgnoreCollision(objectToLift.GetComponent<BoxCollider2D>(), GetComponentInParent<BoxCollider2D>(), true);
			}
		}
	}
}