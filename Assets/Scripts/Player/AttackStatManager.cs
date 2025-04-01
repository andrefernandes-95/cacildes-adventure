using UnityEngine;
using System.Linq;
using AF.Health;

namespace AF
{
    public class AttackStatManager : MonoBehaviour
    {

        [Header("Status attack bonus")]
        [Tooltip("Increased by buffs like potions, or equipment like accessories")]
        public float physicalAttackBonus = 0f;

        [Header("Unarmed Attack Options")]
        public int unarmedLightAttackPostureDamage = 18;
        public int unarmedPostureDamageBonus = 10;

        [Header("Physical Attack")]
        public int basePhysicalAttack = 100;

        public float jumpAttackMultiplier = 1.25f;
        public float twoHandAttackBonusMultiplier = 1.25f;
        public float heavyAttackBonusMultiplier = 1.35f;
        public float footDamageMultiplier = 1.2f;

        [Header("Buff Bonuses")]
        public ParticleSystem increaseNextAttackDamageFX;
        bool increaseNextAttackDamage = false;
        readonly float nextAttackMultiplierFactor = 1.3f;

        [Header("Databases")]
        public PlayerStatsDatabase playerStatsDatabase;
        public EquipmentDatabase equipmentDatabase;

        [Header("Components")]
        public CharacterBaseManager character;

        public enum AttackSource
        {
            WEAPON,
            SHIELD,
            UNARMED
        }

        public AttackSource attackSource = AttackSource.UNARMED;

        public void ResetStates() { }


        public bool HasBowEquipped()
        {
            return equipmentDatabase.IsBowEquipped();
        }

        public Damage GetAttackDamage(WeaponInstance currentWeaponInstance)
        {
            if (currentWeaponInstance != null && currentWeaponInstance.Exists())
            {
                Weapon weapon = currentWeaponInstance.GetItem<Weapon>();

                return weapon.GetCurrentDamage(character, currentWeaponInstance.level);
            }

            return CalculateUnarmedDamage();
        }

        Damage CalculateUnarmedDamage()
        {
            Damage unarmedDamage = new(
                physical: GetUnarmedPhysicalDamage(),
                fire: 0,
                frost: 0,
                magic: 0,
                lightning: 0,
                darkness: 0,
                water: 0,
                postureDamage: (character.combatManager.isHeavyAttacking || character.combatManager.isJumpAttacking)
                    ? unarmedLightAttackPostureDamage + unarmedPostureDamageBonus
                    : unarmedLightAttackPostureDamage,
                poiseDamage: 1,
                weaponAttackType: WeaponAttackType.Blunt,
                statusEffects: null,
                pushForce: 0,
                canNotBeParried: false,
                ignoreBlocking: false
            );

            return unarmedDamage;
        }


        int GetUnarmedPhysicalDamage()
        {
            int attackValue = GetCurrentPhysicalAttack();

            return attackValue;
        }

        int GetCurrentPhysicalAttack()
        {
            int heavyAttackBonus = 0;

            if (equipmentDatabase.GetCurrentRightWeapon() == null && character.combatManager.isHeavyAttacking)
            {
                heavyAttackBonus = character.combatManager.unarmedHeavyAttackBonus;
            }

            var value = basePhysicalAttack;

            return (int)Mathf.Round(
                Mathf.Ceil(
                    value
                        + (character.characterBaseStats.GetStrength() * Formulas.levelMultiplier)
                        + (character.characterBaseStats.GetDexterity() * Formulas.levelMultiplier)
                    ) + physicalAttackBonus + heavyAttackBonus
                );
        }


        public int GetCurrentPhysicalAttackForGivenStrengthAndDexterity(int strength, int dexterity)
        {
            return (int)Mathf.Round(
                Mathf.Ceil(
                    basePhysicalAttack
                        + (strength * Formulas.levelMultiplier)
                        + (dexterity * Formulas.levelMultiplier)
                    )
                );
        }


        public int CompareWeapon(WeaponInstance weaponInstanceToCompare)
        {
            if (equipmentDatabase.GetCurrentRightWeapon() == null || equipmentDatabase.GetCurrentRightWeapon().IsEmpty())
            {
                return 1;
            }

            var weaponToCompareAttack = weaponInstanceToCompare.GetItem<Weapon>().GetCurrentDamage(character, weaponInstanceToCompare.level)?.GetTotalDamage();

            // TODO: Change this to right and left
            var currentWeaponAttack = equipmentDatabase.GetCurrentRightWeapon().GetItem<Weapon>().GetCurrentDamage(character, equipmentDatabase.GetCurrentRightWeapon().level)?.GetTotalDamage();

            if (weaponToCompareAttack > currentWeaponAttack)
            {
                return 1;
            }

            if (weaponToCompareAttack == currentWeaponAttack)
            {
                return 0;
            }

            return -1;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="value"></param>
        public void SetBonusPhysicalAttack(int value)
        {
            physicalAttackBonus = value;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void ResetBonusPhysicalAttack()
        {
            physicalAttackBonus = 0f;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        /// <param name="value"></param>
        public void SetIncreaseNextAttackDamage(bool value)
        {
            increaseNextAttackDamage = value;
            SetBuffDamageFXLoop(value);
        }

        void SetBuffDamageFXLoop(bool isLooping)
        {
            var main = increaseNextAttackDamageFX.main;
            main.loop = isLooping;
        }

        Damage GetNextAttackBonusDamage(Damage damage)
        {
            if (increaseNextAttackDamage)
            {
                increaseNextAttackDamage = false;
                SetBuffDamageFXLoop(false);

                damage.physical = (int)(damage.physical * nextAttackMultiplierFactor);

                if (damage.fire > 0)
                {
                    damage.fire = (int)(damage.fire * nextAttackMultiplierFactor);
                }
                if (damage.frost > 0)
                {
                    damage.frost = (int)(damage.frost * nextAttackMultiplierFactor);
                }
                if (damage.lightning > 0)
                {
                    damage.lightning = (int)(damage.lightning * nextAttackMultiplierFactor);
                }
                if (damage.magic > 0)
                {
                    damage.magic = (int)(damage.magic * nextAttackMultiplierFactor);
                }
                if (damage.darkness > 0)
                {
                    damage.darkness = (int)(damage.darkness * nextAttackMultiplierFactor);
                }
                if (damage.water > 0)
                {
                    damage.water = (int)(damage.water * nextAttackMultiplierFactor);
                }
            }

            return damage;
        }

    }
}
