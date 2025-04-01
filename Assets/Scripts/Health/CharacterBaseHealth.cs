
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AF.Health
{
    public abstract class CharacterBaseHealth : MonoBehaviour
    {
        [Header("Components")]
        public CharacterBaseManager character;

        [Header("Events")]
        public UnityEvent onStart;
        public UnityEvent onTakeDamage;
        public UnityEvent onRestoreHealth;
        public UnityEvent onDeath;
        public UnityEvent onDamageFromPlayer;

        [HideInInspector]
        public UnityEvent onHealthChange;

        [Header("Quests")]
        public Weapon weaponRequiredToKill;
        public bool hasBeenHitWithRequiredWeapon = false;
        public UnityEvent onKilledWithRightWeapon;
        public UnityEvent onKilledWithWrongWeapon;


        [Header("Sounds")]
        public AudioClip postureHitSfx;
        public AudioClip postureBrokeSfx;
        public AudioClip deathSfx;
        public AudioClip dodgeSfx;
        public AudioSource audioSource;

        [Header("Status")]
        public bool hasHealthCutInHalf = false;


        private void Start()
        {
            onStart?.Invoke();

            onHealthChange?.Invoke();
        }

        public abstract void RestoreHealth(float value);
        public abstract void RestoreFullHealth();

        public float GetCurrentHealthPercentage()
        {
            return GetCurrentHealth() * 100 / GetMaxHealth();
        }

        public abstract void TakeDamage(float value);

        public abstract float GetCurrentHealth();
        public abstract void SetCurrentHealth(float value);

        public abstract int GetMaxHealth();
        public abstract void SetMaxHealth(int value);

        public void PlayPostureHit()
        {
            if (audioSource != null && postureHitSfx != null && Random.Range(0, 100f) >= 50f)
            {
                audioSource.pitch = Random.Range(0.91f, 1.05f);
                audioSource.PlayOneShot(postureHitSfx);
            }
        }
        public void PlayPostureBroke()
        {
            if (audioSource != null && postureBrokeSfx != null)
            {
                audioSource.PlayOneShot(postureBrokeSfx);
            }
        }
        public void PlayDodge()
        {
            if (audioSource != null && dodgeSfx != null)
            {
                audioSource.PlayOneShot(dodgeSfx);
            }
        }
        public void PlayDeath()
        {
            if (audioSource != null && deathSfx != null)
            {
                audioSource.PlayOneShot(deathSfx);
            }
        }

        public void CheckIfHasBeenKilledWithRightWeapon()
        {
            if (weaponRequiredToKill == null)
            {
                return;
            }

            if (hasBeenHitWithRequiredWeapon)
            {
                onKilledWithRightWeapon?.Invoke();
            }
            else
            {
                onKilledWithWrongWeapon?.Invoke();
            }
        }

        public virtual void SetHasHealthCutInHealth(bool value)
        {
            hasHealthCutInHalf = value;
        }

        public bool IsDead()
        {
            return GetCurrentHealth() <= 0;
        }


        public float GetExtraAttackBasedOnCurrentHealth()
        {
            var percentage = GetCurrentHealth() * 100 / GetMaxHealth() * 0.01;

            if (percentage > 0.9)
            {
                return 0;
            }
            else if (percentage > 0.8)
            {
                return 0.05f;
            }
            else if (percentage > 0.7)
            {
                return 0.1f;
            }
            else if (percentage > 0.6)
            {
                return 0.2f;
            }
            else if (percentage > 0.5)
            {
                return 0.5f;
            }
            else if (percentage > 0.4)
            {
                return 0.6f;
            }
            else if (percentage > 0.3)
            {
                return 0.8f;
            }
            else if (percentage > 0.2)
            {
                return 1.2f;
            }
            else if (percentage > 0.1)
            {
                return 1.5f;
            }
            else if (percentage > 0)
            {
                return 2f;
            }

            return 0f;
        }
    }

}
