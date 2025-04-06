using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AF
{
    public class UI_EquipmentTab : MonoBehaviour
    {

        [InfoBox("Which button should be automatically selected upon entering this screen?")]
        public UI_EquipmentSlotButton defaultEquipmentSlotButtonToPreselect;

        void OnEnable()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(defaultEquipmentSlotButtonToPreselect.gameObject);
            }
        }

    }
}
