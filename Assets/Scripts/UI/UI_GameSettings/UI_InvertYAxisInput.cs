using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    public class UI_InvertYAxisInput : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private Toggle toggle;
        [SerializeField] private TextMeshProUGUI currentValueLabel;

        private void Awake()
        {
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnEnable()
        {
            if (gameSettings == null)
            {
                Debug.LogError("No game settings found. Assign it on the editor!");
                return;
            }

            toggle.isOn = gameSettings.invertYAxis;
            Refresh();
        }

        private void OnValueChanged(bool value)
        {
            if (gameSettings != null)
            {
                gameSettings.invertYAxis = value;
                Refresh();
            }
        }

        public void Refresh()
        {
            if (currentValueLabel != null)
            {
                currentValueLabel.text = toggle.isOn
                    ? Glossary.IsPortuguese() ? "Ativo" : "Enabled"
                    : Glossary.IsPortuguese() ? "Inativo" : "Disabled";
            }
        }
    }
}
