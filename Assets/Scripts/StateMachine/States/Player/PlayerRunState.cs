namespace AF.StateMachine
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Player Run State", menuName = "States / Player / New Run State")]
    public class PlayerRunState : AIState
    {
        [Header("Run Settings")]
        [SerializeField] float runSpeed = 6f;
        [SerializeField] float sprintMultiplier = 1.25f;

        public override AIState Tick(PlayerManager playerManager)
        {
            playerManager.SetAnimatorFloat(AnimatorParametersConstants.Horizontal, 0);
            playerManager.SetAnimatorFloat(AnimatorParametersConstants.Vertical, playerManager.IsSprinting() ? 2 : 1);

            if (!playerManager.starterAssetsInputs.IsMoving())
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.playerIdleState);
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
                return SwitchState(playerManager, playerManager.playerStateMachine.playerRollState);
            }

            playerManager.Move(runSpeed * (playerManager.IsSprinting() ? sprintMultiplier : 1f), playerManager.playerCamera.GetPlayerRotation());

            return this;
        }
    }
}
