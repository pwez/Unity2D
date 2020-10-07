using State.Attack.Special;
using UnityEngine;

namespace State {
	public abstract class MotionState : BaseState {

        public override void Resume(Player player) {
            base.Resume(player);
			var physics = player.physics;
			var input = player.input;
			var stateMachine = player.stateMachine;
			if (Input.GetKeyDown(KeyCode.U)) {
				if(input.left || input.right)
					stateMachine.Toggle<ForwardSpecialAttackState>();
				else if (input.down)
					stateMachine.Toggle<DownwardSpecialAttackState>();
				else if (input.up)
					stateMachine.Toggle<UpwardSpecialAttackState>();
				else 
					stateMachine.Toggle<NeutralSpecialAttackState>();
			}
        }

	}
}