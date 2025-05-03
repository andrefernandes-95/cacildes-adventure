using AF.Health;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AF.UI
{
    public class UI_UpgradeItemTooltip_WeaponDamage : UI_ItemTooltipBase
    {

        public bool isPhysicalDamage = false;
        public bool isFireDamage = false;
        public bool isFrostDamage = false;
        public bool isLightningDamage = false;
        public bool isMagicalDamage = false;
        public bool isWaterDamage = false;
        public bool isDarknessDamage = false;

        [Header("Status Effect")]
        public StatusEffect statusEffect;

        public void ShowTooltip(WeaponInstance weaponInstance)
        {
            if (weaponInstance == null || weaponInstance.IsEmpty()) return;

            label.text = GetLabel();

            if (statusEffect != null)
            {
                label.color = statusEffect.barColor;
                icon.sprite = weaponInstance.item.sprite;
            }

            int currentDamage = GetCurrentDamageForLevel(weaponInstance, weaponInstance.level);
            currentValue.text = currentDamage.ToString();

            int nextDamage = GetCurrentDamageForLevel(weaponInstance, weaponInstance.level + 1);

            if (weaponInstance.level >= weaponInstance.GetItem<Weapon>().weaponUpgrades.Length)
            {
                nextValue.text = "";
                return;
            }

            nextValue.text = (nextDamage).ToString();
        }

        string GetLabel()
        {
            string label = "";

            if (isPhysicalDamage)
            {
                label += Glossary.IsPortuguese() ? "Dano Físico" : "Physical Damage";
            }
            else if (isFireDamage)
            {
                label += Glossary.IsPortuguese() ? "Dano de Fogo" : "Fire Damage";
            }
            else if (isFrostDamage)
            {
                label += Glossary.IsPortuguese() ? "Dano de Gelo" : "Frost Damage";
            }
            else if (isLightningDamage)
            {
                label += Glossary.IsPortuguese() ? "Dano de Trovão" : "Lightning Damage";
            }
            else if (isMagicalDamage)
            {
                label += Glossary.IsPortuguese() ? "Dano de Magia" : "Magic Damage";
            }
            else if (isWaterDamage)
            {
                label += Glossary.IsPortuguese() ? "Dano de Água" : "Water Damage";
            }
            else if (isDarknessDamage)
            {
                label += Glossary.IsPortuguese() ? "Dano das Trevas" : "Darkness Damage";
            }
            else if (statusEffect != null)
            {
                label += Glossary.IsPortuguese() ? statusEffect.portugueseStatusEffectName : statusEffect.englishStatusEffectName;

                if (Glossary.IsPortuguese())
                {
                    label += " - Quantidade por dano";
                }
                else
                {
                    label += " - Amount per hit";
                }
            }

            return label;
        }


        int GetCurrentDamageForLevel(WeaponInstance weaponInstance, int level)
        {
            int currentDamage = 0;

            if (weaponInstance == null || weaponInstance.IsEmpty())
            {
                return currentDamage;
            }

            if (isPhysicalDamage)
            {
                currentDamage = weaponInstance.GetItem<Weapon>().GetDamageForLevel(level).physical;
            }
            else if (isFireDamage)
            {
                currentDamage = weaponInstance.GetItem<Weapon>().GetDamageForLevel(level).fire;
            }
            else if (isFrostDamage)
            {
                currentDamage = weaponInstance.GetItem<Weapon>().GetDamageForLevel(level).frost;
            }
            else if (isLightningDamage)
            {
                currentDamage = weaponInstance.GetItem<Weapon>().GetDamageForLevel(level).lightning;
            }
            else if (isMagicalDamage)
            {
                currentDamage = weaponInstance.GetItem<Weapon>().GetDamageForLevel(level).magic;
            }
            else if (isWaterDamage)
            {
                currentDamage = weaponInstance.GetItem<Weapon>().GetDamageForLevel(level).water;
            }
            else if (isDarknessDamage)
            {
                currentDamage = weaponInstance.GetItem<Weapon>().GetDamageForLevel(level).darkness;
            }
            else if (statusEffect != null)
            {
                StatusEffectEntry[] statusEffectEntries = weaponInstance.GetItem<Weapon>().GetDamageForLevel(level).statusEffects;

                for (int i = 0; i < statusEffectEntries.Length; i++)
                {
                    if (statusEffectEntries[i].statusEffect == statusEffect)
                    {
                        currentDamage = (int)statusEffectEntries[i].amountPerHit;
                        break;
                    }
                }
            }

            return currentDamage;
        }
    }
}
