using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    public class UI_Statbar : MonoBehaviour
    {
        public Slider slider;
        public TextMeshProUGUI currentAndMaxValues;

        public void SetCurrentValue(float value)
        {
            slider.value = value;
        }

        public void RefreshCurrentAndMaxValues(int currentValue, int maxValue)
        {
            currentAndMaxValues.text = $"{currentValue}/{maxValue}";
        }
    }
}
