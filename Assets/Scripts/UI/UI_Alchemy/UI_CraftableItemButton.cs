using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_CraftableItemButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Components")]
        [SerializeField] TextMeshProUGUI itemNameLabel;
        [SerializeField] Image itemIcon;
        [SerializeField] GameObject craftItemIndicator;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;
        [HideInInspector] public UnityEvent onCreateItem;

        [HideInInspector] public CraftingRecipe craftingRecipe;
        [HideInInspector] public CharacterBaseManager characterBaseManager;

        public void UpdateUI()
        {
            if (craftingRecipe == null)
            {
                return;
            }

            itemNameLabel.text = $"{craftingRecipe.resultingItem.GetName()}";

            if (craftingRecipe.resultingAmount > 0)
            {
                itemNameLabel.text += $" ({craftingRecipe.resultingAmount})";
            }

            itemIcon.sprite = craftingRecipe.resultingItem.sprite;
        }

        public void OnClickToCraftItem()
        {
            if (CraftingUtils.CanCraftItem(characterBaseManager.characterBaseInventory, craftingRecipe))
            {
                CraftItem();
            }
            else
            {
                Debug.Log("You don't have enough materials to create this item.");
            }
        }

        void CraftItem()
        {

            foreach (CraftingIngredientEntry ingredient in craftingRecipe.ingredients)
            {
                for (int i = 0; i < ingredient.amount; i++)
                {
                    characterBaseManager.characterBaseInventory.RemoveItem(ingredient.ingredient);
                }
            }

            for (int i = 0; i < craftingRecipe.resultingAmount; i++)
            {
                characterBaseManager.characterBaseInventory.AddConsumable(craftingRecipe.resultingItem as Consumable);
            }

            onCreateItem?.Invoke();
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke();

            ShowCraftIndicator();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke();

            HideCraftIndicator();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onSelect?.Invoke();

            ShowCraftIndicator();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onDeselect?.Invoke();

            HideCraftIndicator();
        }

        void ShowCraftIndicator()
        {
            if (craftItemIndicator != null)
            {
                craftItemIndicator.SetActive(true);
            }
        }

        void HideCraftIndicator()
        {
            if (craftItemIndicator != null)
            {
                craftItemIndicator.SetActive(false);
            }
        }
    }
}
