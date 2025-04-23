using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_CreditButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [Header("Components")]
        [SerializeField] Image thumbnail;
        [SerializeField] TextMeshProUGUI contributionType;
        [SerializeField] TextMeshProUGUI contributorName;
        [SerializeField] TextMeshProUGUI contributorDescription;
        [SerializeField] GameObject urlIndicator;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;

        [HideInInspector] public Contributor contributor;

        public void SetupButton(Contributor contributor)
        {
            this.contributor = contributor;

            if (contributor?.urlSprite)
            {
                thumbnail.sprite = contributor.urlSprite;
            }

            contributionType.text = Glossary.IsPortuguese() ? contributor.contributionType.labelPt : contributor.contributionType.labelEn;
            contributorName.text = contributor.author;
            contributorDescription.text = Glossary.IsPortuguese() ? contributor.ptContribution : contributor.enContribution;

            urlIndicator.SetActive(!string.IsNullOrEmpty(contributor.authorUrl));
        }

        public void OnClick()
        {
            if (contributor != null && !string.IsNullOrEmpty(contributor.authorUrl))
            {
                Application.OpenURL(contributor.authorUrl);
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke();
        }
    }
}
