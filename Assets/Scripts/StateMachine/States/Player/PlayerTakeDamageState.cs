using UnityEngine;

namespace AF.StateMachine
{

    [CreateAssetMenu(fileName = "Player Take Damage State", menuName = "States / Player / New Player Take Damage State")]
    public class PlayerTakeDamageState : BaseTakeDamageState
    {
        bool hasEnteredState = false;

        public override AIState Tick(PlayerManager playerManager)
        {
            if (playerManager.isTakingDamage)
            {
                return this;
            }

            if (!hasEnteredState)
            {
                hasEnteredState = true;

                PlayDirectionalDamage(playerManager);
                return this;
            }

            return playerManager.playerStateMachine.playerIdleState;
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            hasEnteredState = false;
        }

    }
}
