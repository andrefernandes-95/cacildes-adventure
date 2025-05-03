using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AF.UI
{
    public class UI_ItemTooltipBase : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI label;
        public TextMeshProUGUI currentValue;
        public TextMeshProUGUI nextValue;

        public virtual void ShowTooltip(ItemInstance itemInstance)
        {

        }

    }
}
