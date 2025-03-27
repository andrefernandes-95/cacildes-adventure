using AF.Health;
using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    public class PlayerDamageResistances : DamageResistances
    {
        public PlayerManager playerManager;

        public int damageReductionFactor = 2;

        bool shouldIgnoreIncomingDamage = false;

        public UnityEvent onShouldIgnoreIncomingDamageStarts;
        public UnityEvent onShouldIgnoreIncomingDamageEnds;
        public UnityEvent onDamageIgnored;

        public override Damage FilterIncomingDamage(Damage incomingDamage)
        {
            if (shouldIgnoreIncomingDamage)
            {
                SetIgnoreIncomingDamage(false);
                onDamageIgnored?.Invoke();

                return GetIgnoredDamage();
            }

            Damage filteredDamage = base.FilterIncomingDamage(incomingDamage);

            float physicalDefense = playerManager.defenseStatManager.GetDefenseAbsorption();

            // Improved formula with adjustment for high defense
            float damageReductionPercentage = 1 - Mathf.Pow(1f / (1f + physicalDefense / 100f), damageReductionFactor);

            filteredDamage.physical = (int)(filteredDamage.physical * (1f - damageReductionPercentage));

            if (playerManager.defenseStatManager.physicalDefenseAbsorption > 0)
            {
                filteredDamage.physical -= (int)(filteredDamage.physical * playerManager.defenseStatManager.physicalDefenseAbsorption / 100);
            }

            return filteredDamage;
        }

        Damage GetIgnoredDamage()
        {
            return new(
                physical: 0,
                magic: 0,
                fire: 0,
                frost: 0,
                lightning: 0,
                darkness: 0,
                water: 0,
                postureDamage: 0,
                poiseDamage: 0,
                weaponAttackType: WeaponAttackType.Pierce,
                statusEffects: null,
                pushForce: 0,
                canNotBeParried: false,
                ignoreBlocking: false);
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="value"></param>
        public void SetIgnoreIncomingDamage(bool value)
        {
            this.shouldIgnoreIncomingDamage = value;

            if (value)
            {
                onShouldIgnoreIncomingDamageStarts?.Invoke();
            }
            else
            {
                onShouldIgnoreIncomingDamageEnds?.Invoke();
            }
        }


    }
}
