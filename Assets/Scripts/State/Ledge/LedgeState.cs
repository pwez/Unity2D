namespace State.Ledge {
	public abstract class LedgeState : BaseState {
		public override void Enter(Player player) {
			base.Enter(player);
			player.physics.wallDirection = 0;
		}
	}
}