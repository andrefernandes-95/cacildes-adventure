namespace AF
{
    public class UI_QuickItem_Spell : UI_QuickItem
    {

        void Start()
        {
            character.characterBaseEquipment.onSwitchingSpell.AddListener(ShowItemIcon);
        }

        protected override void ShowItemIcon()
        {
            SpellInstance currentSpell = character.characterBaseEquipment.GetSpellInstance();

            if (currentSpell.Exists())
            {
                Spell spellItem = currentSpell.GetItem<Spell>();

                equippedItemIcon.sprite = spellItem.sprite;
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
