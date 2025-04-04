using AF.Health;
using NaughtyAttributes;
using UnityEngine;

namespace AF
{
    public class MeleeDamageCollider : DamageCollider
    {
        [HideInInspector] public WeaponInstance weaponInstance;

        protected override void ProcessDamageEffect(CharacterBaseManager attacker, DamageReceiver damageReceiver)
        {
            if (weaponInstance == null || weaponInstance.IsEmpty())
            {
                Debug.LogError("No weapon instance found on MeleeDamageCollider");
                return;
            }

            Damage finalDamage = attacker.characterBaseAttackManager.GetAttackingWeaponDamage();

            TakeDamageEffect takeDamageEffect = Instantiate(damageReceiver.character.characterEffectsManager.characterEffectsDatabase.takeDamageEffect);
            takeDamageEffect.damage = finalDamage;
            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.attacker = attacker;
            takeDamageEffect.receiver = damageReceiver.character;
            takeDamageEffect.angleHitFrom = Vector3.SignedAngle(attacker.transform.forward, damageReceiver.transform.forward, Vector3.up);

            damageReceiver.character.characterEffectsManager.ProcessInstantEffect(takeDamageEffect);
        }
    }
}
