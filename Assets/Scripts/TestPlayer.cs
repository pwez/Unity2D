using Physics;
using State.Grounded.Stationary;
using State.Machine;

public class TestPlayer : Player {

	private void Awake() {
		physics = GetComponent<PlayerPhysics>();
		input = GetComponent<ControllerInput>();
		InitializeStateMachine();
	}

	private void Update() {
		if (!physics.simulating) return;
		stateMachine.Resume();
		physics.Simulate();
	}

	private void InitializeStateMachine() {
		stateMachine = new PlayerStateMachine(this);
		foreach (var state in gameObject.GetComponentsInChildren<State.State>())
			stateMachine.Add(state);
		stateMachine.Toggle<IdleState>();
	}
}