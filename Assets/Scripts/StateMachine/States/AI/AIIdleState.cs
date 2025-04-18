using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "AI Idle State", menuName = "States / AI / New Idle State")]
    public class AIIdleState : BaseIdleState
    {

        bool hasEnteredState = false;

        public override AIState Tick(CharacterManager characterManager)
        {
            if (characterManager.targetManager.currentTarget != null)
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.pursueTargetState);
            }

            if (!hasEnteredState)
            {
                characterManager.DisableNavmeshAgent();
                hasEnteredState = true;
            }

            // Search for a target, AI is always eager to fight stuff
            characterManager.sight.FindATargetViaLineOfSight();

            base.PlayIdle(characterManager.animator);

            return this;
        }
        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            base.ResetStateFlags(characterBaseManager);
            hasEnteredState = false;
        }
    }
}
