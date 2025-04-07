using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AF
{
    public class UI_CharacterEquipment : MonoBehaviour
    {
        public CharacterBaseManager character;
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;

        [Header("Main Menu")]
        [SerializeField] UI_MainMenu uI_MainMenu;

        [Header("Inventory")]
        public UI_ItemsList uI_ItemsList; // Reference items list so we can change the items list behaviour for equipment mode

        public void ShowFooterTooltip(string enTooltip, string ptTooltip)
        {
            uI_FooterIndicator.DisplayTooltip(enTooltip, ptTooltip);
        }

        public void SwitchToInventoryTab()
        {
            uI_MainMenu.OpenInventoryTab();
        }

    }
}
