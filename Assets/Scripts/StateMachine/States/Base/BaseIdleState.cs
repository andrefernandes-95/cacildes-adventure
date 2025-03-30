
using UnityEngine;

namespace AF.StateMachine
{
    public class BaseIdleState : AIState
    {
        protected void PlayIdle(Animator animator)
        {
            animator.SetFloat(AnimatorParametersConstants.Horizontal, 0, 0.2f, Time.deltaTime);
            animator.SetFloat(AnimatorParametersConstants.Vertical, 0, 0.2f, Time.deltaTime);
        }

        public override AIState Tick(CharacterManager characterManager)
        {
            PlayIdle(characterManager.animator);
            return this;
        }
    }
}
