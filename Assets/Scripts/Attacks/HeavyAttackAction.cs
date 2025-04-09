using UnityEngine;

namespace AF
{
    [CreateAssetMenu(fileName = "Heavy Attack Action", menuName = "Combat / New Heavy Attack Action", order = 0)]
    public class HeavyAttackAction : AttackAction
    {
        public override void Execute(CharacterBaseManager attacker)
        {
            base.Execute(attacker);

            attacker.animator.SetBool(AnimatorParametersConstants.IsCharging, true);
        }
    }
}
