using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AF.Combat;
using AF.Health;
using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    public class CharacterWeaponHitbox : MonoBehaviour
    {
        [Header("Weapon")]
        public Weapon weapon;

        [Header("Trails")]
        public TrailRenderer trailRenderer;
        public BoxCollider hitCollider => GetComponent<BoxCollider>();

        [Header("Components")]
        public CharacterBaseManager character;
        PlayerManager playerManager;

        [Header("Tags To Ignore")]
        public List<string> tagsToIgnore = new();

        [Header("SFX")]
        public AudioClip swingSfx;
        public AudioClip hitSfx;
        public AudioSource combatAudioSource;

        readonly List<IDamageable> damageReceiversHit = new();

        [Header("Events")]
        public UnityEvent onOpenHitbox;
        public UnityEvent onCloseHitbox;
        public UnityEvent onDamageInflicted;
        public UnityEvent onWeaponSpecial;

        [Header("Character Weapon Addons")]
        public CharacterTwoHandRef characterTwoHandRef;
        public CharacterWeaponBuffs characterWeaponBuffs;

        // Scene References
        Soundbank soundbank;
        WeaponCollisionFXManager weaponCollisionFXManager;

        // Internal flags
        bool canPlayHitSfx = true;

        List<BoxCollider> ownColliders = new();


        // Useful for throwable weapon situation
        [HideInInspector] public bool shouldDisableHitboxOnStart = true;



        private void Awake()
        {
            ownColliders = GetComponents<BoxCollider>()?.ToList();
            playerManager = character as PlayerManager;
        }

        void Start()
        {
            if (shouldDisableHitboxOnStart)
            {
                DisableHitbox();
            }
        }

        public void ShowWeapon()
        {
            gameObject.SetActive(true);

            if (characterTwoHandRef != null)
            {
                characterTwoHandRef.EvaluateTwoHandingUpdate();
            }
        }

        public void HideWeapon()
        {
            gameObject.SetActive(false);
        }

        public void EnableHitbox()
        {
            canPlayHitSfx = true;

            if (trailRenderer != null)
            {
                trailRenderer.Clear();

                trailRenderer.enabled = true;
            }

            if (hitCollider != null)
            {
                hitCollider.enabled = true;
            }

            if (swingSfx != null && HasSoundbank())
            {
                combatAudioSource.pitch = Random.Range(0.9f, 1.1f);
                combatAudioSource.Stop();

                soundbank.PlaySound(swingSfx, combatAudioSource);
            }

            onOpenHitbox?.Invoke();
        }

        public void DisableHitbox()
        {
            if (trailRenderer != null)
            {
                trailRenderer.enabled = false;
            }

            if (hitCollider != null)
            {
                hitCollider.enabled = false;
            }

            if (ownColliders?.Count > 1)
            {
                foreach (var collider in ownColliders)
                {
                    collider.enabled = false;
                }
            }

            damageReceiversHit.Clear();
            onCloseHitbox?.Invoke();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (HasWeaponCollisionManager())
            {
                weaponCollisionFXManager.EvaluateCollision(other, this.gameObject);
            }

            if (ShouldIgnoreCollision(other))
            {
                return;
            }

            if (other.TryGetComponent(out IDamageable damageable) && !damageReceiversHit.Contains(damageable))
            {
                damageReceiversHit.Add(damageable);
                damageable.OnDamage(character, () =>
                {
                    onDamageInflicted?.Invoke();

                    if (hitSfx != null && canPlayHitSfx && character != null)
                    {
                        canPlayHitSfx = false;
                        PlayHitSound();
                    }
                });

                if (playerManager != null)
                {
                    playerManager.playerCombatController.HandlePlayerAttack(damageable, weapon);
                }
            }
        }

        private bool ShouldIgnoreCollision(Collider other)
        {
            if (tagsToIgnore.Contains(other.tag))
            {
                return true;
            }

            return false;
        }

        private void PlayHitSound()
        {
            if (HasSoundbank() && combatAudioSource != null)
            {
                combatAudioSource.pitch = Random.Range(0.9f, 1.1f);
                soundbank.PlaySound(hitSfx, combatAudioSource);
            }
        }

        bool HasSoundbank()
        {
            if (soundbank == null)
            {
                soundbank = FindAnyObjectByType<Soundbank>(FindObjectsInactive.Include);

                return soundbank != null;
            }

            return true;
        }

        public bool UseCustomTwoHandTransform()
        {
            return characterTwoHandRef != null;
        }

        bool HasWeaponCollisionManager()
        {
            if (weaponCollisionFXManager == null)
            {
                weaponCollisionFXManager = FindAnyObjectByType<WeaponCollisionFXManager>(FindObjectsInactive.Include);

                return weaponCollisionFXManager != null;
            }

            return true;
        }
    }
}
