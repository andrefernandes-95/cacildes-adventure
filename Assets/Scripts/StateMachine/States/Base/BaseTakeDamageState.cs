


using UnityEngine;

namespace AF.StateMachine
{
    public class BaseTakeDamageState : AIState
    {
        protected virtual void PlayDirectionalDamage(CharacterBaseManager characterManager)
        {
            if (characterManager.isBusy)
            {
                return;
            }

            // Play damage animation
            string directionalDamage = characterManager.damageReceiver.GetDirectionalDamagedAnimation();
            if (!string.IsNullOrEmpty(directionalDamage))
            {
                characterManager.PlayCrossFadeBusyAnimationWithRootMotion(directionalDamage, 0.2f);
            }
        }

        protected virtual void PlayBlockedDirectionalDamage(CharacterBaseManager characterManager)
        {
            if (characterManager.isBusy)
            {
                return;
            }

            // Play damage animation
            string directionalDamage = characterManager.characterBlockController.GetBlockedDirectionalDamagedAnimation();

            if (!string.IsNullOrEmpty(directionalDamage))
            {
                characterManager.PlayCrossFadeBusyAnimationWithRootMotion(directionalDamage, 0.2f);
            }
        }
    }
}
