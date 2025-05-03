namespace AF.StateMachine
{
    using UnityEngine;

    public class CharacterStateMachine : CharacterBaseStateMachine
    {
        [SerializeField] CharacterManager characterManager;

        [Header("Debug")]
        [SerializeField] protected AIState currentState;

        [Header("Default")]
        [SerializeField] AIState defaultState;

        [Header("States")]
        public AIIdleState idleState;
        public AIPursueTargetState pursueTargetState;
        public AITakeDamageState takeDamageState;
        public AIJumpState jumpState;
        public AIFallState fallState;

        private void Awake()
        {
            // Always instantiate new copies of scriptable objects, especially the AI States which have flags within them
            currentState = Instantiate(defaultState);
            idleState = Instantiate(idleState);
            pursueTargetState = Instantiate(pursueTargetState);
            takeDamageState = Instantiate(takeDamageState);
            jumpState = Instantiate(jumpState);
            fallState = Instantiate(fallState);
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

        public override void ChangeToTakeDamageState()
        {
            if (currentState != null)
            {
                currentState = currentState.SwitchState(this.characterManager, takeDamageState);
            }
            else
            {
                currentState = takeDamageState;
            }
        }

        public override void ChangeToState(AIState newState)
        {
            currentState = currentState.SwitchState(this.characterManager, newState);
        }
    }
}
