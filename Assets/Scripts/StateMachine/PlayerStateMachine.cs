namespace AF.StateMachine
{
    using UnityEngine;

    public class PlayerStateMachine : CharacterBaseStateMachine
    {
        [SerializeField] PlayerManager playerManager;

        public PlayerIdleState playerIdleState;
        public PlayerRunState playerRunState;
        public PlayerJumpState playerJumpState;
        public PlayerFallState playerFallState;
        public PlayerRollState playerRollState;
        public PlayerBackstepState playerBackstepState;
        public PlayerCombatState playerCombatState;
        public PlayerTakeDamageState playerTakeDamageState;

        [Header("Debug")]
        [SerializeField] protected AIState currentState;

        [Header("Default")]
        [SerializeField] AIState defaultState;

        private void Awake()
        {
            currentState = Instantiate(defaultState);
            playerIdleState = Instantiate(playerIdleState);
            playerRunState = Instantiate(playerRunState);
            playerJumpState = Instantiate(playerJumpState);
            playerFallState = Instantiate(playerFallState);
            playerRollState = Instantiate(playerRollState);
            playerBackstepState = Instantiate(playerBackstepState);
            playerCombatState = Instantiate(playerCombatState);
            playerTakeDamageState = Instantiate(playerTakeDamageState);

        }

        void Update()
        {
            ProcessStateMachine();
        }

        public void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(playerManager);
            if (nextState != null)
            {
                currentState = nextState;
            }
        }

        public override void ChangeToTakeDamageState()
        {
            currentState = playerTakeDamageState;
        }
    }
}
