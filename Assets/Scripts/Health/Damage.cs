using System.Collections.Generic;
using System.Linq;

namespace AF.Health
{
    [System.Serializable]
    public class StatusEffectEntry
    {
        public StatusEffect statusEffect;
        public float amountPerHit;

        public StatusEffectEntry Clone()
        {
            return new StatusEffectEntry
            {
                statusEffect = this.statusEffect,
                amountPerHit = this.amountPerHit
            };
        }
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
        public float pushForce = 0f;

        public WeaponAttackType weaponAttackType;
        public StatusEffectEntry[] statusEffects;

        public Scaling strengthScaling = Scaling.E;
        public Scaling dexterityScaling = Scaling.E;
        public Scaling intelligenceScalling = Scaling.E;
        public Scaling faithScaling = Scaling.E;
        public Scaling hexScaling = Scaling.E;

        public bool ignoreBlocking = false;
        public bool canNotBeParried = false;

        // Default constructor
        public Damage() => statusEffects = new StatusEffectEntry[0];

        // Constructor with all fields
        public Damage(int physical, int fire, int frost, int magic, int lightning, int darkness, int water,
                      int postureDamage, int poiseDamage, WeaponAttackType weaponAttackType,
                      StatusEffectEntry[] statusEffects, float pushForce, bool ignoreBlocking, bool canNotBeParried)
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
            this.statusEffects = statusEffects ?? new StatusEffectEntry[0];
            this.pushForce = pushForce;
            this.ignoreBlocking = ignoreBlocking;
            this.canNotBeParried = canNotBeParried;
        }

        public int GetTotalDamage() =>
            physical + fire + frost + magic + lightning + darkness + water;

        public Damage WithScaledDamage(float multiplier)
        {
            Damage scaled = this.Copy();
            scaled.physical = (int)(scaled.physical * multiplier);
            scaled.fire = (int)(scaled.fire * multiplier);
            scaled.frost = (int)(scaled.frost * multiplier);
            scaled.magic = (int)(scaled.magic * multiplier);
            scaled.lightning = (int)(scaled.lightning * multiplier);
            scaled.darkness = (int)(scaled.darkness * multiplier);
            scaled.water = (int)(scaled.water * multiplier);
            return scaled;
        }

        public void ScaleDamageForNewGamePlus(GameSession gameSession)
        {
            var iter = gameSession.currentGameIteration;
            var scale = gameSession.newGamePlusScalingFactor;

            physical = Utils.ScaleWithCurrentNewGameIteration(physical, iter, scale);
            fire = Utils.ScaleWithCurrentNewGameIteration(fire, iter, scale);
            frost = Utils.ScaleWithCurrentNewGameIteration(frost, iter, scale);
            magic = Utils.ScaleWithCurrentNewGameIteration(magic, iter, scale);
            lightning = Utils.ScaleWithCurrentNewGameIteration(lightning, iter, scale);
            darkness = Utils.ScaleWithCurrentNewGameIteration(darkness, iter, scale);
            water = Utils.ScaleWithCurrentNewGameIteration(water, iter, scale);
            postureDamage = Utils.ScaleWithCurrentNewGameIteration(postureDamage, iter, scale);
            poiseDamage = Utils.ScaleWithCurrentNewGameIteration(poiseDamage, iter, scale);
        }

        public Damage Copy()
        {
            return new Damage
            {
                physical = this.physical,
                fire = this.fire,
                frost = this.frost,
                magic = this.magic,
                lightning = this.lightning,
                darkness = this.darkness,
                water = this.water,
                postureDamage = this.postureDamage,
                poiseDamage = this.poiseDamage,
                pushForce = this.pushForce,
                canNotBeParried = this.canNotBeParried,
                ignoreBlocking = this.ignoreBlocking,
                weaponAttackType = this.weaponAttackType,
                statusEffects = this.statusEffects?.Select(se => se.Clone()).ToArray()
            };
        }

        public Damage Combine(Damage other)
        {
            Damage combined = this.Copy();
            combined.CombineInPlace(other);
            return combined;
        }

        void CombineInPlace(Damage other)
        {
            physical += other.physical;
            fire += other.fire;
            frost += other.frost;
            magic += other.magic;
            lightning += other.lightning;
            darkness += other.darkness;
            water += other.water;
            postureDamage += other.postureDamage;
            poiseDamage += other.poiseDamage;
            pushForce += other.pushForce;

            if (other.ignoreBlocking) ignoreBlocking = true;
            if (other.canNotBeParried) canNotBeParried = true;

            // Merge status effects intelligently
            var combinedEffects = statusEffects.ToList();
            foreach (var effect in other.statusEffects)
            {
                var match = combinedEffects.FirstOrDefault(x => x.statusEffect == effect.statusEffect);
                if (match != null)
                {
                    match.amountPerHit += effect.amountPerHit;
                }
                else
                {
                    combinedEffects.Add(effect.Clone());
                }
            }

            statusEffects = combinedEffects.ToArray();
        }

        public Damage AddPhysicalBonus(int amount)
        {
            Damage copy = this.Copy();
            copy.physical += amount;
            return copy;
        }

        public override string ToString()
        {
            return $"Damage(Phys: {physical}, Fire: {fire}, Frost: {frost}, Magic: {magic}, Light: {lightning}, Dark: {darkness}, Water: {water})";
        }
    }
}
