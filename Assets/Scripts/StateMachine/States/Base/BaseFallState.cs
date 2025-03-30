using UnityEngine;

namespace AF.StateMachine
{
    public class BaseFallState : AIState
    {
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
