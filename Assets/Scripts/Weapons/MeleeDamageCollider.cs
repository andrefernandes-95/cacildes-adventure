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

            Damage finalDamage = weaponInstance.GetItem<Weapon>().GetCurrentDamage(attacker, weaponInstance.level);

            TakeDamageEffect takeDamageEffect = Instantiate(damageReceiver.character.characterEffectsManager.characterEffectsDatabase.takeDamageEffect);
            takeDamageEffect.damage = finalDamage;
            takeDamageEffect.contactPoint = contactPoint;

            damageReceiver.character.characterEffectsManager.ProcessInstantEffect(takeDamageEffect);
        }
    }
}
