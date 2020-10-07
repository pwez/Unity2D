using State.Airborne;
using State.Attack.Special;
using UnityEngine;

namespace State.Grounded.Stationary.Crouching {
	public class CrouchingEnterState : CrouchingState {

		public override void Resume(Player player) {
			var stateMachine = player.stateMachine;
			var physics = player.physics;
			var input = player.input;
			physics.ApplyGravity();
			if (!input.down)
				stateMachine.Toggle<CrouchingExitState>();
			else if (input.commandPressed)
				stateMachine.Toggle<BackflipState>();
			else if (Input.GetKeyDown(KeyCode.U))
				stateMachine.Toggle<DownwardSpecialAttackState>();
		}

	}
}