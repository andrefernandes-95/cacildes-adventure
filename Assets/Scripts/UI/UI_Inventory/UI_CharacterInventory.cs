using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AF
{
    public class UI_CharacterInventory : MonoBehaviour
    {
        public CharacterBaseManager character;

        [Header("Inventory Components")]
        public UI_ItemsList uI_ItemList;
        public UI_ItemContextualMenu uI_ItemContextualMenu;
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;


        [Header("Equipment Components")]
        public UI_CharacterEquipment uI_CharacterEquipment;

        private void Awake()
        {
            HideItemContextualMenu();
        }

        private void OnDisable()
        {
            HideItemContextualMenu();
        }

        public void ShowFooterTooltip(string enTooltip, string ptTooltip)
        {
            uI_FooterIndicator.DisplayTooltip(enTooltip, ptTooltip);
        }

        public void HideItemContextualMenu()
        {
            uI_ItemContextualMenu.gameObject.SetActive(false);
        }

        public void ShowItemContextualMenu()
        {
            uI_ItemContextualMenu.gameObject.SetActive(true);
        }

    }
}
