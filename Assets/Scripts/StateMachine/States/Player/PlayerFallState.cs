using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Player Fall State", menuName = "States / Player / New Fall State")]
    public class PlayerFallState : BaseFallState
    {
        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 5f;

        bool hasUpdatedFallBegin = false;

        public override AIState Tick(PlayerManager playerManager)
        {
            SetInAirTimer(playerManager.animator, 0f);
            PlayFall(playerManager.animator);

            if (!hasUpdatedFallBegin)
            {
                hasUpdatedFallBegin = true;
                playerManager.characterGravity.UpdateFallBegin();
            }

            // Already grounded?
            if (playerManager.characterGravity.isGrounded)
            {
                return SwitchState(playerManager, playerManager.playerStateMachine.playerIdleState);
            }

            playerManager.Move(moveSpeed, playerManager.playerCamera.GetPlayerRotation());

            return this;
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            base.ResetStateFlags(characterBaseManager);

            if (characterBaseManager.characterGravity.GetFallHeight() > 1)
            {
                SetInAirTimer(characterBaseManager.animator, 1f);
            }

            hasUpdatedFallBegin = false;
        }

    }
}
