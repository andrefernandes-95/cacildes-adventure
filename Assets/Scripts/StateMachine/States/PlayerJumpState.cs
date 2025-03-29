using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Player Jump State", menuName = "States / Player / New Jump State")]
    public class PlayerJumpState : BaseJumpState
    {
        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 3f;

        public override AIState Tick(PlayerManager playerManager)
        {
            if (playerManager.starterAssetsInputs.jump)
            {
                playerManager.starterAssetsInputs.jump = false;
            }

            PlayJump(playerManager.animator);

            // Free Falling?
            if (playerManager.characterGravity.VerticalVelocity >= 0)
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.fallingState);
            }

            playerManager.Move(moveSpeed, playerManager.playerCamera.GetPlayerRotation());

            return this;
        }
    }
}
