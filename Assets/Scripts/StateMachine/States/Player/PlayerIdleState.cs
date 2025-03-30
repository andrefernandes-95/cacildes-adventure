using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Player Idle State", menuName = "States / Player / New Idle State")]
    public class PlayerIdleState : BaseIdleState
    {
        public override AIState Tick(PlayerManager playerManager)
        {
            PlayIdle(playerManager.animator);

            if (playerManager.starterAssetsInputs.IsMoving())
            {
                return playerManager.playerStateMachine.playerRunState;
            }

            if (playerManager.IsJumping())
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.playerJumpState);
            }

            if (playerManager.IsFalling())
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.playerFallState);
            }

            if (playerManager.IsAttemptingToDodge())
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.playerBackstepState);
            }

            return this;
        }
    }
}
