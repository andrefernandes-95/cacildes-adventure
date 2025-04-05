using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class UI_EquipmentSlotButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private AudioSource audioSource => GetComponent<AudioSource>();

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
        public CharacterBaseManager character;

        private void Awake()
        {
        }

        private void OnEnable()
        {
            DrawSprite();
        }

        void DrawSprite()
        {
            ItemInstance itemInstance = null;

            if (isRightHand)
            {
                itemInstance = character.characterBaseEquipment.GetRightWeaponInSlot(slotIndex);
            }
            else if (isLeftHand)
            {
                itemInstance = character.characterBaseEquipment.GetLeftWeaponInSlot(slotIndex);
            }
            else if (isArrow)
            {
                itemInstance = character.characterBaseEquipment.GetArrowInSlot(slotIndex);
            }
            else if (isSkills)
            {
                itemInstance = character.characterBaseEquipment.GetSpellInSlot(slotIndex);
            }
            else if (isConsumables)
            {
                itemInstance = character.characterBaseEquipment.GetConsumableInSlot(slotIndex);
            }
            else if (isAccessories)
            {
                itemInstance = character.characterBaseEquipment.GetAccessoryInSlot(slotIndex);
            }
            else if (isHelmet)
            {
                itemInstance = character.characterBaseEquipment.GetHelmetInstance();
            }
            else if (isArmor)
            {
                itemInstance = character.characterBaseEquipment.GetArmorInstance();
            }
            else if (isGauntlets)
            {
                itemInstance = character.characterBaseEquipment.GetGauntletInstance();
            }
            else if (isBoots)
            {
                itemInstance = character.characterBaseEquipment.GetLegwearInstance();
            }

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
        }

        private void OnLostFocus()
        {
        }

    }
}
