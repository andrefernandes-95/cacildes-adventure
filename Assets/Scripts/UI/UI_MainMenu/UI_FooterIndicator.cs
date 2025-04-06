using TMPro;
using UnityEngine;

namespace AF
{
    public class UI_FooterIndicator : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;

        public void DisplayTooltip(string enTooltip, string ptTooltip)
        {
            // Disable and reenable later to trigger animation
            gameObject.SetActive(false);

            if (Glossary.IsPortuguese())
            {
                label.text = ptTooltip;
            }
            else
            {
                label.text = enTooltip;
            }

            gameObject.SetActive(true);
        }
    }
}
