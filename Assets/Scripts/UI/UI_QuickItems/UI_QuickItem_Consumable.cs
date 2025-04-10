namespace AF
{
    public class UI_QuickItem_Consumable : UI_QuickItem
    {

        void Start()
        {
            character.characterBaseEquipment.onSwitchingConsumable.AddListener(ShowItemIcon);
        }

        protected override void ShowItemIcon()
        {
            Consumable consumable = character.characterBaseEquipment.GetConsumable();

            if (consumable != null)
            {
                equippedItemIcon.sprite = consumable.sprite;
                equippedItemContainer.gameObject.SetActive(true);
            }
            else
            {
                HideItemIcon();
            }

            PlayPopEffect();
        }
    }
}
