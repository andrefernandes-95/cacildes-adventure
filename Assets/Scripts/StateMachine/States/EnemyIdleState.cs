using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "AI Idle State", menuName = "States / AI / New Idle State")]
    public class EnemyIdleState : BaseIdleState
    {
        public override AIState Tick(CharacterManager characterManager)
        {
            if (characterManager.targetManager.currentTarget != null)
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.pursueTargetState);
            }

            // Continue to search for target
            //aICharacterManager.aICharacterCombatManager.FindATargetViaLineOfSight(aICharacterManager);
            return this;
        }
    }
}
