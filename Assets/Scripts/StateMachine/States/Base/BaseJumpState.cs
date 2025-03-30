using UnityEngine;

namespace AF.StateMachine
{
    public class BaseJumpState : AIState
    {
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
