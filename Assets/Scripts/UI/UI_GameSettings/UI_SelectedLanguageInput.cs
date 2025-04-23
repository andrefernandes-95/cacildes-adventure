using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace AF
{
    public class UI_SelectedLanguageInput : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private TextMeshProUGUI currentValueLabel;

        private void Awake()
        {
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnEnable()
        {
            int defaultValue = 0;

            if (LocalizationSettings.SelectedLocale.Identifier.Code == "en")
            {
                defaultValue = 0;
            }
            else if (LocalizationSettings.SelectedLocale.Identifier.Code == "pt")
            {
                defaultValue = 1;
            }

            dropdown.value = defaultValue;

            UpdateOptionLabels();

            LocalizationSettings.SelectedLocaleChanged += UpdateOptionLabels;

            Refresh();
        }

        void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= UpdateOptionLabels;
        }

        void UpdateOptionLabels(Locale locale = null)
        {
            dropdown.options[0].text = Glossary.IsPortuguese() ? "Inglês" : "English";
            dropdown.options[1].text = Glossary.IsPortuguese() ? "Português" : "Portuguese";
        }

        private void OnValueChanged(int value)
        {
            if (value == 0)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
            }
            else if (value == 1)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("pt");
            }

            Refresh();
        }

        public void Refresh()
        {
            if (currentValueLabel != null)
            {
                string localeCode = LocalizationSettings.SelectedLocale?.Identifier.Code;

                string currentValue = localeCode switch
                {
                    "en" => Glossary.IsPortuguese() ? "Inglês" : "English",
                    "pt" => Glossary.IsPortuguese() ? "Português" : "Portuguese",
                    _ => "Unknown",
                };
                currentValueLabel.text = currentValue;
            }
        }
    }
}
