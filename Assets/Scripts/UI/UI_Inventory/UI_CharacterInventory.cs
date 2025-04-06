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
        [SerializeField] UI_FooterIndicator uI_FooterIndicator;

        public void ShowFooterTooltip(string enTooltip, string ptTooltip)
        {
            uI_FooterIndicator.DisplayTooltip(enTooltip, ptTooltip);
        }

    }
}
