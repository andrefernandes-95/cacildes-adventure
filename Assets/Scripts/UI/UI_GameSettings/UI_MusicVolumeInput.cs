using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    public class UI_MusicVolumeInput : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI currentValueLabel;

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnEnable()
        {
            if (gameSettings == null)
            {
                Debug.LogError("No game settings found. Assign it on the editor!");
                return;
            }

            slider.value = gameSettings.musicVolume;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            Refresh();
        }

        private void OnSliderValueChanged(float value)
        {
            if (gameSettings != null)
            {
                gameSettings.SetMusicVolume(value);
                Refresh();
            }
        }

        public void Refresh()
        {
            if (currentValueLabel != null)
            {
                currentValueLabel.text = slider.value.ToString("F2"); // Format to 2 decimal places
            }
        }
    }
}
