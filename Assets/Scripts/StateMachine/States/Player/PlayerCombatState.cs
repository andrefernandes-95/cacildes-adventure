using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Player Combat State", menuName = "States / Player / New Combat State")]
    public class PlayerCombatState : BaseCombatState
    {
        void ResetPlayerInputs(PlayerManager playerManager)
        {
            playerManager.starterAssetsInputs.rightHandBumper = false;
            playerManager.starterAssetsInputs.leftHandAttack = false;
        }

        public void CheckForCombos(PlayerManager playerManager)
        {
            if (playerManager.IsAttemptingToRightAttack() || playerManager.IsAttemptingToLeftAttack())
            {
                hasChosenAttack = false;
            }
        }

        public override AIState Tick(PlayerManager playerManager)
        {
            if (!hasChosenAttack)
            {
                ResetPlayerInputs(playerManager);

                hasChosenAttack = true;
                playerManager.combatManager.AttemptAttack();
            }

            if (playerManager.isBusy)
            {
                return this;
            }

            return SwitchState(playerManager, playerManager.playerStateMachine.playerIdleState);
        }

    }
}
