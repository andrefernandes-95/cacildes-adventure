namespace AF
{
    using AF.StateMachine;
    using UnityEngine;

    public abstract class CharacterBaseStateMachine : MonoBehaviour
    {
        public abstract void ChangeToTakeDamageState();
        public abstract void ChangeToState(AIState newState);
    }
}