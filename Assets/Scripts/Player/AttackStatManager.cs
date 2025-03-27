using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using AF.Health;

namespace AF
{
    public class AttackStatManager : MonoBehaviour
    {
        // RECOMMENDED ATTACK FORMULA:
        // STR LEVEL * levelMultiplier * weaponScaling
        // A weapon that has S scaling with a levelmultiplier of 3.25 produces:
        // 1 * 3.25 * 2.4 = 8
        // 4 * 3.25 * 2.4 = 31
        // 8 * 3.25 * 2.4 = 62
        // 16 * 3.25 * 2.4 = 125
        // This gives good values, similar to Dark Souls

        private Dictionary<string, float> scalingDictionary = new();

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
        public PlayerManager playerManager;

        public enum AttackSource
        {
            WEAPON,
            SHIELD,
            UNARMED
        }

        public AttackSource attackSource = AttackSource.UNARMED;

        private void Start()
        {
            scalingDictionary.Add("E", Formulas.E);
            scalingDictionary.Add("D", Formulas.D);
            scalingDictionary.Add("C", Formulas.C);
            scalingDictionary.Add("B", Formulas.B);
            scalingDictionary.Add("A", Formulas.A);
            scalingDictionary.Add("S", Formulas.S);
        }

        public void ResetStates() { }

        public bool IsHeavyAttacking()
        {
            return playerManager.playerCombatController.isHeavyAttacking;
        }

        public bool IsJumpAttacking()
        {
            return playerManager.playerCombatController.isJumpAttacking;
        }

        public bool HasBowEquipped()
        {
            return equipmentDatabase.IsBowEquipped();
        }

        public Damage GetAttackDamage()
        {
            Damage damage = CalculateCurrentDamage();

            damage = GetNextAttackBonusDamage(damage);

            return damage;
        }

        Damage CalculateCurrentDamage()
        {

            int rageBonus = playerManager.rageManager.GetRageBonus();

            Weapon weapon = equipmentDatabase.GetCurrentWeapon();
            if (weapon != null && playerManager.playerCombatController.isAttackingWithFoot == false)
            {
                Damage weaponDamage = new(
                    physical: GetWeaponAttack(weapon) + rageBonus,
                    fire: (int)weapon.GetWeaponFireAttack(playerManager.attackStatManager),
                    frost: (int)weapon.GetWeaponFrostAttack(playerManager.attackStatManager),
                    magic: (int)weapon.GetWeaponMagicAttack(playerManager.attackStatManager),
                    lightning: (int)weapon.GetWeaponLightningAttack(playerManager.playerStatsDatabase.GetCurrentReputation(), playerManager.attackStatManager),
                    darkness: (int)weapon.GetWeaponDarknessAttack(playerManager.playerStatsDatabase.GetCurrentReputation(), playerManager.attackStatManager),
                    water: (int)weapon.GetWeaponWaterAttack(playerManager.attackStatManager),
                    postureDamage: (IsHeavyAttacking() || IsJumpAttacking())
                    ? (int)(weapon.damage.postureDamage * 1.1f)
                    : weapon.damage.postureDamage,
                    poiseDamage: weapon.damage.poiseDamage,
                    weaponAttackType: weapon.damage.weaponAttackType,
                    statusEffects: weapon.damage.statusEffects,
                    pushForce: weapon.damage.pushForce,
                    canNotBeParried: weapon.damage.canNotBeParried,
                    ignoreBlocking: weapon.damage.ignoreBlocking
                );

                if (rageBonus > 0)
                {
                    weaponDamage.damageType = DamageType.ENRAGED;
                }

                return playerManager.playerWeaponsManager.GetBuffedDamage(weaponDamage);
            }


            Damage unarmedDamage = new(
                physical: GetUnarmedPhysicalDamage() + rageBonus,
                fire: 0,
                frost: 0,
                magic: 0,
                lightning: 0,
                darkness: 0,
                water: 0,
                postureDamage: (IsHeavyAttacking() || IsJumpAttacking())
                    ? unarmedLightAttackPostureDamage + unarmedPostureDamageBonus
                    : unarmedLightAttackPostureDamage,
                poiseDamage: 1,
                weaponAttackType: WeaponAttackType.Blunt,
                statusEffects: null,
                pushForce: 0,
                canNotBeParried: false,
                ignoreBlocking: false
            );

            if (rageBonus > 0)
            {
                unarmedDamage.damageType = DamageType.ENRAGED;
            }

            return unarmedDamage;
        }

        public int GetCurrentAttackForWeapon(Weapon weapon)
        {
            return weapon != null
                ? GetWeaponAttack(weapon)
                : GetCurrentPhysicalAttack();
        }

        int GetUnarmedPhysicalDamage()
        {
            int attackValue = GetCurrentPhysicalAttack();

            if (playerManager.thirdPersonController.Grounded == false || IsJumpAttacking())
            {
                attackValue = Mathf.FloorToInt(attackValue * jumpAttackMultiplier);

                var jumpAttackBonuses = equipmentDatabase.accessories.Sum(x => x != null ? x.jumpAttackBonus : 0);
                attackValue += jumpAttackBonuses;
            }

            return GetAttackBuffs(attackValue);
        }

        int GetCurrentPhysicalAttack()
        {
            int heavyAttackBonus = 0;

            if (equipmentDatabase.GetCurrentWeapon() == null && playerManager.playerCombatController.isHeavyAttacking)
            {
                heavyAttackBonus = playerManager.playerCombatController.unarmedHeavyAttackBonus;
            }

            var value = basePhysicalAttack;

            return (int)Mathf.Round(
                Mathf.Ceil(
                    value
                        + (playerStatsDatabase.strength * Formulas.levelMultiplier)
                        + (playerStatsDatabase.dexterity * Formulas.levelMultiplier)
                        + (playerManager.statsBonusController.strengthBonus * Formulas.levelMultiplier)
                        + (playerManager.statsBonusController.dexterityBonus * Formulas.levelMultiplier)
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


        public int CompareWeapon(Weapon weaponToCompare)
        {
            if (equipmentDatabase.GetCurrentWeapon() == null)
            {
                return 1;
            }

            var weaponToCompareAttack = GetWeaponAttack(weaponToCompare);
            var currentWeaponAttack = GetWeaponAttack(equipmentDatabase.GetCurrentWeapon());

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

        public int GetWeaponAttack(Weapon weapon)
        {
            var baseValue = HasBowEquipped() || weapon.damage.physical <= 0 ? 0 : GetCurrentPhysicalAttack();

            var value = (int)
                baseValue + weapon.damage.physical <= 0 ? 0 : (
                weapon.GetWeaponAttack(this)
            );

            if (equipmentDatabase.isTwoHanding)
            {
                value = (int)(value * twoHandAttackBonusMultiplier);
            }

            if (playerManager.playerCombatController.isHeavyAttacking)
            {
                value = (int)(value * heavyAttackBonusMultiplier);
            }

            if (playerManager.thirdPersonController.Grounded == false || IsJumpAttacking())
            {
                value = Mathf.FloorToInt(value * jumpAttackMultiplier);

                var jumpAttackBonuses = equipmentDatabase.accessories.Sum(x => x != null ? x.jumpAttackBonus : 0);
                value += jumpAttackBonuses;
            }

            if (weapon.halveDamage && equipmentDatabase.isTwoHanding)
            {
                return (int)(value / 2);
            }

            return GetAttackBuffs(value);
        }

        public int GetAttackBuffs(int value)
        {
            // + Attack the lower the rep
            if (equipmentDatabase.accessories.FirstOrDefault(x => x != null && x.increaseAttackPowerTheLowerTheReputation) != null)
            {
                if (playerStatsDatabase.GetCurrentReputation() < 0)
                {
                    int extraAttackPower = Mathf.Min(150, (int)(Mathf.Abs(playerStatsDatabase.GetCurrentReputation()) * 2.25f));

                    value += extraAttackPower;
                }
            }

            // + Attack the lower the health
            if (equipmentDatabase.accessories.FirstOrDefault(x => x != null && x.increaseAttackPowerWithLowerHealth) != null)
            {
                int extraAttackPower = (int)(value * (playerManager.health as PlayerHealth).GetExtraAttackBasedOnCurrentHealth());

                value += extraAttackPower;
            }

            // Generic attack bonuses
            var attackBonuses = equipmentDatabase.accessories.Sum(x => x != null ? x.physicalAttackBonus : 0);
            value += attackBonuses;

            // Bonus for guard counters and parry attacks
            if (playerManager.characterBlockController.IsWithinCounterAttackWindow())
            {
                value = (int)(value * playerManager.characterBlockController.counterAttackMultiplier);
            }

            float attackMultiplierBonuses = 1;

            // Bonus for two handing attack accessories
            if (equipmentDatabase.isTwoHanding)
            {
                attackMultiplierBonuses += playerManager.statsBonusController.twoHandAttackBonusMultiplier;
            }

            Weapon currentWeapon = equipmentDatabase.GetCurrentWeapon();

            if (currentWeapon == null)
            {
                if (playerManager.statsBonusController.increaseAttackPowerWhenUnarmed)
                {
                    attackMultiplierBonuses *= 1.65f;
                }
            }
            else
            {
                if (currentWeapon.damage.weaponAttackType == WeaponAttackType.Pierce)
                {
                    attackMultiplierBonuses += playerManager.statsBonusController.pierceDamageMultiplier;
                }
                else if (currentWeapon.damage.weaponAttackType == WeaponAttackType.Slash)
                {
                    attackMultiplierBonuses += playerManager.statsBonusController.slashDamageMultiplier;
                }
                else if (currentWeapon.damage.weaponAttackType == WeaponAttackType.Blunt)
                {
                    attackMultiplierBonuses += playerManager.statsBonusController.bluntDamageMultiplier;
                }
            }

            if (playerManager.playerCombatController.isAttackingWithFoot)
            {
                value = (int)(value * (footDamageMultiplier + playerManager.statsBonusController.footDamageMultiplier));
            }

            value = (int)(value * attackMultiplierBonuses);

            return value;
        }


        public int GetStrengthBonusFromWeapon(Weapon weapon)
        {
            if (weapon.damage.physical <= 0)
            {
                return 0;
            }

            return Formulas.GetBonusFromWeapon(
                playerManager.statsBonusController.GetCurrentStrength(),
                 scalingDictionary[weapon.strengthScaling.ToString()]
            );
        }

        public float GetDexterityBonusFromWeapon(Weapon weapon)
        {
            if (weapon.damage.physical <= 0)
            {
                return 0;
            }

            return Formulas.GetBonusFromWeapon(
                playerManager.statsBonusController.GetCurrentDexterity(),
                 scalingDictionary[weapon.dexterityScaling.ToString()]
            );
        }

        public float GetIntelligenceBonusFromWeapon(Weapon weapon)
        {
            return Formulas.GetBonusFromWeapon(
                playerManager.statsBonusController.GetCurrentIntelligence(),
                 scalingDictionary[weapon.intelligenceScaling.ToString()]
            );
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
