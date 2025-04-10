namespace AF
{
    public class UI_QuickItem_RightWeapon : UI_QuickItem
    {

        void Start()
        {
            character.characterBaseEquipment.onSwitchingRightWeapon.AddListener(ShowItemIcon);
        }

        protected override void ShowItemIcon()
        {
            WeaponInstance weaponInstance = character.characterBaseEquipment.GetRightHandWeapon();

            if (weaponInstance.Exists())
            {
                Weapon weapon = weaponInstance.GetItem<Weapon>();

                equippedItemIcon.sprite = weapon.sprite;
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
