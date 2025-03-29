namespace AF.StateMachine
{
    using UnityEngine;
    using NaughtyAttributes;

    public class CharacterStateMachine : MonoBehaviour
    {
        [ShowIf("IsAICharacter")]
        [SerializeField] CharacterManager characterManager;
        protected bool IsAICharacter() => this is not PlayerStateMachine;

        [Header("Debug")]
        [SerializeField] protected AIState currentState;

        [Header("Default")]
        [SerializeField] AIState defaultState;

        [Header("States")]
        [ShowIf("IsAICharacter")] public BaseIdleState idleState;
        [ShowIf("IsAICharacter")] public BaseJumpState jumpState;
        [ShowIf("IsAICharacter")] public BaseFallState fallingState;
        [ShowIf("IsAICharacter")] public AIState pursueTargetState;

        private void Awake()
        {
            currentState = defaultState;
        }

        void Update()
        {
            ProcessStateMachine();
        }

        public virtual void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(characterManager);
            if (nextState != null)
            {
                currentState = nextState;
            }

            // Reset navmesh agent local position and rotation because previously we tried to turn to the rotation of the navmesh agent, and it messes it up
            /*            navMeshAgent.transform.localPosition = Vector3.zero;
                        navMeshAgent.transform.localRotation = Quaternion.identity;*/

            /*
                        if (aICharacterCombatManager.currentTarget != null)
                        {
                            aICharacterCombatManager.targetDirection = aICharacterCombatManager.currentTarget.transform.position - transform.position;
                            aICharacterCombatManager.targetViewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, aICharacterCombatManager.targetDirection);
                            aICharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aICharacterCombatManager.currentTarget.transform.position);
                        }*/

            /*
                        if (navMeshAgent.enabled)
                        {
                            Vector3 agentDestination = navMeshAgent.destination;
                            float remainingDistance = Vector3.Distance(agentDestination, transform.position);
                            if (remainingDistance > navMeshAgent.stoppingDistance)
                            {
                                aICharacterNetworkManager.isMoving.Value = true;
                            }
                            else
                            {
                                aICharacterNetworkManager.isMoving.Value = false;
                            }
                        }
                        else
                        {
                            aICharacterNetworkManager.isMoving.Value = false;
                        }*/
        }
    }
}