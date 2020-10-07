using State.Machine;
using UnityEngine;

[RequireComponent(typeof(ControllerInput))]
[RequireComponent(typeof(Physics.Physics))]
public abstract class Player : MonoBehaviour {
	[HideInInspector] public ControllerInput input;
	[HideInInspector] public Physics.Physics physics;
	[HideInInspector] public StateMachine stateMachine;
}