
namespace State.Airborne {
	public class FallingState : AirborneState {
		
		public override void Resume(Player player) {
			base.Resume(player);
			var physics = player.physics;
			if (physics.velocity.y < -physics.maxVelocity.y)
				physics.velocity.y = -physics.maxVelocity.y;
		}
		
	}
}