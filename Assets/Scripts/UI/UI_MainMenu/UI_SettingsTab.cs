using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AF
{
    public class UI_SettingsTab : MonoBehaviour
    {
        [SerializeField] GameObject defaultButton;
        void OnEnable()
        {
            if (defaultButton != null)
            {
                EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
            }
        }
    }
}
