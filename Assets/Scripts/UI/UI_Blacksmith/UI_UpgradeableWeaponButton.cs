using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_UpgradeableWeaponButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Components")]
        [SerializeField] TextMeshProUGUI weaponNameLabel;
        [SerializeField] Image weaponIcon;
        [SerializeField] GameObject upgradeWeaponIndicator;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;
        [HideInInspector] public UnityEvent onUpgradeWeapon;

        [HideInInspector] public WeaponInstance weaponInstance;
        [HideInInspector] public CharacterBaseManager characterBaseManager;

        public void UpdateUI()
        {
            if (weaponInstance == null || weaponInstance.IsEmpty())
            {
                return;
            }

            Weapon weapon = weaponInstance.GetItem<Weapon>();
            weaponNameLabel.text = $"{weapon.GetName()} +{weaponInstance.level}";
            weaponIcon.sprite = weapon.sprite;
        }

        public void OnClickToUpgradeWeapon()
        {
            if (CraftingUtils.CanImproveWeapon(
                characterBaseManager.characterBaseInventory, weaponInstance, characterBaseManager.characterBaseGold.GetCurrentGold()))
            {
                UpgradeWeapon();
            }
            else
            {
                Debug.Log("You don't have enough materials to upgrade this weapon.");
            }
        }

        void UpgradeWeapon()
        {
            if (weaponInstance.GetItem<Weapon>().TryGetNextWeaponUpgradeLevel(out WeaponUpgradeLevel weaponUpgradeForNextLevel))
            {
                var currentWeaponLevel = weaponInstance.level;

                // Remove character gold
                characterBaseManager.characterBaseGold.RemoveGold(weaponUpgradeForNextLevel.goldCostForUpgrade);

                foreach (var upgradeMaterial in weaponUpgradeForNextLevel.upgradeMaterials)
                {
                    characterBaseManager.characterBaseInventory.RemoveItem(upgradeMaterial.Key);
                }

                int nextLevel = currentWeaponLevel + 1;

                characterBaseManager.characterBaseInventory.UpdateWeaponLevel(weaponInstance, nextLevel);

                // Now check if the weapon is in the right or left hand and update the level accordingly, since the equipment has its own cloned instances of each weapon
                // This is a bit of a hack, but it works for now.
                if (characterBaseManager.characterBaseEquipment.GetRightHandWeapons().Contains(weaponInstance))
                {
                    characterBaseManager.characterBaseEquipment.UpdateWeaponLevel(weaponInstance, nextLevel, true);
                }
                else if (characterBaseManager.characterBaseEquipment.GetLeftHandWeapons().Contains(weaponInstance))
                {
                    characterBaseManager.characterBaseEquipment.UpdateWeaponLevel(weaponInstance, nextLevel, false);
                }

                onUpgradeWeapon?.Invoke();
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke();

            ShowUpgradeWeaponIndicator();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke();

            HideUpgradeWeaponIndicator();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onSelect?.Invoke();

            ShowUpgradeWeaponIndicator();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onDeselect?.Invoke();

            HideUpgradeWeaponIndicator();
        }

        void ShowUpgradeWeaponIndicator()
        {
            if (upgradeWeaponIndicator != null)
            {
                upgradeWeaponIndicator.SetActive(true);
            }
        }

        void HideUpgradeWeaponIndicator()
        {
            if (upgradeWeaponIndicator != null)
            {
                upgradeWeaponIndicator.SetActive(false);
            }
        }
    }
}
