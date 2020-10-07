using State.Attack.Special;
using State.Grounded.Stationary;
using UnityEngine;

namespace Players.Mario.States {
    // TODO there is no upward boost if velocity.y > 0 before entering state
    public class MarioUpwardSpecialAttackState : UpwardSpecialAttackState {

        [Header("Jump Parameters")]
        public float timeToHeight;
		public float maxHeight;
        public float horizontalSpeed;
        public float horizontalAcceleration;
		private float maxJumpSpeed;
		private float gravity;
        private float counter = 0f;

		private void Start() {
			CalculateJumpParameters();
		}

		private void CalculateJumpParameters() {
			gravity = 2 * maxHeight / Mathf.Pow (timeToHeight, 2);
			maxJumpSpeed = gravity * timeToHeight;
		}

        public override void Enter(Player player) {
            base.Enter(player);
			var physics = player.physics;
			physics.gravity = gravity;
            physics.velocity.x = physics.facingDirection * horizontalSpeed;
			physics.velocity.y = maxJumpSpeed;
        }

        public override void Resume(Player player) {
            var stateMachine = player.stateMachine;
            var physics = player.physics;
            physics.ApplyGravity();
            if (counter < animationClip.length)
                counter += Time.deltaTime;
            else if (counter >= animationClip.length) {
                if (physics.isGrounded)
                    stateMachine.Toggle<IdleState>();
            }
            if (physics.isGrounded)
                stateMachine.Toggle<IdleState>();
            else if (physics.velocity.y < 0f) {
                var input = player.input;
                if (input.left)
                    physics.AccelerateX(-horizontalAcceleration, -horizontalSpeed);
                else if (input.right)
                    physics.AccelerateX(horizontalAcceleration, horizontalSpeed);
                if (physics.velocity.y < -physics.maxVelocity.y * 0.75f)
                    physics.velocity.y = -physics.maxVelocity.y * 0.75f;
            }
        }

    }
}