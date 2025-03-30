using UnityEngine;

namespace AF.StateMachine
{
    public class BaseRollState : AIState
    {
        protected bool hasRolled = false;

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            hasRolled = false;
        }

    }
}
