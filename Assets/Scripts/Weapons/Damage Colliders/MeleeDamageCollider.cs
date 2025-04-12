using AF.Health;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

namespace AF
{
    public class MeleeDamageCollider : DamageCollider
    {
        [HideInInspector] public CharacterBaseManager damageOwner;
        [HideInInspector] public WeaponInstance weaponInstance;

        [Header("VFX")]
        public TrailRenderer trailRenderer;

        [Header("Sounds")]
        [SerializeField] Soundpack swooshes;
        [SerializeField] Soundpack hits;
        [SerializeField] Soundpack weaponDrawn;

        protected override void Awake()
        {
            base.Awake();

            damageOwner = GetComponentInParent<CharacterBaseManager>();
        }

        private void OnEnable()
        {
            if (weaponDrawn != null && damageOwner != null)
            {
                weaponDrawn.Play(damageOwner);
            }
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


        public override void EnableCollider()
        {
            base.EnableCollider();

            EnableTrail();

            if (swooshes != null)
            {
                swooshes.Play(damageOwner);
            }
        }

        public override void DisableCollider()
        {
            base.DisableCollider();

            StopTrail();
        }

        protected override void ProcessDamageEffect(DamageReceiver damageReceiver)
        {
            if (!CanAttackTarget(damageReceiver))
            {
                return;
            }
            if (weaponInstance == null || weaponInstance.IsEmpty())
            {
                Debug.LogError("No weapon instance found on MeleeDamageCollider");
                return;
            }

            Damage finalDamage = damageOwner.characterBaseAttackManager.GetAttackingWeaponDamage();

            TakeDamageEffect takeDamageEffect = Instantiate(damageReceiver.character.characterEffectsManager.characterEffectsDatabase.takeDamageEffect);
            takeDamageEffect.damage = finalDamage;
            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.attacker = damageOwner;
            takeDamageEffect.receiver = damageReceiver.character;
            takeDamageEffect.angleHitFrom = Vector3.SignedAngle(damageOwner.transform.forward, damageReceiver.transform.forward, Vector3.up);

            damageReceiver.character.characterEffectsManager.ProcessInstantEffect(takeDamageEffect);

            if (hits != null)
            {
                hits.Play(damageOwner);
            }
        }

        bool CanAttackTarget(DamageReceiver targetToDamage)
        {
            if (targetToDamage == damageOwner?.damageReceiver)
            {
                return false;
            }

            if (damagedTargets.Contains(targetToDamage))
            {
                return false;
            }

            return true;
        }

        protected override void ProcessTakeBlockedDamageEffect(DamageReceiver damageReceiver)
        {
            if (!CanAttackTarget(damageReceiver))
            {
                return;
            }

            TakeBlockedDamageEffect takeDamageEffect = Instantiate(damageReceiver.character.characterEffectsManager.characterEffectsDatabase.takeBlockedDamageEffect);
            Damage finalDamage = damageOwner.characterBaseAttackManager.GetAttackingWeaponDamage();
            takeDamageEffect.damage = finalDamage;
            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.attacker = null;
            takeDamageEffect.receiver = damageReceiver.character;
            takeDamageEffect.angleHitFrom = Vector3.SignedAngle(transform.forward, damageReceiver.transform.forward, Vector3.up);

            damageReceiver.character.characterEffectsManager.ProcessInstantEffect(takeDamageEffect);

            if (hits != null)
            {
                hits.Play(damageOwner);
            }
        }
    }
}
