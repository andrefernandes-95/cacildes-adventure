using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Player Combat State", menuName = "States / Player / New Combat State")]
    public class PlayerCombatState : BaseCombatState
    {
        void ResetPlayerInputs(PlayerManager playerManager)
        {
            playerManager.starterAssetsInputs.rightHandBumper = false;
            playerManager.starterAssetsInputs.leftHandBumper = false;
        }

        public override AIState Tick(PlayerManager playerManager)
        {
            if (playerManager.combatManager.allowCombos && (playerManager.IsAttemptingToRightAttack() || playerManager.IsAttemptingToLeftAttack()))
            {
                playerManager.combatManager.allowCombos = false;

                hasChosenAttack = false;
            }

            if (!hasChosenAttack)
            {
                ResetPlayerInputs(playerManager);

                hasChosenAttack = true;

                playerManager.combatManager.AttemptAttack();
            }

            if (playerManager.combatManager.isJumpAttacking)
            {
                // TODO: Change this to airJumpMovespeed
                playerManager.Move(3f, playerManager.playerCamera.GetPlayerRotation());
            }

            if (playerManager.isBusy)
            {
                return this;
            }

            return SwitchState(playerManager, playerManager.playerStateMachine.playerIdleState);
        }

    }
}
