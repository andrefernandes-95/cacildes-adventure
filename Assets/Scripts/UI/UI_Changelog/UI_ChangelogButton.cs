using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AF
{
    public class UI_ChangelogButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [Header("Components")]
        [SerializeField] Image changelogThumbnail;
        [SerializeField] TextMeshProUGUI date;
        [SerializeField] TextMeshProUGUI version;
        [SerializeField] TextMeshProUGUI updateType;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] GameObject additions;
        [SerializeField] GameObject additionsContent;
        [SerializeField] GameObject improvements;
        [SerializeField] GameObject improvementsContent;
        [SerializeField] GameObject bugfixes;
        [SerializeField] GameObject bugfixesContent;

        [SerializeField] GameObject changelogDescriptionPrefab;

        [HideInInspector] public UnityEvent onSelect;
        [HideInInspector] public UnityEvent onDeselect;

        public void SetupButton(Changelog changelog)
        {
            if (changelog.changelogThumbnail != null)
            {
                changelogThumbnail.sprite = changelog.changelogThumbnail;
                changelogThumbnail.gameObject.SetActive(true);
            }
            else
            {
                changelogThumbnail.gameObject.SetActive(false);
            }

            date.text = changelog.date;
            version.text = changelog.name;

            updateType.text = "";
            if (changelog.updateType == Changelog.UpdateType.SMALL_UPDATE)
            {
                updateType.text = Glossary.IsPortuguese() ? "Atualização Pequena" : "Small Update";
            }
            else if (changelog.updateType == Changelog.UpdateType.BIG_UPDATE)
            {
                updateType.text = Glossary.IsPortuguese() ? "Atualização Grande" : "Big Update";
            }
            else if (changelog.updateType == Changelog.UpdateType.EXPANSION)
            {
                updateType.text = Glossary.IsPortuguese() ? "Expansão" : "Expansion";
            }

            description.text = changelog.smallDescription.IsEmpty ? "" : changelog.smallDescription.GetLocalizedString();

            if (changelog.additions.Length > 0)
            {
                additions.SetActive(true);
                additionsContent.SetActive(true);
                foreach (var addition in changelog.additions)
                {
                    var clone = Instantiate(changelogDescriptionPrefab, additionsContent.transform);
                    clone.GetComponent<TextMeshProUGUI>().text = addition.GetLocalizedString();
                }
            }
            else
            {
                additions.SetActive(false);
            }

            if (changelog.improvements.Length > 0)
            {
                improvements.SetActive(true);
                improvementsContent.SetActive(true);
                foreach (var improvement in changelog.improvements)
                {
                    var clone = Instantiate(changelogDescriptionPrefab, improvementsContent.transform);
                    clone.GetComponent<TextMeshProUGUI>().text = improvement.GetLocalizedString();
                }
            }
            else
            {
                improvements.SetActive(false);
            }


            if (changelog.bugfixes.Length > 0)
            {
                bugfixes.SetActive(true);
                bugfixesContent.SetActive(true);
                foreach (var bugfix in changelog.bugfixes)
                {
                    var clone = Instantiate(changelogDescriptionPrefab, bugfixesContent.transform);
                    clone.GetComponent<TextMeshProUGUI>().text = bugfix.GetLocalizedString();
                }
            }
            else
            {
                bugfixes.SetActive(false);
            }

        }

        public void OnClick()
        {
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
