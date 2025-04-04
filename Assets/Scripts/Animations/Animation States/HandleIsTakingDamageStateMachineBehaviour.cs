
using UnityEngine;

namespace AF
{
    public class HandleIsTakingDamageStateMachineBehaviour : StateMachineBehaviour
    {
        CharacterBaseManager character;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                animator.TryGetComponent(out character);
            }

            character.isTakingDamage = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                animator.TryGetComponent(out character);
            }

            character.isTakingDamage = false;

            animator.applyRootMotion = false;
        }
    }
}
