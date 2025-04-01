namespace AF
{
    using AF.Health;
    using NaughtyAttributes;
    using UnityEngine;

    public class UnarmedDamageCollider : DamageCollider
    {
        [InfoBox("This is the collider own damage. It will be combined with the character base attack damage")]
        public Damage damage;

        protected override void ProcessDamageEffect(CharacterBaseManager attacker, DamageReceiver damageReceiver)
        {
            Damage finalDamage = attacker.GetAttackDamage().Combine(damage);

            TakeDamageEffect takeDamageEffect = Instantiate(damageReceiver.character.characterEffectsManager.characterEffectsDatabase.takeDamageEffect);
            takeDamageEffect.damage = finalDamage;
            takeDamageEffect.contactPoint = contactPoint;
        }
    }
}
