using UnityEngine;

namespace AF
{
    public class EnableCanMoveOnStateExit : StateMachineBehaviour
    {
        CharacterBaseManager character;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                animator.TryGetComponent(out character);
            }

            character.EnableCanMove();
        }

    }
}
