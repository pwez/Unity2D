namespace State.Machine {
	public interface StateMachine {
		void Add(State state);
		void Toggle<TS>() where TS : State;
		void Resume();
	}
}