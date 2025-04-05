using UnityEngine;
using UnityEngine.AI;

namespace AF.StateMachine
{
    [CreateAssetMenu(fileName = "Pursue Target State", menuName = "States / AI / New Pursue Target State")]
    public class AIPursueTargetState : AIState
    {
        bool hasEnteredState = false;

        [Header("Jump Detection Settings")]
        public float jumpHeightThreshold = 1.5f;

        public override AIState Tick(CharacterManager characterManager)
        {
            if (characterManager.targetManager.currentTarget == null)
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.idleState);
            }

            Transform self = characterManager.transform;
            Transform target = characterManager.targetManager.currentTarget.transform;
            float distanceToTarget = Vector3.Distance(self.position, target.position);

            // If we're already close enough to the target, stop moving
            if (distanceToTarget <= characterManager.agent.stoppingDistance)
            {
                characterManager.SetAnimatorFloat(AnimatorParametersConstants.Vertical, 0f, 0.15f);
                return this;
            }

            // Try to jump if the target is above and close enough
            if (ShouldAttemptJump(self, target, characterManager))
            {
                return SwitchState(characterManager, characterManager.characterStateMachine.jumpState);
            }

            if (!hasEnteredState)
            {
                hasEnteredState = true;
                characterManager.EnableNavmeshAgent();
            }

            characterManager.agent.SetDestination(target.position);

            if (characterManager.agent.velocity.magnitude <= 0)
            {
                characterManager.RotateTowards(target);
            }

            float moveSpeed = characterManager.agent.velocity.magnitude > 0 ? 2f : 0f;
            characterManager.SetAnimatorFloat(AnimatorParametersConstants.Vertical, moveSpeed, 0.3f);

            return this;
        }

        private bool ShouldAttemptJump(Transform self, Transform target, CharacterManager characterManager)
        {
            float verticalOffset = target.position.y - self.position.y;

            // Check if path to target is invalid or blocked
            NavMeshPath path = new NavMeshPath();
            bool canReach = characterManager.agent.CalculatePath(target.position, path);
            bool pathBlocked = !canReach || path.status != NavMeshPathStatus.PathComplete;

            // Check for jumping up
            bool isTargetAbove = verticalOffset > jumpHeightThreshold;
            bool isTargetBelow = verticalOffset < -jumpHeightThreshold;

            bool shouldJumpUp = pathBlocked && isTargetAbove;
            bool shouldJumpDown = pathBlocked && isTargetBelow;

            return shouldJumpUp || shouldJumpDown;
        }

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            base.ResetStateFlags(characterBaseManager);

            (characterBaseManager as CharacterManager).DisableNavmeshAgent();
            characterBaseManager.SetAnimatorFloat(AnimatorParametersConstants.Vertical, 0f, 0.15f);

            hasEnteredState = false;
        }
    }
}
