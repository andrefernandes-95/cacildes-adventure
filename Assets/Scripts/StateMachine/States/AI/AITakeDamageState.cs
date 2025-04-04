using UnityEngine;

namespace AF.StateMachine
{

    [CreateAssetMenu(fileName = "AI Take Damage State", menuName = "States / AI / New Take Damage State")]
    public class AITakeDamageState : BaseTakeDamageState
    {
        bool hasEnteredState = false;

        public override AIState Tick(CharacterManager characterManager)
        {
            if (characterManager.isTakingDamage)
            {
                return this;
            }

            if (!hasEnteredState)
            {
                hasEnteredState = true;

                PlayDirectionalDamage(characterManager);
                return this;
            }

            return SwitchState(characterManager, characterManager.characterStateMachine.idleState);
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            hasEnteredState = false;
        }

    }
}
