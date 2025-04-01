using System.Collections.Generic;
using AF.Stats;
using UnityEngine;

namespace AF
{

    public class DefenseStatManager : MonoBehaviour
    {
        [Header("Physical Defense")]
        public int basePhysicalDefense = 30;
        [Tooltip("Increases with endurance level")]
        public float levelMultiplier = 3.25f;

        [Header("Status defense bonus")]
        [Tooltip("Increased by buffs like potions, or equipment like accessories")]
        public float physicalDefenseBonus = 0f;
        [Range(0, 100f)] public float physicalDefenseAbsorption = 0f;

        [Header("Components")]
        public StatsBonusController playerStatsBonusController;

        [Header("Database")]
        public PlayerStatsDatabase playerStatsDatabase;
        public EquipmentDatabase equipmentDatabase;

        public bool ignoreDefense = false;

        public float GetDefenseAbsorption()
        {
            if (ignoreDefense)
            {
                return 0f;
            }

            return (int)(
                GetCurrentPhysicalDefense()
                + playerStatsBonusController.equipmentPhysicalDefense // Equipment Bonus
                + physicalDefenseBonus
                + (playerStatsBonusController.enduranceBonus * levelMultiplier)
            );
        }

        public int GetCurrentPhysicalDefense()
        {
            return (int)(this.basePhysicalDefense + playerStatsDatabase.endurance * levelMultiplier) / 2;
        }

        public int GetCurrentPhysicalDefenseForGivenEndurance(int endurance)
        {
            return (int)(this.basePhysicalDefense + ((endurance * levelMultiplier) / 2));
        }

        public float GetMaximumStatusResistanceBeforeSufferingStatusEffect(StatusEffect statusEffect)
        {
            return 1f;
        }

        public float GetMagicDefense()
        {
            return playerStatsBonusController.magicDefenseBonus;
        }

        public float GetDarknessDefense()
        {
            return playerStatsBonusController.darkDefenseBonus;
        }

        public float GetWaterDefense()
        {
            return playerStatsBonusController.waterDefenseBonus;
        }

        public float GetFireDefense()
        {
            return playerStatsBonusController.fireDefenseBonus;
        }

        public float GetFrostDefense()
        {
            return playerStatsBonusController.frostDefenseBonus;
        }

        public float GetLightningDefense()
        {
            return playerStatsBonusController.lightningDefenseBonus;
        }

        public int CompareHelmet(HelmetInstance helmet)
        {
            if (equipmentDatabase.helmet == null || !equipmentDatabase.helmet.Exists())
            {
                return 1;
            }

            if (helmet.GetItem<Helmet>().physicalDefense > equipmentDatabase.helmet.GetItem<Helmet>().physicalDefense)
            {
                return 1;
            }

            if (equipmentDatabase.helmet.GetItem<Helmet>().physicalDefense == helmet.GetItem<Helmet>().physicalDefense)
            {
                return 0;
            }

            return -1;
        }

        public int CompareArmor(ArmorInstance armor)
        {
            if (equipmentDatabase.armor == null || !equipmentDatabase.armor.Exists())
            {
                return 1;
            }

            if (armor.GetItem<Helmet>().physicalDefense > equipmentDatabase.armor.GetItem<Helmet>().physicalDefense)
            {
                return 1;
            }

            if (equipmentDatabase.armor.GetItem<Helmet>().physicalDefense == armor.GetItem<Helmet>().physicalDefense)
            {
                return 0;
            }

            return -1;
        }

        public int CompareGauntlet(GauntletInstance gauntlet)
        {
            if (equipmentDatabase.gauntlet == null || !equipmentDatabase.gauntlet.Exists())
            {
                return 1;
            }

            if (gauntlet.GetItem<Gauntlet>().physicalDefense > equipmentDatabase.gauntlet.GetItem<Gauntlet>().physicalDefense)
            {
                return 1;
            }

            if (equipmentDatabase.gauntlet.GetItem<Gauntlet>().physicalDefense == gauntlet.GetItem<Gauntlet>().physicalDefense)
            {
                return 0;
            }

            return -1;
        }

        public int CompareLegwear(LegwearInstance legwear)
        {
            if (equipmentDatabase.legwear == null || !legwear.Exists())
            {
                return 1;
            }

            if (legwear.GetItem<Legwear>().physicalDefense > equipmentDatabase.legwear.GetItem<Legwear>().physicalDefense)
            {
                return 1;
            }

            if (equipmentDatabase.legwear.GetItem<Legwear>().physicalDefense == legwear.GetItem<Legwear>().physicalDefense)
            {
                return 0;
            }

            return -1;
        }

        public void SetDefenseAbsorption(int value)
        {
            physicalDefenseAbsorption = value;
        }

        public void ResetDefenseAbsorption()
        {
            physicalDefenseAbsorption = 0f;
        }
    }
}
