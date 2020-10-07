using State.Airborne;
using State.Attack.Special;
using State.Grounded.Stationary;
using UnityEngine;

namespace Players.Mario.States {
    public class MarioNeutralSpecialAttackState : NeutralSpecialAttackState {

        [Header("Projectile Parameters")]
        public GameObject projectile;
        public Vector2 offset;
        private int facingDirection;
        private float counter;

        public override void Enter(Player player) {
            base.Enter(player);
            var position = (Vector2) transform.position;
            var physics = player.physics;
            if (physics.isGrounded)
                physics.velocity.x = 0f;
            counter = 0f;
        }

        public override void Resume(Player player) {
            var stateMachine = player.stateMachine;
            var physics = player.physics;
            physics.ApplyGravity();
            if (counter < animationClip.length) {
                counter += Time.deltaTime;
                if (counter >= animationClip.length) {
                    if (physics.isGrounded)
                        stateMachine.Toggle<IdleState>();
                    else
                        stateMachine.Toggle<FallingState>();
                }
            }
        }

    }
}
