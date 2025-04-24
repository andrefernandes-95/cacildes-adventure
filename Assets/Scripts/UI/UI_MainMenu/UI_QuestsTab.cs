using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AF
{
    public class UI_QuestsTab : MonoBehaviour
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
