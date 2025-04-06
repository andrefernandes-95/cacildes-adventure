using UnityEngine;
using UnityEngine.EventSystems;

namespace AF
{
    public class UI_InventoryFilterButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Slot Settings")]
        public bool isWeapons = false;
        public bool isShields = false;
        public bool isArrows = false;
        public bool isSkills = false;
        public bool isAccessories = false;
        public bool isConsumables = false;
        public bool isHelmet = false;
        public bool isArmor = false;
        public bool isGauntlets = false;
        public bool isBoots = false;
        public bool isAll = false;

        [Header("Equipment")]
        public UI_CharacterInventory uI_CharacterInventory;

        private void Awake()
        {
        }

        private void OnEnable()
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnHover();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnHover();
        }

        public void OnSubmit(BaseEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnLostFocus();
        }

        private void OnHover()
        {
            string enTooltip = "";
            string ptTooltip = "";


            enTooltip = GetSlotEnglishLabel();
            ptTooltip = GetSlotPortugueseLabel();

            uI_CharacterInventory.ShowFooterTooltip(enTooltip, ptTooltip);
        }

        private void OnLostFocus()
        {
        }


        string GetSlotEnglishLabel()
        {
            string label = "";

            if (isWeapons)
            {
                label = "Show weapons";
            }
            else if (isShields)
            {
                label = "Show shields";
            }
            else if (isArrows)
            {
                label = "Show arrows";
            }
            else if (isSkills)
            {
                label = "Show skills & spells";
            }
            else if (isConsumables)
            {
                label = "Show consumables";
            }
            else if (isAccessories)
            {
                label = "Show accessories";
            }
            else if (isHelmet)
            {
                label = "Show helmets";
            }
            else if (isArmor)
            {
                label = "Show armors";
            }
            else if (isGauntlets)
            {
                label = "Show gauntlets";
            }
            else if (isBoots)
            {
                label = "Show boots";
            }

            return label;
        }


        string GetSlotPortugueseLabel()
        {
            string label = "";

            if (isWeapons)
            {
                label = "Mostrar armas";
            }
            else if (isShields)
            {
                label = "Mostrar escudos";
            }
            else if (isArrows)
            {
                label = "Mostrar flechas";
            }
            else if (isSkills)
            {
                label = "Mostrar abilidades & feitiços";
            }
            else if (isConsumables)
            {
                label = "Mostra consumíveis";
            }
            else if (isAccessories)
            {
                label = "Mostrar acessórios";
            }
            else if (isHelmet)
            {
                label = "Mostrar elmos";
            }
            else if (isArmor)
            {
                label = "Mostrar armaduras";
            }
            else if (isGauntlets)
            {
                label = "Mostrar manoplas";
            }
            else if (isBoots)
            {
                label = "Mostrar botas";
            }

            return label;
        }
    }
}
