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

        public virtual AIState SwitchState(CharacterBaseManager characterBaseManager, AIState newState)
        {
            if (newState != this)
            {
                ResetStateFlags(characterBaseManager);
            }

            return newState;
        }

        protected virtual void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {

        }
    }
}
