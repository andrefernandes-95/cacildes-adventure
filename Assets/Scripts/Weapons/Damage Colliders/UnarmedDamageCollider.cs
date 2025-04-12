namespace AF
{
    using AF.Health;
    using NaughtyAttributes;
    using UnityEngine;

    public class UnarmedDamageCollider : DamageCollider
    {
        [HideInInspector] public CharacterBaseManager damageOwner;

        [InfoBox("This is the collider own damage. It will be combined with the character base attack damage")]
        public Damage damage = new();

        [Header("Sounds")]
        [SerializeField] Soundpack swooshes;
        [SerializeField] Soundpack hits;
        [SerializeField] Soundpack weaponDrawn;

        [Header("VFX")]
        [SerializeField] TrailRenderer trailRenderer;

        protected override void Awake()
        {
            base.Awake();

            damageOwner = GetComponentInParent<CharacterBaseManager>();
        }

        public override void EnableCollider()
        {
            base.EnableCollider();

            EnableTrail();

            if (swooshes != null && damageOwner != null)
            {
                swooshes.Play(damageOwner);
            }
        }

        public override void DisableCollider()
        {
            base.DisableCollider();
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


        protected override void ProcessDamageEffect(DamageReceiver damageReceiver)
        {
            if (!CanAttackTarget(damageReceiver))
            {
                return;
            }

            TakeDamageEffect takeDamageEffect = Instantiate(damageReceiver.character.characterEffectsManager.characterEffectsDatabase.takeDamageEffect);
            takeDamageEffect.damage = damageOwner.characterBaseAttackManager.GetAttackingWeaponDamage();
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

        protected override void ProcessTakeBlockedDamageEffect(DamageReceiver damageReceiver)
        {
            TakeBlockedDamageEffect takeBlockedDamageEffect = Instantiate(damageReceiver.character.characterEffectsManager.characterEffectsDatabase.takeBlockedDamageEffect);
            Damage finalDamage = damageOwner.characterBaseAttackManager.GetAttackingWeaponDamage();
            takeBlockedDamageEffect.damage = finalDamage;
            takeBlockedDamageEffect.contactPoint = contactPoint;
            takeBlockedDamageEffect.attacker = null;
            takeBlockedDamageEffect.receiver = damageReceiver.character;
            takeBlockedDamageEffect.angleHitFrom = Vector3.SignedAngle(transform.forward, damageReceiver.transform.forward, Vector3.up);

            ShieldInstance shieldInstance = (ShieldInstance)(damageReceiver.character.combatManager.currentAttackingMember == AttackingMember.RIGHT_HAND
                ? damageReceiver.character.characterBaseEquipment.GetRightHandWeapon()
                : damageReceiver.character.characterBaseEquipment.GetLeftHandWeapon());

            if (shieldInstance == null || shieldInstance.IsEmpty())
            {
                Debug.Log("Error blocking, could not find shield instance equipped on character that is blocking");
                return;
            }

            Shield shield = shieldInstance.GetItem<Shield>();
            takeBlockedDamageEffect.shield = shield;

            damageReceiver.character.characterEffectsManager.ProcessInstantEffect(takeBlockedDamageEffect);
            if (hits != null)
            {
                hits.Play(damageOwner);
            }
        }
    }
}
