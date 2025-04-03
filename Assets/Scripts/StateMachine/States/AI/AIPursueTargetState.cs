using UnityEditor;
using UnityEngine;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Pursue Target State", menuName = "States / AI / New Pursue Target State")]
    public class AIPursueTargetState : AIState
    {
        bool hasEnteredState = false;

        public override AIState Tick(CharacterManager characterManager)
        {
            if (characterManager.targetManager.currentTarget == null)
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.idleState);
            }

            if (Vector3.Distance(characterManager.transform.position, characterManager.targetManager.currentTarget.transform.position)
                <= characterManager.agent.stoppingDistance)
            {
                characterManager.SetAnimatorFloat(AnimatorParametersConstants.Vertical, 0f, 0.15f);
                return this;
            }

            if (!hasEnteredState)
            {
                hasEnteredState = true;
                characterManager.EnableNavmeshAgent();
            }

            characterManager.agent.SetDestination(characterManager.targetManager.currentTarget.transform.position);
            characterManager.SetAnimatorFloat(AnimatorParametersConstants.Vertical, 2f, 0.3f);

            return this;
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            base.ResetStateFlags(characterBaseManager);

            hasEnteredState = false;
        }
    }
}
