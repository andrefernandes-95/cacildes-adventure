using System;
using System.Collections.Generic;
using System.Linq;
using AF.Animations;
using AF.Health;
using AF.Stats;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace AF
{
    public enum Scaling
    {
        S,
        A,
        B,
        C,
        D,
        E
    }

    public enum WeaponAttackType
    {
        Slash,
        Pierce,
        Blunt,
        Range,
        Staff,
    }

    public enum WeaponElementType
    {
        None,
        Fire,
        Frost,
        Lightning,
        Magic,
        Darkness,
        Water,
    }

    [System.Serializable]
    public class WeaponUpgradeLevel
    {
        public int goldCostForUpgrade;
        public Damage newDamage;

        public SerializedDictionary<UpgradeMaterial, int> upgradeMaterials;
    }

    [CreateAssetMenu(menuName = "Items / Action Item / New Weapon")]
    public class Weapon : ActionItem
    {

        [Header("Prefab")]
        public WorldWeapon worldWeapon;

        [Header("Attack")]
        public Damage damage;

        [Header("Level & Upgrades")]
        public bool canBeUpgraded = true;
        public int level = 1;
        public WeaponUpgradeLevel[] weaponUpgrades;

        //        [Tooltip("How much block hit this weapon does on an enemy shield. Heavier weapons should do at least 2 or 3 hits.")]
        //        public int blockHitAmount = 1;

        //        [Header("Block Absorption")]
        //        [Range(0, 100)] public int blockAbsorption = 75;
        //        public float blockStaminaCost = 30f;

        [Header("Requirements")]
        public int strengthRequired = 0;
        public int dexterityRequired = 0;
        public int intelligenceRequired = 0;
        public int positiveReputationRequired = 0;
        public int negativeReputationRequired = 0;


        [Header("Stamina")]
        public int lightAttackStaminaCost = 20;
        public int heavyAttackStaminaCost = 35;

        [Header("Scaling")]
        public Scaling strengthScaling = Scaling.E;
        public Scaling dexterityScaling = Scaling.E;
        public Scaling intelligenceScaling = Scaling.E;
        [Header("Weapon Special Options")]
        public int manaCostToUseWeaponSpecialAttack = 0;

        [Header("Use New Weapon System?")]
        [Tooltip("Temporary bool. If true, will use the new World Weapons system when equipped")]
        public bool useNewWeaponSystem = false;

        [Header("Animation Overrides")]
        public List<AnimationOverride> animationOverrides;
        [Tooltip("Optional")] public List<AnimationOverride> twoHandOverrides;
        [Tooltip("Optional")] public List<AnimationOverride> blockOverrides;

        public int lightAttackCombos = 2;
        public int heavyAttackCombos = 1;

        [Header("Upper Layer Options")]
        public bool useUpperLayerAnimations = false;
        public bool allowUpperLayerWhenOneHanding = true;
        public bool allowUpperLayerWhenTwoHanding = true;

        [Header("Dual Wielding Options")]
        public bool halveDamage = false;

        [Header("Speed Penalty")]
        [Tooltip("Will be added as a negative speed to the animator when equipped")]
        public float speedPenalty = 0f;
        [Range(0.1f, 2f)] public float oneHandAttackSpeedPenalty = 1f;
        [Range(0.1f, 2f)] public float twoHandAttackSpeedPenalty = 1f;

        [Header("Weapon Bonus")]
        public int amountOfGoldReceivedPerHit = 0;
        public bool doubleCoinsUponKillingEnemies = false;
        public bool doubleDamageDuringNightTime = false;
        public bool doubleDamageDuringDayTime = false;
        public int healthRestoredWithEachHit = 0;

        [Header("Jump Attack")]
        public float jumpAttackVelocity = -5f;

        [Header("Is Holy?")]
        public bool isHolyWeapon = false;
        public bool isHexWeapon = false;

        [Header("Range Category")]
        public bool isCrossbow = false;
        public bool isHuntingRifle = false;

        [Header("Block Options")]
        [Range(0, 1f)] public float blockAbsorption = .8f;

        [Header("Staff Options")]
        public bool shouldRegenerateMana = false;
        public bool ignoreSpellsAnimationClips = false;

#if UNITY_EDITOR
        private void OnEnable()
        {
            // No need to populate the list; it's serialized directly
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                // Clear the list when exiting play mode
                level = 1;
            }
        }
#endif

        public Damage GetDamageForLevel(int currentLevel)
        {
            WeaponUpgradeLevel weaponUpgradeLevel = weaponUpgrades.ElementAtOrDefault(currentLevel - 2);

            if (weaponUpgradeLevel != null)
            {
                return weaponUpgradeLevel.newDamage;
            }

            return this.damage;
        }

        public int GetStrengthBonusFromWeapon(CharacterBaseManager character)
        {
            if (damage.physical <= 0)
            {
                return 0;
            }

            return Formulas.GetBonusFromWeapon(
                character.characterBaseStats.GetStrength(),
                strengthScaling.ToString()
            );
        }

        public float GetDexterityBonusFromWeapon(CharacterBaseManager character)
        {
            if (damage.physical <= 0)
            {
                return 0;
            }

            return Formulas.GetBonusFromWeapon(
                character.characterBaseStats.GetDexterity(),
                dexterityScaling.ToString()
            );
        }

        public float GetIntelligenceBonusFromWeapon(CharacterBaseManager character)
        {
            return Formulas.GetBonusFromWeapon(
                character.characterBaseStats.GetIntelligence(),
                intelligenceScaling.ToString()
            );
        }


        public string GetFormattedStatusDamages()
        {
            string result = "";

            foreach (var statusEffect in damage.statusEffects)
            {
                if (statusEffect != null)
                {
                    result += $"+{statusEffect.amountPerHit} {statusEffect.statusEffect.GetName()} {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Inflicted per Hit")}\n";
                }
            }

            return result.TrimEnd();
        }

        public bool CanBeUpgradedFurther()
        {
            return canBeUpgraded && weaponUpgrades != null && weaponUpgrades.Length > 0 && this.level > 0 && this.level <= weaponUpgrades.Length - 1;
        }

        public string GetMaterialCostForNextLevel()
        {
            if (CanBeUpgradedFurther() && weaponUpgrades[this.level - 1] != null && weaponUpgrades[this.level - 1].upgradeMaterials != null)
            {
                WeaponUpgradeLevel nextWeaponUpgradeLevel = weaponUpgrades[this.level - 1];
                string text = $"{LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Next Weapon Level: ")}{this.level + 1}\n";

                text += $"{LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Required Gold:")} {nextWeaponUpgradeLevel.goldCostForUpgrade} {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Coins")}\n";
                text += $"{LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Required Items:")} \n";

                foreach (var upgradeMat in weaponUpgrades[this.level - 1].upgradeMaterials)
                {
                    if (upgradeMat.Key != null)
                    {
                        text += $"- {upgradeMat.Key.GetName()}: x{upgradeMat.Value}\n";
                    }
                }

                return text;
            }

            return "";
        }

        public bool HasRequirements()
        {
            return strengthRequired != 0 || dexterityRequired != 0 || intelligenceRequired != 0 || positiveReputationRequired != 0 || negativeReputationRequired != 0;
        }

        public bool AreRequirementsMet(CharacterBaseManager character)
        {
            if (character.statsBonusController.ignoreWeaponRequirements)
            {
                return true;
            }

            if (strengthRequired != 0 && character.characterBaseStats.GetStrength() < strengthRequired)
            {
                return false;
            }
            else if (dexterityRequired != 0 && character.characterBaseStats.GetDexterity() < dexterityRequired)
            {
                return false;
            }
            else if (intelligenceRequired != 0 && character.characterBaseStats.GetIntelligence() < intelligenceRequired)
            {
                return false;
            }
            else if (positiveReputationRequired != 0 && character.characterBaseStats.GetReputation() < positiveReputationRequired)
            {
                return false;
            }
            else if (negativeReputationRequired != 0 && character.characterBaseStats.GetReputation() > -negativeReputationRequired)
            {
                return false;
            }

            return true;
        }

        public string DrawRequirements(CharacterBaseManager character)
        {
            string text = AreRequirementsMet(character)
                ? LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Requirements met: ")
                : LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Requirements not met: ");

            if (strengthRequired != 0)
            {
                text += $"  {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Strength Required:")} {strengthRequired}   {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Current:")} {character.characterBaseStats.GetStrength()}\n";
            }
            if (dexterityRequired != 0)
            {
                text += $"  {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Dexterity Required:")} {dexterityRequired}   {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Current:")} {character.characterBaseStats.GetDexterity()}\n";
            }
            if (intelligenceRequired != 0)
            {
                text += $"  {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Intelligence Required:")} {intelligenceRequired}   {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Current:")} {character.characterBaseStats.GetIntelligence()}\n";
            }
            if (positiveReputationRequired != 0)
            {
                text += $"  {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Reputation Required:")} {intelligenceRequired}   {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Current:")} {character.characterBaseStats.GetReputation()}\n";
            }

            if (negativeReputationRequired != 0)
            {
                text += $"  {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Reputation Required:")} -{negativeReputationRequired}   {LocalizationSettings.StringDatabase.GetLocalizedString("UIDocuments", "Current:")} {character.characterBaseStats.GetReputation()}\n";
            }

            return text.TrimEnd();
        }



        public bool IsRangeWeapon()
        {
            return damage.weaponAttackType == WeaponAttackType.Range;
        }
        public bool IsStaffWeapon()
        {
            return damage.weaponAttackType == WeaponAttackType.Staff;
        }
    }

}
