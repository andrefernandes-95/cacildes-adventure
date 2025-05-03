using AF.Health;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AF.UI
{
    public class UI_UpgradeRequiredItemTooltip : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI label;
        public TextMeshProUGUI currentAmount;
        public TextMeshProUGUI requiredAmountLabel;

        public void ShowTooltip(CharacterBaseManager characterBaseManager, Item item, int requiredAmount)
        {
            icon.sprite = item.sprite;
            label.text = item.GetName();

            bool hasEnoughMaterial = characterBaseManager.characterBaseInventory.GetItemQuantity(item) >= requiredAmount;
            currentAmount.text = characterBaseManager.characterBaseInventory.GetItemQuantity(item).ToString();
            currentAmount.color = hasEnoughMaterial ? Color.green : Color.red;
            requiredAmountLabel.text = "/" + requiredAmount.ToString();
        }
    }
}
