using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_ItemContextualMenu : MonoBehaviour
    {
        [SerializeField] UI_CharacterInventory uI_CharacterInventory;

        [Header("Selected Items")]
        public ItemInstance selectedItemInstance;
        public Item selectedStackableItem;

        [Header("UI Components")]
        public TextMeshProUGUI selectedItemLabel;

        public Button equipButton;

        public Button buttonToAutoSelectUponEntering;

        private void OnEnable()
        {
            if (selectedItemInstance != null && selectedItemInstance.Exists())
            {
                selectedItemLabel.text = selectedItemInstance.GetItem<Item>().GetName();
            }
            else if (selectedStackableItem != null)
            {
                selectedItemLabel.text = selectedStackableItem.GetName();
            }

            UpdateEquipButtonLabel();

            if (buttonToAutoSelectUponEntering != null)
            {
                EventSystem.current.SetSelectedGameObject(buttonToAutoSelectUponEntering.gameObject);
            }
        }

        private void OnDisable()
        {
            ClearSelection();
        }

        void ClearSelection()
        {
            selectedItemInstance = null;
            selectedStackableItem = null;
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnEquip()
        {
            if (selectedItemInstance != null || selectedStackableItem != null)
            {
                // By default, attempt to equip an item on the first slot
                int slotToEquip = 0;

                // Unless it's a weapon, arrow, skill or consumable - in that case, check the active slot first so we can replace the item equipped with this new item
                if (selectedItemInstance is WeaponInstance)
                {
                    slotToEquip = uI_CharacterInventory.character.characterBaseEquipment.GetCurrentRightHandWeaponSlotIndex();
                }
                else if (selectedStackableItem is Arrow)
                {
                    slotToEquip = uI_CharacterInventory.character.characterBaseEquipment.GetCurrentArrowsSlotIndex();
                }
                else if (selectedItemInstance is SpellInstance)
                {
                    slotToEquip = uI_CharacterInventory.character.characterBaseEquipment.GetCurrentSkillsSlotIndex();
                }
                else if (selectedStackableItem is Consumable)
                {
                    slotToEquip = uI_CharacterInventory.character.characterBaseEquipment.GetCurrentConsumablesSlotIndex();
                }

                UI_EquipmentUtils.EquipItem(
                    uI_CharacterInventory.character,
                    selectedItemInstance,
                    selectedStackableItem,
                    slotToEquip,
                    // By default, try to equip any weapon from the inventory screen on the right hand
                    true,
                    false
                );
            }

            // Update item list to reflect any changes that might have occured
            uI_CharacterInventory.uI_ItemList.Refresh();

            OnGoBack();
        }

        void UpdateEquipButtonLabel()
        {

            bool isEquipped = selectedStackableItem != null && uI_CharacterInventory.character.characterBaseEquipment.IsItemEquipped(selectedStackableItem)
                            || selectedItemInstance != null && uI_CharacterInventory.character.characterBaseEquipment.IsItemInstanceEquipped(selectedItemInstance);

            if (isEquipped)
            {
                buttonToAutoSelectUponEntering.GetComponentInChildren<TextMeshProUGUI>().text =
                    Glossary.IsPortuguese() ? "Desequipar" : "Unequip";
            }
            else
            {
                buttonToAutoSelectUponEntering.GetComponentInChildren<TextMeshProUGUI>().text =
                    Glossary.IsPortuguese() ? "Equipar" : "Equip";
            }
        }

        /// <summary>
        /// Unity Event
        /// </summary>
        public void OnGoBack()
        {
            // Close contextual menu
            uI_CharacterInventory.HideItemContextualMenu();
        }
    }
}
