using UnityEngine;

namespace AF
{
    public abstract class BaseWorldWeapon : MonoBehaviour
    {
        public abstract void OpenDamageCollider();

        public abstract void CloseDamageCollider();

        public abstract void SetWeaponInstance(WeaponInstance weaponInstance);
    }
}
