using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Player Jump State", menuName = "States / Player / New Jump State")]
    public class PlayerJumpState : BaseJumpState
    {
        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 5f;

        public override AIState Tick(PlayerManager playerManager)
        {
            PlayJump(playerManager.animator);

            // Free Falling?
            if (playerManager.characterGravity.VerticalVelocity >= 0)
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.playerFallState);
            }

            playerManager.Move(moveSpeed, playerManager.playerCamera.GetPlayerRotation());

            return this;
        }
    }
}
