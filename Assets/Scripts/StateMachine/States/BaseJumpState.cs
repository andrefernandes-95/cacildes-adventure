using UnityEngine;

namespace AF.StateMachine
{
    public class BaseJumpState : AIState
    {
        public override AIState Tick(CharacterManager characterManager)
        {
            PlayJump(characterManager.animator);

            // Free Falling?
            if (characterManager.characterGravity.VerticalVelocity >= 0)
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.fallingState);
            }
            // Already grounded?
            else if (characterManager.characterGravity.isGrounded)
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.idleState);
            }

            return this;
        }

        protected void PlayJump(Animator animator)
        {
            animator.SetBool(AnimatorParametersConstants.Jump, true);
        }

        protected void StopJump(Animator animator)
        {
            animator.SetBool(AnimatorParametersConstants.Jump, false);
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            StopJump(characterBaseManager.animator);
        }
    }
}
