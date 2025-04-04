using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

namespace AF
{
    public class WorldWeapon : MonoBehaviour
    {
        [HideInInspector] public CharacterBaseManager owner;

        [ShowIf("IsWorldWeapon()")]
        public MeleeDamageCollider damageCollider;
        public bool IsWorldWeapon()
        {
            return this is WorldWeapon;
        }

        public TrailRenderer trailRenderer;

        [Header("Sounds")]
        [SerializeField] Soundpack swooshes;

        private void Awake()
        {
            if (owner == null)
            {
                owner = GetComponentInParent<CharacterBaseManager>();
            }

            DisableCollider();
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

        public virtual void EnableCollider()
        {
            damageCollider.EnableCollider();
        }

        public virtual void DisableCollider()
        {
            damageCollider.DisableCollider();
        }

        public void OpenDamageCollider()
        {
            EnableTrail();
            EnableCollider();

            owner.characterSoundManager.PlaySoundpack(swooshes);
        }

        public void CloseDamageCollider()
        {
            DisableCollider();

            StopTrail();
        }
    }
}
