using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace AF
{
    public class UI_GraphicsQualityInput : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private TextMeshProUGUI currentValueLabel;

        private void Awake()
        {
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnEnable()
        {
            if (gameSettings == null)
            {
                Debug.LogError("No game settings found. Assign it on the editor!");
                return;
            }

            dropdown.value = gameSettings.graphicsQuality;

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
            dropdown.options[0].text = Glossary.IsPortuguese() ? "Baixa" : "Low";
            dropdown.options[1].text = Glossary.IsPortuguese() ? "Média" : "Medium";
            dropdown.options[2].text = Glossary.IsPortuguese() ? "Alta" : "High";
            dropdown.options[3].text = Glossary.IsPortuguese() ? "Máxima" : "Maximum";
        }

        private void OnValueChanged(int value)
        {
            if (gameSettings != null)
            {
                gameSettings.SetGameQuality(value);
                Refresh();
            }
        }

        public void Refresh()
        {
            if (currentValueLabel != null)
            {
                string quality = gameSettings.graphicsQuality switch
                {
                    0 => Glossary.IsPortuguese() ? "Baixa" : "Low",
                    1 => Glossary.IsPortuguese() ? "Média" : "Medium",
                    2 => Glossary.IsPortuguese() ? "Alta" : "High",
                    3 => Glossary.IsPortuguese() ? "Máxima" : "Maximum",
                    _ => "Unknown",
                };
                currentValueLabel.text = quality;
            }
        }
    }
}
