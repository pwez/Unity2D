
namespace State.WallStates {
	public class WallStickingState : WallState {

		public override void Resume(Player player) {
			base.Resume(player);
			if (player.input.down)
				player.stateMachine.Toggle<WallSlidingState>();
		}

	}
}