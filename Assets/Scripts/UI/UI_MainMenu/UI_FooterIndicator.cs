using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.PropertyVariants;

namespace AF
{
    public class UI_FooterIndicator : MonoBehaviour
    {
        [Header("Tooltip Label")]
        [SerializeField] TextMeshProUGUI tooltipLabel;

        [Header("Contextual Actions")]
        [SerializeField] GameObject actionsContainer;

        private void OnEnable()
        {
            foreach (Transform child in actionsContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void AddFooterActionButton(GameObject prefab, string actionLabel)
        {
            GameObject button = Instantiate(prefab, actionsContainer.transform);
            var target = button.GetComponentInChildren<UI_ActionDescription>();
            if (target.TryGetComponent(out GameObjectLocalizer comp))
            {
                Destroy(comp);
            }
            target.GetComponent<TextMeshProUGUI>().text = actionLabel;
        }

        public void DisplayTooltip(string enTooltip, string ptTooltip)
        {
            // Disable and reenable later to trigger animation
            gameObject.SetActive(false);

            if (Glossary.IsPortuguese())
            {
                tooltipLabel.text = ptTooltip;
            }
            else
            {
                tooltipLabel.text = enTooltip;
            }

            gameObject.SetActive(true);
        }

        public void Refresh()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}
