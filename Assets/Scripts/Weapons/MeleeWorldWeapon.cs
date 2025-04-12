using NaughtyAttributes;
using UnityEngine;

namespace AF
{
    public class MeleeWorldWeapon : BaseWorldWeapon
    {
        [SerializeField] MeleeDamageCollider meleeDamageCollider;

        [Header("Back Pivot")]
        [SerializeField] Vector3 weaponInBack_localPosition;
        [SerializeField] Vector3 weaponInBack_localRotation;

        [Header("Two Handing Pivot")]
        [SerializeField] Vector3 twoHanding_localPosition;
        [SerializeField] Vector3 twoHanding_localRotation;

        Vector3 originalLocalPosition;
        Quaternion originalLocalRotation;

        private void Awake()
        {
            originalLocalPosition = transform.localPosition;
            originalLocalRotation = transform.localRotation;
        }

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

        public void ActivateTwoHandPivots()
        {
            transform.SetLocalPositionAndRotation(twoHanding_localPosition, Quaternion.Euler(twoHanding_localRotation));
        }

        public void RestoreDefaultPivots()
        {
            transform.SetLocalPositionAndRotation(originalLocalPosition, originalLocalRotation);
        }

        public void ActivatBackPivots()
        {
            transform.SetLocalPositionAndRotation(weaponInBack_localPosition, Quaternion.Euler(weaponInBack_localRotation));
        }

    }

}
