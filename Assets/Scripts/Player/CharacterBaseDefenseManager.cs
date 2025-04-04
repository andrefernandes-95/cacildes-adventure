using System.Collections.Generic;
using AF.Health;
using AF.Stats;
using UnityEngine;

namespace AF
{

    public abstract class CharacterBaseDefenseManager : MonoBehaviour
    {
        [Header("Negated Damage")]
        public Damage damagedAbsorbed = new();

        [Header("Components")]
        public CharacterBaseManager character;

        // Call this whenever equipment is changed or stats are updated
        public void RecalculateDamageAbsorbed()
        {
            int vitality = character.characterBaseStats.GetVitality();
            int strength = character.characterBaseStats.GetStrength();
            int endurance = character.characterBaseStats.GetEndurance();

            damagedAbsorbed.physical = GetPhysicalDamageAbsorption(vitality, endurance, strength);
            damagedAbsorbed.magic = GetMagicDamageAbsorption();
            damagedAbsorbed.fire = GetFireDamageAbsorption();
            damagedAbsorbed.frost = GetFrostDamageAbsorption();
            damagedAbsorbed.water = GetWaterDamageAbsorption();
            damagedAbsorbed.darkness = GetDarknessDamageAbsorption();
            damagedAbsorbed.lightning = GetLightningDamageAbsorption();

            //TODO: Poise, Posture, Status Effects, etc.
        }

        // Why do we pass the stats here? Because we also call this function in Level Up screen 
        public int GetPhysicalDamageAbsorption(int vitality, int endurance, int strength)
        {
            int defense = 0;

            // Defense from stats
            defense += DefenseUtils.GetPhysicalDefenseFromEndurance(endurance);
            defense += DefenseUtils.GetPhysicalDefenseFromVitaly(vitality);
            defense += DefenseUtils.GetPhysicalDefenseFromStrength(strength);

            // Defense from equipment
            defense += character.statsBonusController.equipmentPhysicalDefenseBonus;

            return defense;
        }

        public int GetMagicDamageAbsorption()
        {
            return GetElementalDefense(character.statsBonusController.equipmentMagicDefenseBonus);
        }

        public int GetFireDamageAbsorption()
        {
            return GetElementalDefense(character.statsBonusController.equipmentMagicDefenseBonus);
        }

        public int GetFrostDamageAbsorption()
        {
            return GetElementalDefense(character.statsBonusController.equipmentMagicDefenseBonus);
        }

        public int GetWaterDamageAbsorption()
        {
            return GetElementalDefense(character.statsBonusController.equipmentMagicDefenseBonus);
        }

        int GetElementalDefense(int defenseFromEquipment)
        {
            int defense = 0;

            // Defense from stats
            defense += DefenseUtils.GetElementalDefenseFromIntelligence(character.characterBaseStats.GetIntelligence());

            defense += defenseFromEquipment;

            return defense;
        }


        int GetLightningDamageAbsorption()
        {
            int defense = 0;
            int defenseFromEquipment = character.statsBonusController.equipmentLightningDefenseBonus;

            // If the character has a reputation, they don't get a bonus to their defense from their stats, only from their equipment
            int reputation = character.characterBaseStats.GetReputation();
            if (reputation >= 0)
            {
                return defenseFromEquipment;
            }

            // If is evil character, makes sense to get lightning damage because of negative reputation
            reputation = Mathf.Abs(character.characterBaseStats.GetReputation());

            // Defense from stats
            defense += DefenseUtils.GetElementalDefenseFromReputation(reputation);
            defense += defenseFromEquipment;

            return defense;
        }

        int GetDarknessDamageAbsorption()
        {
            int defense = 0;
            int defenseFromEquipment = character.statsBonusController.equipmentDarkDefenseBonus;

            // If the character has negative reputation, they don't get a bonus to their defense from their stats, only from their equipment
            int reputation = character.characterBaseStats.GetReputation();
            if (reputation < 0)
            {
                return defenseFromEquipment;
            }

            // If is good / neutral character, makes sense to get darkness damage defense because of positive reputation
            reputation = character.characterBaseStats.GetReputation();

            // Defense from stats
            defense += DefenseUtils.GetElementalDefenseFromReputation(reputation);
            defense += defenseFromEquipment;

            return defense;
        }

        public int CompareHelmet(HelmetInstance helmetInstance)
        {
            int currentHelmetDamage = 0;
            if (character.characterBaseEquipment.GetHelmetInstance().Exists())
            {
                currentHelmetDamage = character.characterBaseEquipment.GetHelmetInstance().GetItem<Helmet>().damageAbsorbed.GetTotalDamage();
            }

            int newHelmetDamage = 0;
            if (helmetInstance != null && helmetInstance.Exists())
            {
                newHelmetDamage = helmetInstance.GetItem<Helmet>().damageAbsorbed.GetTotalDamage();
            }

            return CompareValues(currentHelmetDamage, newHelmetDamage);
        }

        public int CompareArmor(ArmorInstance armorInstance)
        {
            int currentDamage = 0;
            if (character.characterBaseEquipment.GetArmorInstance().Exists())
            {
                currentDamage = character.characterBaseEquipment.GetArmorInstance().GetItem<Armor>().damageAbsorbed.GetTotalDamage();
            }

            int itemDamage = 0;
            if (armorInstance != null && armorInstance.Exists())
            {
                itemDamage = armorInstance.GetItem<Armor>().damageAbsorbed.GetTotalDamage();
            }

            return CompareValues(currentDamage, itemDamage);
        }

        public int CompareGauntlets(GauntletInstance gauntletInstance)
        {
            int currentDamage = 0;
            if (character.characterBaseEquipment.GetGauntletInstance().Exists())
            {
                currentDamage = character.characterBaseEquipment.GetGauntletInstance().GetItem<Gauntlet>().damageAbsorbed.GetTotalDamage();
            }

            int itemDamage = 0;
            if (gauntletInstance != null && gauntletInstance.Exists())
            {
                itemDamage = gauntletInstance.GetItem<Gauntlet>().damageAbsorbed.GetTotalDamage();
            }

            return CompareValues(currentDamage, itemDamage);
        }


        public int CompareLegwears(LegwearInstance legwearInstance)
        {
            int currentDamage = 0;
            if (character.characterBaseEquipment.GetLegwearInstance().Exists())
            {
                currentDamage = character.characterBaseEquipment.GetLegwearInstance().GetItem<Legwear>().damageAbsorbed.GetTotalDamage();
            }

            int itemDamage = 0;
            if (legwearInstance != null && legwearInstance.Exists())
            {
                itemDamage = legwearInstance.GetItem<Legwear>().damageAbsorbed.GetTotalDamage();
            }

            return CompareValues(currentDamage, itemDamage);
        }

        int CompareValues(int current, int next)
        {

            (bool isBetter, bool isWorse, bool isEqual) = DefenseUtils.CompareDamageNegation(current, next);

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

    }
}
