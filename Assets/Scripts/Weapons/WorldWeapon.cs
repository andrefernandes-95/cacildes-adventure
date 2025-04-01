using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    public class WorldWeapon : MonoBehaviour
    {
        [HideInInspector] public CharacterBaseManager owner;

        public MeleeDamageCollider damageCollider;
        public TrailRenderer trailRenderer;

        [Header("Sounds")]
        [SerializeField] Soundpack swooshes;

        private void Awake()
        {
            if (owner == null)
            {
                owner = GetComponentInParent<CharacterBaseManager>();
            }

            damageCollider.enabled = false;

            StopTrail();
        }

        void EnableTrail()
        {
            if (trailRenderer != null)
            {
                trailRenderer.emitting = true;
            }
        }

        void StopTrail()
        {
            if (trailRenderer != null)
            {
                trailRenderer.emitting = false;
            }
        }

        public void OpenDamageCollider()
        {
            damageCollider.enabled = true;
            EnableTrail();

            owner.characterSoundManager.PlaySoundpack(swooshes);
        }

        public void CloseDamageCollider()
        {
            damageCollider.enabled = false;
            StopTrail();
        }
    }
}
