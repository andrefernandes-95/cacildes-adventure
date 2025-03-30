using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Player Backstep State", menuName = "States / Player / New Backstep State")]
    public class PlayerBackstepState : BaseBackstepState
    {

        public override AIState Tick(PlayerManager playerManager)
        {
            if (!hasBackstepped)
            {
                hasBackstepped = true;
                playerManager.characterRollManager.AttemptBackstep();
            }

            if (playerManager.isBusy)
            {
                return this;
            }

            return SwitchState(playerManager, playerManager.playerStateMachine.playerIdleState);
        }

    }
}
