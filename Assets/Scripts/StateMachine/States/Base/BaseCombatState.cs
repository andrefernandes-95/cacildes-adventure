using UnityEngine;

namespace AF.StateMachine
{
    public class BaseCombatState : AIState
    {
        protected bool hasChosenAttack = false;

        protected override void ResetStateFlags(CharacterBaseManager characterBaseManager)
        {
            hasChosenAttack = false;
        }

    }
}
