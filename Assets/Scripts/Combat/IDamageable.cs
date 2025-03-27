using System;

namespace AF.Combat
{
    public interface IDamageable
    {
        void OnDamage(CharacterBaseManager attacker, Action onDamageInflicted);
    }
}
