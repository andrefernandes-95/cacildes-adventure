namespace AF
{
    using AF.Health;
    using NaughtyAttributes;
    using UnityEngine;

    public class UnarmedDamageCollider : DamageCollider
    {
        [InfoBox("This is the collider own damage. It will be combined with the character base attack damage")]
        public Damage damage = new();

        protected override void ProcessDamageEffect(CharacterBaseManager attacker, DamageReceiver damageReceiver)
        {
            TakeDamageEffect takeDamageEffect = Instantiate(damageReceiver.character.characterEffectsManager.characterEffectsDatabase.takeDamageEffect);
            takeDamageEffect.damage = attacker.characterBaseAttackManager.GetAttackingWeaponDamage();
            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.attacker = attacker;
            takeDamageEffect.receiver = damageReceiver.character;
            takeDamageEffect.angleHitFrom = Vector3.SignedAngle(attacker.transform.forward, damageReceiver.transform.forward, Vector3.up);

            damageReceiver.character.characterEffectsManager.ProcessInstantEffect(takeDamageEffect);
        }
    }
}
