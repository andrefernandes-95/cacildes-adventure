using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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

            if (characterManager.agent.velocity.magnitude <= 0)
            {
                Debug.Log("agent has no velocity for some reason");
                Vector3 direction = characterManager.targetManager.currentTarget.transform.position - characterManager.transform.position;
                direction.y = 0;
                characterManager.transform.rotation = Quaternion.Slerp(characterManager.transform.rotation, Quaternion.LookRotation(direction), characterManager.rotationSpeed * Time.deltaTime);
            }

            characterManager.SetAnimatorFloat(AnimatorParametersConstants.Vertical, characterManager.agent.velocity.magnitude > 0 ? 2f : 0f, 0.3f);

            return this;
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            base.ResetStateFlags(characterBaseManager);

            // Stop character
            (characterBaseManager as CharacterManager).DisableNavmeshAgent();
            characterBaseManager.SetAnimatorFloat(AnimatorParametersConstants.Vertical, 0f, 0.15f);

            hasEnteredState = false;
        }
    }
}
