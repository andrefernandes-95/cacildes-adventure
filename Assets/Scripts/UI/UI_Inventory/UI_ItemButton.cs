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
                UI_EquipmentUtils.EquipItem(
                    uI_CharacterInventory.character,
                    itemInstance,
                    stackableItem,
                    uI_CharacterInventory.uI_ItemList.equippingSlotIndex,
                    uI_CharacterInventory.uI_ItemList.isAttemptingToEquipRightWeapon,
                    uI_CharacterInventory.uI_ItemList.isAttemptingToEquipLeftWeapon
                );

                uI_CharacterInventory.uI_CharacterEquipment.OpenEquipmentTab();
            }
            else
            {
                // Open Overlay Menu to Manage Item
                uI_CharacterInventory.uI_ItemContextualMenu.selectedItemInstance = itemInstance;
                uI_CharacterInventory.uI_ItemContextualMenu.selectedStackableItem = stackableItem;
                uI_CharacterInventory.ShowItemContextualMenu();
            }
        }

    }
}
