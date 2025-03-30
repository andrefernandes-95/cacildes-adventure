using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Player Roll State", menuName = "States / Player / New Roll State")]
    public class PlayerRollState : BaseRollState
    {
        public override AIState Tick(PlayerManager playerManager)
        {
            if (!hasRolled)
            {
                hasRolled = true;
                playerManager.characterRollManager.AttemptRoll();
            }

            if (playerManager.isBusy)
            {
                return this;
            }

            return SwitchState(playerManager, playerManager.playerStateMachine.playerIdleState);
        }
    }
}
