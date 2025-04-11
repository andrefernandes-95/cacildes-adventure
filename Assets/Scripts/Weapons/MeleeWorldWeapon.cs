using NaughtyAttributes;
using UnityEngine;

namespace AF
{
    public class MeleeWorldWeapon : BaseWorldWeapon
    {
        [SerializeField] MeleeDamageCollider meleeDamageCollider;

        public void ActivateEquippedWorldWeapon()
        {
            meleeDamageCollider.gameObject.SetActive(true);
        }

        public void DeactivateEquippedWorldWeapon()
        {
            meleeDamageCollider.gameObject.SetActive(false);
        }

        public override void OpenDamageCollider()
        {
            meleeDamageCollider.EnableCollider();
        }

        public override void CloseDamageCollider()
        {
            meleeDamageCollider.DisableCollider();
        }

        public override void SetWeaponInstance(WeaponInstance weaponInstance)
        {
            meleeDamageCollider.weaponInstance = weaponInstance;
        }
    }
}
