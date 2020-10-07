namespace State.Grounded.Stationary {
	public class LookingUpState : StationaryState {
		
		public override void Resume(Player player) {
			base.Resume(player);
			if(!player.input.up)
				player.stateMachine.Toggle<IdleState>();
		}
		
	}
}