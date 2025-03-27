using System.Collections.Generic;

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
            Weapon currentWeapon,
            int playerReputation,
            bool isFaithSpell,
            bool isHexSpell,
            bool shouldDoubleDamage)
        {
            float multiplier = shouldDoubleDamage ? 2 : 1f;

            if (this.fire > 0)
            {
                this.fire += (int)(currentWeapon.GetWeaponFireAttack(attackStatManager) + attackStatManager.GetIntelligenceBonusFromWeapon(currentWeapon) * multiplier);
            }

            if (this.frost > 0)
            {
                this.frost += (int)(currentWeapon.GetWeaponFrostAttack(attackStatManager) + attackStatManager.GetIntelligenceBonusFromWeapon(currentWeapon) * multiplier);
            }

            if (this.magic > 0)
            {
                this.magic += (int)(currentWeapon.GetWeaponMagicAttack(attackStatManager) + attackStatManager.GetIntelligenceBonusFromWeapon(currentWeapon) * multiplier);
            }

            if (this.lightning > 0)
            {
                this.lightning += (int)(
                    currentWeapon.GetWeaponLightningAttack(playerReputation, attackStatManager) + attackStatManager.GetIntelligenceBonusFromWeapon(currentWeapon) * multiplier);
            }

            if (this.darkness > 0)
            {
                this.darkness += (int)(currentWeapon.GetWeaponDarknessAttack(playerReputation, attackStatManager)
                    + attackStatManager.GetIntelligenceBonusFromWeapon(currentWeapon) * multiplier);
            }

            if (this.water > 0)
            {
                this.water += (int)(currentWeapon.GetWeaponWaterAttack(attackStatManager) + attackStatManager.GetIntelligenceBonusFromWeapon(currentWeapon) * multiplier);
            }

            if (this.pushForce > 0 && isFaithSpell)
            {
                this.pushForce += playerReputation > 0 ? (playerReputation * 0.1f) : 0;
            }

            Damage weaponDamage = currentWeapon.GetWeaponDamage();


            if (weaponDamage.statusEffects != null && weaponDamage.statusEffects.Length > 0)
            {
                List<StatusEffectEntry> updatedStatusEffects = new List<StatusEffectEntry>();

                // First, copy all existing status effects
                foreach (StatusEffectEntry existingEffect in this.statusEffects)
                {
                    updatedStatusEffects.Add(new StatusEffectEntry() { amountPerHit = existingEffect.amountPerHit, statusEffect = existingEffect.statusEffect });
                }

                // Then, combine with weapon status effects
                foreach (StatusEffectEntry weaponStatusEffectEntry in weaponDamage.statusEffects)
                {
                    StatusEffectEntry existingEffect = updatedStatusEffects.Find(x => x.statusEffect == weaponStatusEffectEntry.statusEffect);

                    if (existingEffect != null)
                    {
                        // Create a new entry with combined amount
                        int index = updatedStatusEffects.IndexOf(existingEffect);
                        updatedStatusEffects[index] = new StatusEffectEntry()
                        {
                            statusEffect = existingEffect.statusEffect,
                            amountPerHit = existingEffect.amountPerHit + weaponStatusEffectEntry.amountPerHit
                        };
                    }
                    else
                    {
                        // Add a new copy of the weapon status effect
                        updatedStatusEffects.Add(new StatusEffectEntry() { amountPerHit = weaponStatusEffectEntry.amountPerHit, statusEffect = weaponStatusEffectEntry.statusEffect });
                    }
                }

                this.statusEffects = updatedStatusEffects.ToArray();
            }
        }

        public void ScaleProjectile(AttackStatManager attackStatManager, Weapon currentWeapon)
        {
            // Steel arrow might inherit magic from a magical bow, hence don't check if base values are greater than zero
            this.physical += (int)currentWeapon.GetWeaponAttack(attackStatManager);

            if (attackStatManager.playerManager.statsBonusController.projectileMultiplierBonus > 0f)
            {
                this.physical = (int)(this.physical * attackStatManager.playerManager.statsBonusController.projectileMultiplierBonus);
            }

            this.fire += (int)currentWeapon.GetWeaponFireAttack(attackStatManager);
            this.frost += (int)currentWeapon.GetWeaponFrostAttack(attackStatManager);
            this.magic += (int)currentWeapon.GetWeaponMagicAttack(attackStatManager);
            this.lightning += (int)currentWeapon.GetWeaponLightningAttack(attackStatManager.playerStatsDatabase.GetCurrentReputation(), attackStatManager);
            this.darkness += (int)currentWeapon.GetWeaponDarknessAttack(attackStatManager.playerStatsDatabase.GetCurrentReputation(), attackStatManager);
            this.water += (int)currentWeapon.GetWeaponWaterAttack(attackStatManager);
        }


        public Damage Clone()
        {
            return (Damage)this.MemberwiseClone();
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
