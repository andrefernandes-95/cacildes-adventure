using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

namespace AF
{
    public class UI_EquipmentSlotButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Icons")]
        [SerializeField] Sprite unequippedSprite;
        [SerializeField] Image icon;

        [Header("Slot Settings")]
        public bool isRightHand = false;
        public bool isLeftHand = false;
        public bool isArrow = false;
        public bool isSkills = false;
        public bool isAccessories = false;
        public bool isConsumables = false;
        public bool isHelmet = false;
        public bool isArmor = false;
        public bool isGauntlets = false;
        public bool isBoots = false;
        public int slotIndex = 0;

        [Header("Equipment")]
        public UI_CharacterEquipment uI_CharacterEquipment;

        private void Awake()
        {
        }

        private void OnEnable()
        {
            DrawSprite();
        }

        void DrawSprite()
        {
            ItemInstance itemInstance = GetSlotItemInstance();

            if (itemInstance == null || itemInstance.IsEmpty())
            {
                icon.sprite = unequippedSprite;
            }
            else
            {
                icon.sprite = itemInstance.GetItem<Item>().sprite;
            }
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


            Item item = GetSlotItem();
            if (item == null)
            {
                enTooltip = GetSlotEnglishLabel();
                ptTooltip = GetSlotPortugueseLabel();
            }
            else
            {
                enTooltip = item.GetName();
                ptTooltip = item.GetName();
            }

            uI_CharacterEquipment.ShowFooterTooltip(enTooltip, ptTooltip);
        }

        private void OnLostFocus()
        {
        }

        ItemInstance GetSlotItemInstance()
        {
            ItemInstance itemInstance = null;

            if (isRightHand)
            {
                itemInstance = uI_CharacterEquipment.character.characterBaseEquipment.GetRightWeaponInSlot(slotIndex);
            }
            else if (isLeftHand)
            {
                itemInstance = uI_CharacterEquipment.character.characterBaseEquipment.GetLeftWeaponInSlot(slotIndex);
            }
            else if (isSkills)
            {
                itemInstance = uI_CharacterEquipment.character.characterBaseEquipment.GetSpellInSlot(slotIndex);
            }
            else if (isAccessories)
            {
                itemInstance = uI_CharacterEquipment.character.characterBaseEquipment.GetAccessoryInSlot(slotIndex);
            }
            else if (isHelmet)
            {
                itemInstance = uI_CharacterEquipment.character.characterBaseEquipment.GetHelmetInstance();
            }
            else if (isArmor)
            {
                itemInstance = uI_CharacterEquipment.character.characterBaseEquipment.GetArmorInstance();
            }
            else if (isGauntlets)
            {
                itemInstance = uI_CharacterEquipment.character.characterBaseEquipment.GetGauntletInstance();
            }
            else if (isBoots)
            {
                itemInstance = uI_CharacterEquipment.character.characterBaseEquipment.GetLegwearInstance();
            }

            return itemInstance;
        }

        public Item GetSlotItem()
        {
            if (isArrow)
            {
                return uI_CharacterEquipment.character.characterBaseEquipment.GetArrowInSlot(slotIndex);
            }
            else if (isConsumables)
            {
                return uI_CharacterEquipment.character.characterBaseEquipment.GetConsumableInSlot(slotIndex);
            }
            ItemInstance itemInstance = GetSlotItemInstance();

            if (itemInstance.Exists())
            {
                return itemInstance.GetItem<Item>();
            }

            return null;
        }

        string GetSlotEnglishLabel()
        {
            string label = "";

            if (isRightHand)
            {
                label = "Right Hand Weapons & Shields";
            }
            else if (isLeftHand)
            {
                label = "Left Hand Weapons & Shields";
            }
            else if (isArrow)
            {
                label = "Arrows";
            }
            else if (isSkills)
            {
                label = "Skills & Spells";
            }
            else if (isConsumables)
            {
                label = "Consumables";
            }
            else if (isAccessories)
            {
                label = "Accessories";
            }
            else if (isHelmet)
            {
                label = "Helmet";
            }
            else if (isArmor)
            {
                label = "Armor";
            }
            else if (isGauntlets)
            {
                label = "Gauntlets";
            }
            else if (isBoots)
            {
                label = "Boots";
            }

            return label;
        }


        string GetSlotPortugueseLabel()
        {
            string label = "";

            if (isRightHand)
            {
                label = "Mão Direita (Armas & Escudos)";
            }
            else if (isLeftHand)
            {
                label = "Mão Esquerda (Armas & Escudos)";
            }
            else if (isArrow)
            {
                label = "Flechas";
            }
            else if (isSkills)
            {
                label = "Abilidades & Feitiços";
            }
            else if (isConsumables)
            {
                label = "Consumíveis";
            }
            else if (isAccessories)
            {
                label = "Acessórios";
            }
            else if (isHelmet)
            {
                label = "Elmos";
            }
            else if (isArmor)
            {
                label = "Armaduras";
            }
            else if (isGauntlets)
            {
                label = "Manoplas";
            }
            else if (isBoots)
            {
                label = "Botas";
            }

            return label;
        }

        // Unity Event
        public void OnClick()
        {
            if (isLeftHand)
            {
                uI_CharacterEquipment.uI_ItemsList.isAttemptingToEquipLeftWeapon = true;
                uI_CharacterEquipment.uI_ItemsList.FilterForWeapons();
            }
            else if (isRightHand)
            {
                uI_CharacterEquipment.uI_ItemsList.isAttemptingToEquipRightWeapon = true;
                uI_CharacterEquipment.uI_ItemsList.FilterForWeapons();
            }
            else if (isArrow)
            {
                uI_CharacterEquipment.uI_ItemsList.FilterForArrows();
            }
            else if (isAccessories)
            {
                uI_CharacterEquipment.uI_ItemsList.FilterForAccessories();
            }
            else if (isConsumables)
            {
                uI_CharacterEquipment.uI_ItemsList.FilterForConsumables();
            }
            else if (isSkills)
            {
                uI_CharacterEquipment.uI_ItemsList.FilterForSkills();
            }
            else if (isHelmet)
            {
                uI_CharacterEquipment.uI_ItemsList.FilterForHelmets();
            }
            else if (isArmor)
            {
                uI_CharacterEquipment.uI_ItemsList.FilterForArmors();
            }
            else if (isGauntlets)
            {
                uI_CharacterEquipment.uI_ItemsList.FilterForGauntlets();
            }
            else if (isBoots)
            {
                uI_CharacterEquipment.uI_ItemsList.FilterForBoots();
            }

            uI_CharacterEquipment.uI_ItemsList.SetIsAttemptingToEquipItems(true, slotIndex);
            uI_CharacterEquipment.OpenInventoryTab();
        }
    }
}
