namespace AF.StateMachine
{
    using UnityEngine;


    [CreateAssetMenu(fileName = "Player Run State", menuName = "States / Player / New Run State")]
    public class PlayerRunState : AIState
    {
        [Header("Run Settings")]
        [SerializeField] float runSpeed = 5f;

        public override AIState Tick(PlayerManager playerManager)
        {
            playerManager.SetAnimatorFloat(AnimatorParametersConstants.Horizontal, 0);
            playerManager.SetAnimatorFloat(AnimatorParametersConstants.Vertical, 1);

            if (!playerManager.starterAssetsInputs.IsMoving())
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.idleState);
            }

            if (playerManager.IsJumping())
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.playerJumpState);
            }

            if (playerManager.IsFalling())
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.playerFallState);
            }

            playerManager.Move(runSpeed, playerManager.playerCamera.GetPlayerRotation());

            return this;
        }
    }
}
