using System.Collections.Generic;
using System.Linq;

namespace AF.Health
{

    [System.Serializable]
    public class StatusEffectEntry
    {
        public StatusEffect statusEffect;
        public float amountPerHit;
    }

    [System.Serializable]
    public enum DamageType
    {
        NORMAL,
        COUNTER_ATTACK,
        ENRAGED,
    }

    [System.Serializable]
    public class Damage
    {
        public int physical;
        public int fire;
        public int frost;
        public int magic;
        public int lightning;
        public int darkness;
        public int water;
        public int postureDamage;
        public int poiseDamage;
        public float pushForce = 0;

        public WeaponAttackType weaponAttackType;

        public StatusEffectEntry[] statusEffects;

        public bool ignoreBlocking = false;
        public bool canNotBeParried = false;
        public DamageType damageType = DamageType.NORMAL;

        public Damage()
        {
        }

        public Damage(
            int physical,
            int fire,
            int frost,
            int magic,
            int lightning,
            int darkness,
            int water,
            int postureDamage,
            int poiseDamage,
            WeaponAttackType weaponAttackType,
            StatusEffectEntry[] statusEffects,
            float pushForce,
            bool ignoreBlocking,
            bool canNotBeParried)
        {
            this.physical = physical;
            this.fire = fire;
            this.frost = frost;
            this.magic = magic;
            this.lightning = lightning;
            this.darkness = darkness;
            this.water = water;
            this.postureDamage = postureDamage;
            this.poiseDamage = poiseDamage;
            this.weaponAttackType = weaponAttackType;
            this.statusEffects = statusEffects;
            this.pushForce = pushForce;
            this.ignoreBlocking = ignoreBlocking;
            this.canNotBeParried = canNotBeParried;
        }

        public int GetTotalDamage()
        {
            return physical + fire + frost + magic + lightning + darkness + water;
        }

        public void ScaleDamage(float multiplier)
        {
            this.physical = (int)(this.physical * multiplier);
            this.fire = (int)(this.fire * multiplier);
            this.frost = (int)(this.frost * multiplier);
            this.magic = (int)(this.magic * multiplier);
            this.lightning = (int)(this.lightning * multiplier);
            this.darkness = (int)(this.darkness * multiplier);
            this.water = (int)(this.water * multiplier);
        }

        public void ScaleSpell(
            AttackStatManager attackStatManager,
            WeaponInstance currentWeaponInstance,
            int playerReputation,
            bool isFaithSpell,
            bool isHexSpell,
            bool shouldDoubleDamage)
        {
            float multiplier = shouldDoubleDamage ? 2 : 1f;

            Weapon currentWeapon = currentWeaponInstance.GetItem<Weapon>();

            Damage currentWeaponDamage = currentWeapon.GetCurrentDamage(attackStatManager.character, currentWeaponInstance.level);

            if (currentWeapon.IsStaffWeapon())
            {
                this.Combine(currentWeaponDamage);
            }
        }

        public void ScaleProjectile(AttackStatManager attackStatManager, WeaponInstance currentWeaponInstance)
        {
            Weapon currentWeapon = currentWeaponInstance.GetItem<Weapon>();
            Damage currentWeaponDamage = currentWeapon.GetCurrentDamage(attackStatManager.character, currentWeaponInstance.level);

            if (currentWeapon.IsRangeWeapon())
            {
                this.Combine(currentWeaponDamage);
            }
        }

        public Damage Clone()
        {
            return (Damage)this.MemberwiseClone();
        }

        public Damage Combine(Damage damageToCombine)
        {
            List<StatusEffectEntry> statusEffectsCombined = this.statusEffects.ToList();

            foreach (StatusEffectEntry s in damageToCombine.statusEffects)
            {
                int idx = statusEffectsCombined.FindIndex(x => x == s);
                if (idx != -1)
                {
                    statusEffectsCombined[idx].amountPerHit += s.amountPerHit;
                }
                else
                {
                    statusEffectsCombined.Add(s);
                }
            }


            Damage newDamage = new()
            {
                physical = this.physical + damageToCombine.physical,
                fire = this.fire + damageToCombine.fire,
                frost = this.frost + damageToCombine.frost,
                lightning = this.lightning + damageToCombine.lightning,
                magic = this.magic + damageToCombine.magic,
                darkness = this.darkness + damageToCombine.darkness,
                water = this.water + damageToCombine.water,
                canNotBeParried = damageToCombine.canNotBeParried,
                ignoreBlocking = damageToCombine.ignoreBlocking,
                poiseDamage = this.poiseDamage + damageToCombine.poiseDamage,
                postureDamage = this.postureDamage + damageToCombine.postureDamage,
                pushForce = this.pushForce + damageToCombine.pushForce,
                //weaponAttackType = damageToCombine.weaponAttackType,
                statusEffects = statusEffectsCombined.ToArray(),
            };

            return newDamage;
        }
        public void ScaleDamageForNewGamePlus(GameSession gameSession)
        {
            this.physical = Utils.ScaleWithCurrentNewGameIteration(this.physical, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
            this.fire = Utils.ScaleWithCurrentNewGameIteration(this.fire, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
            this.frost = Utils.ScaleWithCurrentNewGameIteration(this.frost, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
            this.lightning = Utils.ScaleWithCurrentNewGameIteration(this.lightning, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
            this.magic = Utils.ScaleWithCurrentNewGameIteration(this.magic, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
            this.darkness = Utils.ScaleWithCurrentNewGameIteration(this.darkness, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
            this.water = Utils.ScaleWithCurrentNewGameIteration(this.water, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
            this.poiseDamage = Utils.ScaleWithCurrentNewGameIteration(this.poiseDamage, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
            this.postureDamage = Utils.ScaleWithCurrentNewGameIteration(this.postureDamage, gameSession.currentGameIteration, gameSession.newGamePlusScalingFactor);
        }

        public Damage Copy()
        {
            Damage newDamage = new()
            {
                physical = this.physical,
                fire = this.fire,
                frost = this.frost,
                lightning = this.lightning,
                magic = this.magic,
                darkness = this.darkness,
                water = this.water,
                canNotBeParried = this.canNotBeParried,
                damageType = this.damageType,
                ignoreBlocking = this.ignoreBlocking,
                poiseDamage = this.poiseDamage,
                postureDamage = this.postureDamage,
                pushForce = this.pushForce,
                weaponAttackType = this.weaponAttackType,
                statusEffects = this.statusEffects
            };

            return newDamage;
        }
    }
}
