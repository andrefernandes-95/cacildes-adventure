using UnityEngine;
using System.Linq;
using AF.Health;
using AYellowpaper.SerializedCollections;

namespace AF
{
    public class CharacterBaseAttackManager : MonoBehaviour
    {
        [Header("Components")]
        public CharacterBaseManager character;

        [Header("Current Damage")]
        public Damage currentRightWeaponDamage;
        public Damage currentLeftWeaponDamage;

        [Header("Charging Attack Modifier")]
        public float currentChargingAttackMultiplier = 0f;

        public void ResetStates()
        {
            currentChargingAttackMultiplier = 0f;
        }

        // Call this every time equipment changes or stats are updated
        public void RecalculateDamages()
        {
            CalculateCurrentRightWeaponDamage();
            CalculateCurrentLeftWeaponDamage();
        }

        public Damage GetAttackingWeaponDamage()
        {
            Damage damage = new Damage();

            if (character.combatManager.currentAttackingMember == AttackingMember.RIGHT_HAND)
            {
                damage = currentRightWeaponDamage;
            }
            else if (character.combatManager.currentAttackingMember == AttackingMember.LEFT_HAND)
            {
                damage = currentLeftWeaponDamage;
            }

            if (currentChargingAttackMultiplier > 0)
            {
                damage = damage.WithScaledDamage(1 + currentChargingAttackMultiplier);
                Debug.Log("Charging attack, will multiply " + (1 + currentChargingAttackMultiplier) + " to the current damage");
            }

            Debug.Log("Damage dealt: " + damage.GetTotalDamage());

            return damage;
        }

        /// <summary>
        /// Checks for the source of the damage based on the character current equipment
        /// and then saves the damage based on scaling and bonus attribute modifiers
        /// 
        /// Any heavy attack, jump attack, two hand attack bonus should be handled in the TakeDamage effect
        /// </summary>
        public void CalculateCurrentRightWeaponDamage()
        {
            WeaponInstance rightWeaponInstance = character.characterBaseEquipment.GetRightHandWeapon();

            Damage initialDamage = new();

            if (rightWeaponInstance.Exists())
            {
                initialDamage = CalculateDamageOutput(rightWeaponInstance);
            }
            else if (character.characterWeapons.equippedRightWeaponInstance is UnarmedWorldWeapon unarmedWorldWeapon)
            {
                initialDamage = CalculateDamageOutput(unarmedWorldWeapon.unarmedDamageCollider.damage);
            }

            this.currentRightWeaponDamage = initialDamage;
        }

        /// <summary>
        /// Checks for the source of the damage based on the character current equipment
        /// and then saves the damage based on scaling and bonus attribute modifiers
        /// 
        /// Any heavy attack, jump attack, two hand attack bonus should be handled in the TakeDamage effect
        /// </summary>
        public void CalculateCurrentLeftWeaponDamage()
        {
            WeaponInstance leftWeaponInstance = character.characterBaseEquipment.GetLeftHandWeapon();

            Damage initialDamage = new();

            if (leftWeaponInstance.Exists())
            {
                initialDamage = CalculateDamageOutput(leftWeaponInstance);
            }
            else if (character.characterWeapons.equippedLeftWeaponInstance is UnarmedWorldWeapon unarmedWorldWeapon)
            {
                initialDamage = CalculateDamageOutput(unarmedWorldWeapon.unarmedDamageCollider.damage);
            }

            this.currentLeftWeaponDamage = initialDamage;
        }

        #region Damage Modifiers
        public Damage CalculateDamageOutput(Damage baseDamage)
        {
            Damage clonedDamage = baseDamage.Copy();

            // Scale damage
            ScaleDamage(clonedDamage);

            // Apply attributes
            ApplyModifiers(clonedDamage);

            return clonedDamage;
        }

        /// <summary>
        /// Similar to Calculate Damage Output, but takes into account the weapon current level
        /// </summary>
        /// <param name="weaponInstance"></param>
        /// <returns></returns>
        public Damage CalculateDamageOutput(WeaponInstance weaponInstance)
        {
            if (weaponInstance == null || weaponInstance.IsEmpty())
            {
                return null;
            }

            Weapon weapon = weaponInstance.GetItem<Weapon>();

            Damage clonedDamage = weapon.GetDamageForLevel(weaponInstance.level);

            // Scale damage
            ScaleDamage(clonedDamage);

            // Apply attributes
            ApplyModifiers(clonedDamage);

            return clonedDamage;
        }

        void ScaleDamage(Damage incomingDamage)
        {
            incomingDamage.physical += AttackUtils.GetScalingBonus(character.characterBaseStats.GetStrength(), incomingDamage.strengthScaling);
            incomingDamage.physical += AttackUtils.GetScalingBonus(character.characterBaseStats.GetDexterity(), incomingDamage.dexterityScaling);

            incomingDamage.fire += AttackUtils.GetScalingBonus(character.characterBaseStats.GetIntelligence(), incomingDamage.intelligenceScalling);
            incomingDamage.frost += AttackUtils.GetScalingBonus(character.characterBaseStats.GetIntelligence(), incomingDamage.intelligenceScalling);
            incomingDamage.magic += AttackUtils.GetScalingBonus(character.characterBaseStats.GetIntelligence(), incomingDamage.intelligenceScalling);
            incomingDamage.water += AttackUtils.GetScalingBonus(character.characterBaseStats.GetIntelligence(), incomingDamage.intelligenceScalling);

            int characterReputation = character.characterBaseStats.GetReputation();

            if (characterReputation >= 0)
            {
                incomingDamage.lightning += AttackUtils.GetScalingBonus(character.characterBaseStats.GetReputation(), incomingDamage.faithScaling);
            }
            else
            {
                incomingDamage.darkness += AttackUtils.GetScalingBonus(character.characterBaseStats.GetReputation(), incomingDamage.hexScaling);
            }
        }

        void ApplyModifiers(Damage damage)
        {
            float modifiers = 1f;

            if (character.combatManager.isTwoHanding)
            {
                modifiers += character.combatManager.twoHandingMultiplier + character.statsBonusController.twoHandAttackBonusMultiplier;
            }

            if (character.combatManager.isHeavyAttacking)
            {
                modifiers += character.combatManager.heavyAttackMultiplier + character.statsBonusController.heavyAttackBonusMultiplier;
            }

            if (character.combatManager.isJumpAttacking)
            {
                modifiers += character.combatManager.jumpAttackMultiplier + character.statsBonusController.jumpAttackBonusMultiplier;
            }

            // Bonus for guard counters and parry attacks
            if (character.characterBlockController.IsWithinCounterAttackWindow())
            {
                modifiers += character.characterBlockController.counterAttackMultiplier;
            }

            if (character.statsBonusController.increaseNextAttackDamage)
            {
                character.statsBonusController.increaseNextAttackDamage = false;

                modifiers += character.statsBonusController.nextAttackMultiplierFactor;
            }

            // If weapon is unarmed
            /* if (currentWeapon == null)
            {
                if (character.statsBonusController.increaseAttackPowerWhenUnarmed)
                {
                    attackMultiplierBonuses *= 1.65f;
                }
            }*/

            if (damage.weaponAttackType == WeaponAttackType.Pierce)
            {
                modifiers += character.statsBonusController.pierceDamageMultiplier;
            }
            else if (damage.weaponAttackType == WeaponAttackType.Blunt)
            {
                modifiers += character.statsBonusController.bluntDamageMultiplier;
            }
            else if (damage.weaponAttackType == WeaponAttackType.Slash)
            {
                modifiers += character.statsBonusController.slashDamageMultiplier;
            }
            // TODO: Add Range Multiplier

            ApplyModifier(damage, modifiers);

            if (character.statsBonusController.physicalAttackBonus > 0)
            {
                damage.physical = (int)(damage.physical + character.statsBonusController.physicalAttackBonus);
            }

            int extraAttackPower = 0;

            // + Attack the lower the reputation
            if (character.statsBonusController.increaseAttackPowerTheLowerTheReputation && character.characterBaseStats.GetReputation() < 0)
            {
                extraAttackPower += Mathf.Min(150, (int)(Mathf.Abs(character.characterBaseStats.GetReputation()) * 2.25f));
            }

            // + Attack when health is low
            if (character.statsBonusController.increaseAttackPowerWithLowerHealth)
            {
                extraAttackPower += (int)character.health.GetExtraAttackBasedOnCurrentHealth();
            }

            AddToDamage(damage, extraAttackPower);
        }

        void ApplyModifier(Damage damage, float modifier)
        {
            damage.physical = (int)(damage.physical * modifier);
            damage.fire = (int)(damage.fire * modifier);
            damage.frost = (int)(damage.frost * modifier);
            damage.magic = (int)(damage.magic * modifier);
            damage.lightning = (int)(damage.lightning * modifier);
            damage.darkness = (int)(damage.darkness * modifier);
            damage.water = (int)(damage.water * modifier);
        }

        void AddToDamage(Damage damage, int value)
        {
            if (damage.physical > 0)
            {
                damage.physical += value;
            }
            if (damage.fire > 0)
            {
                damage.fire += value;
            }
            if (damage.frost > 0)
            {
                damage.frost += value;
            }
            if (damage.magic > 0)
            {
                damage.magic += value;
            }
            if (damage.lightning > 0)
            {
                damage.lightning += value;
            }
            if (damage.darkness > 0)
            {
                damage.darkness += value;
            }
            if (damage.water > 0)
            {
                damage.water += value;
            }
        }

        #endregion

        #region Comparisons
        public int CompareRightWeapon(WeaponInstance weaponInstance)
        {
            int currentDamage = 0;
            if (character.characterBaseEquipment.GetRightHandWeapon().Exists())
            {
                currentDamage = character.characterBaseEquipment.GetRightHandWeapon().GetItem<Weapon>().damage.GetTotalDamage();
            }

            int itemDamage = 0;
            if (weaponInstance != null && weaponInstance.Exists())
            {
                itemDamage = weaponInstance.GetItem<Weapon>().damage.GetTotalDamage();
            }

            return CompareValues(currentDamage, itemDamage);
        }

        public int CompareLeftWeapon(WeaponInstance weaponInstance)
        {
            int currentDamage = 0;
            if (character.characterBaseEquipment.GetLeftHandWeapon().Exists())
            {
                currentDamage = character.characterBaseEquipment.GetLeftHandWeapon().GetItem<Weapon>().damage.GetTotalDamage();
            }

            int itemDamage = 0;
            if (weaponInstance != null && weaponInstance.Exists())
            {
                itemDamage = weaponInstance.GetItem<Weapon>().damage.GetTotalDamage();
            }

            return CompareValues(currentDamage, itemDamage);
        }

        int CompareValues(int current, int next)
        {
            (bool isBetter, bool isWorse, bool isEqual) = AttackUtils.CompareDamage(current, next);

            if (isBetter)
            {
                return 1;
            }
            else if (isWorse)
            {
                return -1;
            }

            // Is Equal
            return 0;
        }
        #endregion
    }
}
