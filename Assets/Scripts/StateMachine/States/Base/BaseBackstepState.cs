using UnityEngine;

namespace AF.StateMachine
{
    public class BaseBackstepState : AIState
    {
        protected bool hasBackstepped = false;

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            hasBackstepped = false;
        }

    }
}
