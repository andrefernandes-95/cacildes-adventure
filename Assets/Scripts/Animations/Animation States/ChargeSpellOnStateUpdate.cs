using UnityEngine;

namespace AF
{
    public class ChargeSpellOnStateUpdate : StateMachineBehaviour
    {
        CharacterBaseManager character;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                animator.TryGetComponent(out character);
            }

            // Reset state
            character.characterBaseMagicManager.currentChargingSpellDamageMultiplier = 0f;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            character.characterBaseMagicManager.currentChargingSpellDamageMultiplier = Mathf.Clamp(stateInfo.normalizedTime, 0f, 1f);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            character.characterBaseMagicManager.currentChargingSpellDamageMultiplier = 1f;
        }
    }
}
