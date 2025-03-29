namespace AF.StateMachine
{
    using UnityEngine;

    public class PlayerStateMachine : CharacterStateMachine
    {
        public PlayerIdleState playerIdleState;
        public PlayerRunState playerRunState;
        public PlayerJumpState playerJumpState;
        public PlayerFallState playerFallState;

        [Header("Components")]
        [SerializeField] PlayerManager playerManager;

        public override void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(playerManager);
            if (nextState != null)
            {
                currentState = nextState;
            }
        }
    }
}
