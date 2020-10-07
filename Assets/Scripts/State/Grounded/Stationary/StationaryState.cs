namespace State.Grounded.Stationary {
	public class StationaryState : GroundedState {
		public override void Enter(Player player) {
			base.Enter(player);
			player.physics.velocity.x = 0f;
		}
	}
}