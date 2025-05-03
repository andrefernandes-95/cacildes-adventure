using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AF.UI
{
    public class UI_UpgradeItemTooltip_WeaponLevel : UI_ItemTooltipBase
    {
        public void ShowTooltip(WeaponInstance weaponInstance)
        {

            if (weaponInstance == null || weaponInstance.IsEmpty()) return;

            icon.sprite = weaponInstance.item.sprite;
            label.text = Glossary.IsPortuguese() ? "Nível" : "Level";

            currentValue.text = weaponInstance.level.ToString();

            Weapon weapon = weaponInstance.GetItem<Weapon>();

            if (weaponInstance.level >= weapon.weaponUpgrades.Length)
            {
                nextValue.text = "";
                return;
            }

            nextValue.text = (weaponInstance.level + 1).ToString();
        }
    }
}
