using AF.Health;
using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    public class PlayerThrowWeaponManager : MonoBehaviour
    {
        public PlayerManager playerManager;

        public Projectile weaponThrowProjectilePrefab;

        public void ThrowWeapon()
        {
            CharacterWeaponHitbox currentWeapon = playerManager.playerWeaponsManager.currentWeaponInstance;

            if (currentWeapon == null)
            {
                return;
            }

            currentWeapon.gameObject.SetActive(false);

            GameObject weaponThrowProjectileInstance = Instantiate(weaponThrowProjectilePrefab.gameObject, currentWeapon.transform.parent);
            GameObject clonedWeapon = Instantiate(currentWeapon.gameObject, weaponThrowProjectileInstance.transform.GetChild(0));

            // Enable the hitbox on the cloned weapon
            CharacterWeaponHitbox clonedWeaponHitbox = clonedWeapon.GetComponent<CharacterWeaponHitbox>();
            clonedWeaponHitbox.gameObject.SetActive(true);
            clonedWeaponHitbox.EnableHitbox();
            clonedWeaponHitbox.shouldDisableHitboxOnStart = false;

            Utils.UpdateTransformChildrenWhere(clonedWeaponHitbox.transform, (childObject) => childObject.GetComponent<IKHelper>() == null);

            if (clonedWeapon.TryGetComponent<CharacterTwoHandRef>(out var twoHandRef))
            {
                twoHandRef.enabled = false;
            }

            // Set the local scale, position, and rotation of the cloned weapon
            clonedWeapon.transform.localScale = new Vector3(1, 1, 1);
            clonedWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            // Throw the projectile instance
            playerManager.projectileSpawner.Throw(weaponThrowProjectileInstance);

            // Clear disappearing FX from this instance
            weaponThrowProjectileInstance.TryGetComponent<Projectile>(out var projectile);
            if (projectile != null)
            {
                projectile.disappearingFx = null;
            }

            // Destroy the projectile instance after throwing
            Destroy(weaponThrowProjectileInstance);
        }

        public void ShowCurrentWeapon()
        {
            CharacterWeaponHitbox currentWeapon = playerManager.playerWeaponsManager.currentWeaponInstance;

            if (currentWeapon == null)
            {
                return;
            }

            currentWeapon.gameObject.SetActive(true);
        }

    }
}
