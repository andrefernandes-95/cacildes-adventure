namespace AF
{
    using AF.Health;
    using NaughtyAttributes;
    using UnityEngine;

    public class EnvironmentDamageCollider : DamageCollider
    {

        [InfoBox("This is the collider own damage. It will be combined with the character base attack damage")]
        public Damage damage = new();

        [InfoBox("How much time until the list of hit targets gets reset")]
        public float intervalBeforeClearingTargets = 1f;

        protected override void Awake()
        {
            base.Awake();

            EnableCollider();
        }

        void ClearTargets()
        {
            damagedTargets.Clear();
        }

        public override void EnableCollider()
        {
            base.EnableCollider();
        }

        public override void DisableCollider()
        {
            base.DisableCollider();
        }

        bool CanAttackTarget(DamageReceiver targetToDamage)
        {
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
            takeDamageEffect.damage = damage;
            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.attacker = null;
            takeDamageEffect.receiver = damageReceiver.character;
            takeDamageEffect.angleHitFrom = Vector3.SignedAngle(transform.forward, damageReceiver.transform.forward, Vector3.up);

            damageReceiver.character.characterEffectsManager.ProcessInstantEffect(takeDamageEffect);

            Invoke(nameof(ClearTargets), intervalBeforeClearingTargets);
        }


        protected override void ProcessTakeBlockedDamageEffect(DamageReceiver damageReceiver)
        {
            if (!CanAttackTarget(damageReceiver))
            {
                return;
            }

            TakeBlockedDamageEffect takeBlockedDamageEffect = Instantiate(damageReceiver.character.characterEffectsManager.characterEffectsDatabase.takeBlockedDamageEffect);
            takeBlockedDamageEffect.damage = damage;
            takeBlockedDamageEffect.contactPoint = contactPoint;
            takeBlockedDamageEffect.attacker = null;
            takeBlockedDamageEffect.receiver = damageReceiver.character;
            takeBlockedDamageEffect.angleHitFrom = Vector3.SignedAngle(transform.forward, damageReceiver.transform.forward, Vector3.up);

            WeaponInstance shieldInstance = (WeaponInstance)(damageReceiver.character.combatManager.currentAttackingMember == AttackingMember.RIGHT_HAND
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
        }

    }
}
