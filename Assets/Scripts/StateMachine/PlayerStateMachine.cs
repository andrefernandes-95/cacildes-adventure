namespace AF.StateMachine
{
    using UnityEngine;

    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] PlayerManager playerManager;

        public PlayerIdleState playerIdleState;
        public PlayerRunState playerRunState;
        public PlayerJumpState playerJumpState;
        public PlayerFallState playerFallState;
        public PlayerRollState playerRollState;
        public PlayerBackstepState playerBackstepState;

        [Header("Debug")]
        [SerializeField] protected AIState currentState;

        [Header("Default")]
        [SerializeField] AIState defaultState;

        private void Awake()
        {
            currentState = defaultState;
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
    }
}
