namespace AF
{
    public class UI_QuickItem_LeftWeapon : UI_QuickItem
    {

        void Start()
        {
            character.characterBaseEquipment.onSwitchingLeftWeapon.AddListener(ShowItemIcon);
        }

        protected override void ShowItemIcon()
        {
            WeaponInstance weaponInstance = character.characterBaseEquipment.GetLeftHandWeapon();

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
