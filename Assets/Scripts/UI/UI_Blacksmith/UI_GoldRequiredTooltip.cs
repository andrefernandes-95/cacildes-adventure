using TMPro;
using UnityEngine;

namespace AF.UI
{
    public class UI_GoldRequiredTooltip : MonoBehaviour
    {
        public TextMeshProUGUI currentAmount;
        public TextMeshProUGUI requiredAmountLabel;

        public void ShowTooltip(CharacterBaseManager characterBaseManager, int requiredAmount)
        {
            bool hasEnoughMaterial = characterBaseManager.characterBaseGold.GetCurrentGold() >= requiredAmount;
            currentAmount.text = characterBaseManager.characterBaseGold.GetCurrentGold().ToString();
            currentAmount.color = hasEnoughMaterial ? Color.green : Color.red;
            requiredAmountLabel.text = "/" + requiredAmount.ToString();
        }
    }
}
