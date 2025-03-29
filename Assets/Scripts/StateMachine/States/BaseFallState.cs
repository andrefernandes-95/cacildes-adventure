using UnityEngine;

namespace AF.StateMachine
{
    public class BaseFallState : AIState
    {
        public override AIState Tick(CharacterManager characterManager)
        {
            PlayFall(characterManager.animator);

            // Already grounded?
            if (characterManager.characterGravity.isGrounded)
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.idleState);
            }

            return this;
        }

        protected void SetInAirTimer(Animator animator, float time)
        {
            animator.SetFloat(AnimatorParametersConstants.InAirTimer, time);
        }

        protected void PlayFall(Animator animator)
        {
            animator.SetBool(AnimatorParametersConstants.FreeFall, true);
        }
        protected void StopFall(Animator animator)
        {
            animator.SetBool(AnimatorParametersConstants.FreeFall, false);
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            StopFall(characterBaseManager.animator);
        }

    }
}
