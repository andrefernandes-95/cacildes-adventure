using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_ItemButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Icons")]
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI itemCount;
        [SerializeField] Image equippedIndicator;

        [Header("Equipment")]
        [HideInInspector] public UI_CharacterInventory uI_CharacterInventory;

        [Header("Is Stackable Item?")]
        public Item stackableItem;
        public int stackableItemAmount;

        [Header("Or Item Instance")]
        public ItemInstance itemInstance;



        public void SetupButton()
        {
            itemCount.gameObject.SetActive(false);
            equippedIndicator.gameObject.SetActive(false);

            if (stackableItem != null)
            {
                icon.sprite = stackableItem.sprite;
                itemCount.text = stackableItemAmount.ToString();
                itemCount.gameObject.SetActive(true);

                equippedIndicator.gameObject.SetActive(uI_CharacterInventory.character.characterBaseEquipment.IsItemEquipped(stackableItem));
            }
            else if (itemInstance != null && itemInstance.Exists())
            {
                icon.sprite = itemInstance.GetItem<Item>().sprite;
                equippedIndicator.gameObject.SetActive(uI_CharacterInventory.character.characterBaseEquipment.IsItemInstanceEquipped(itemInstance));
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnHover();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnHover();
        }

        public void OnSubmit(BaseEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnLostFocus();
        }

        private void OnHover()
        {
            string itemName = "";

            if (stackableItem != null)
            {
                itemName = stackableItem.GetName();
            }
            else if (itemInstance.Exists())
            {
                itemName = itemInstance.GetItem<Item>().name;
            }

            uI_CharacterInventory.ShowFooterTooltip(itemName, itemName);
        }

        private void OnLostFocus()
        {
        }

        /// <summary>
        /// UnityEvent
        /// </summary>
        public void OnClick()
        {
            if (uI_CharacterInventory.uI_ItemList.IsAttemptingToEquipItems())
            {
                // Slot to equip
                int slot = uI_CharacterInventory.uI_ItemList.equippingSlotIndex;
                if (slot < 0)
                {
                    Debug.LogError("Error: Attempting to equip something on a slot with index of -1");
                    return;
                }

                CharacterBaseEquipment characterEquipment = uI_CharacterInventory.character.characterBaseEquipment;

                // Attempting to equip weapon?
                bool isEquippingRightWeapon = uI_CharacterInventory.uI_ItemList.isAttemptingToEquipRightWeapon;
                bool isEquippingLeftWeapon = uI_CharacterInventory.uI_ItemList.isAttemptingToEquipLeftWeapon;

                if ((isEquippingRightWeapon || isEquippingLeftWeapon) && itemInstance is WeaponInstance weaponInstance)
                {
                    uI_CharacterInventory.character.characterWeapons.EquipWeapon(
                        weaponInstance,
                        slot,
                        isEquippingRightWeapon);
                    return;
                }

                // Attempting to equip arrows?
                if (stackableItem is Arrow arrow)
                {
                    characterEquipment.EquipArrow(arrow, slot);
                    return;
                }

                // Attempting to equip skills?
                if (itemInstance is SpellInstance spellInstance)
                {
                    characterEquipment.EquipSkill(spellInstance, slot);
                    return;
                }

                // Attempting to equip accessory?
                if (itemInstance is AccessoryInstance accessoryInstance)
                {
                    characterEquipment.EquipAccessory(accessoryInstance, slot);
                    return;
                }

                // Attempting to equip consumable?
                if (stackableItem is Consumable consumable)
                {
                    characterEquipment.EquipConsumable(consumable, slot);
                    return;
                }

                if (itemInstance is HelmetInstance helmetInstance)
                {
                    characterEquipment.EquipHelmet(helmetInstance);
                    return;
                }

                if (itemInstance is ArmorInstance armorInstance)
                {
                    characterEquipment.EquipArmor(armorInstance);
                    return;
                }

                if (itemInstance is GauntletInstance gauntletInstance)
                {
                    characterEquipment.EquipGauntlets(gauntletInstance);
                    return;
                }

                if (itemInstance is LegwearInstance legwearInstance)
                {
                    characterEquipment.EquipLegwear(legwearInstance);
                    return;
                }
            }
            else
            {
                // Open Overlay Menu to Manage Item
            }
        }

    }
}
