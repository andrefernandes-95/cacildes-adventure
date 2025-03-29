namespace AF.StateMachine
{
    using UnityEngine;

    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(PlayerManager playerManager)
        {
            return this;
        }

        public virtual AIState Tick(CharacterManager characterManager)
        {
            return this;
        }

        protected virtual AIState SwitchState(CharacterBaseManager characterBaseManager, AIState newState)
        {
            ResetStateFlags(characterBaseManager);

            return newState;
        }

        protected virtual void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {

        }
    }
}
