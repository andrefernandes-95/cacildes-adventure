using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AF
{
    public class UI_InventoryTab : MonoBehaviour
    {

        [InfoBox("Which button should be automatically selected upon entering this screen?")]
        public UI_EquipmentSlotButton defaultEquipmentSlotButtonToPreselect;

        void OnEnable()
        {
            if (EventSystem.current.currentSelectedGameObject == null && defaultEquipmentSlotButtonToPreselect != null)
            {
                EventSystem.current.SetSelectedGameObject(defaultEquipmentSlotButtonToPreselect.gameObject);
            }
        }

    }
}
