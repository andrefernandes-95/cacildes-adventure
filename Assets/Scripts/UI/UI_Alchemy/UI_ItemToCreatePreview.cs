namespace AF
{
    using AF.UI;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(CanvasGroup))]
    public class UI_ItemToCreatePreview : MonoBehaviour
    {

        [Header("UI")]
        public GameObject requiredMaterialsContainer;

        [Header("UI - Pieces")]
        CanvasGroup canvasGroup => GetComponent<CanvasGroup>();
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemDescription;
        public Image itemIcon;

        [Header("Prefabs")]
        public UI_UpgradeRequiredItemTooltip uI_UpgradeRequiredItemTooltip;

        public void ShowUI(CraftingRecipe craftingRecipe, CharacterBaseManager character)
        {
            if (craftingRecipe == null)
            {
                return;
            }

            DrawItemInfo(craftingRecipe.resultingItem);
            DrawRequiredMaterials(craftingRecipe, character);
            canvasGroup.alpha = 1f;
        }

        public void HideUI()
        {
            canvasGroup.alpha = 0f;
        }

        void DrawItemInfo(Item item)
        {
            itemName.text = item.GetName();
            itemDescription.text = item.GetDescription();
            itemIcon.sprite = item.sprite;
        }

        void DrawRequiredMaterials(CraftingRecipe craftingRecipe, CharacterBaseManager character)
        {
            Utils.ClearChildren(requiredMaterialsContainer.transform);

            foreach (CraftingIngredientEntry craftingIngredientEntry in craftingRecipe.ingredients)
            {
                UI_UpgradeRequiredItemTooltip uI_UpgradeRequiredItemTooltipInstance = Instantiate(uI_UpgradeRequiredItemTooltip, requiredMaterialsContainer.transform);
                uI_UpgradeRequiredItemTooltipInstance.gameObject.SetActive(true);
                uI_UpgradeRequiredItemTooltipInstance.ShowTooltip(character, craftingIngredientEntry.ingredient, craftingIngredientEntry.amount);
            }
        }
    }
}
