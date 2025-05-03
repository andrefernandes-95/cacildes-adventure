using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Blacksmith Anvil State", menuName = "States / Common / New Blacksmith Anvil State")]
    public class BlacksmithAnvilState : AIState
    {
        protected bool hasEnteredState = false;

        public override AIState Tick(CharacterManager characterManager)
        {
            return RunState(characterManager, characterManager.characterStateMachine.idleState);
        }

        public override AIState Tick(PlayerManager playerManager)
        {
            return RunState(playerManager, playerManager.playerStateMachine.playerIdleState);
        }

        AIState RunState(CharacterBaseManager characterBaseManager, BaseIdleState idleState)
        {
            if (!hasEnteredState)
            {
                hasEnteredState = true;
            }

            if (characterBaseManager.characterBaseBlacksmithManager.isWorking)
            {
                return this;
            }
            else
            {
                return SwitchState(characterBaseManager, idleState);
            }
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            hasEnteredState = false;
        }

    }
}
