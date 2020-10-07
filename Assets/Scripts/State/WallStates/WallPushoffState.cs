using State.Airborne;

namespace State.WallStates {
	public class WallPushoffState : WallState {
		
		public override void Enter(Player player) {
			FlipScale(player);
			var physics = player.physics;
			physics.velocity.x = -physics.wallDirection * wallHorizontalLaunchSpeed;
			if (wallParticles)
				wallParticles.Play();
			player.stateMachine.Toggle<FallingState>();
		}
	}
}