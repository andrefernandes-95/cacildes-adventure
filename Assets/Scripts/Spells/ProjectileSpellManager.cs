using System.Collections.Generic;
using AF.Health;
using UnityEngine;

namespace AF
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileSpellManager : MonoBehaviour
    {
        [HideInInspector] public CharacterBaseManager spellOwner;
        [SerializeField] float upwardsVelocity = 5;
        [SerializeField] float forwardVelocity = 3;
        List<CharacterBaseManager> hitTargets = new();

        public DestroyableParticle spellExplosionUponHittingTarget;

        Rigidbody rigidbody => GetComponent<Rigidbody>();

        public void InitializeSpell(CharacterBaseManager owner)
        {
            spellOwner = owner;

            if (owner.characterBaseTargetManager.currentTarget != null)
            {
                transform.LookAt(owner.characterBaseTargetManager.currentTarget.transform.position);
            }
            else
            {
                transform.forward = spellOwner.transform.forward;
            }

            Vector3 upwardVelocityVector = transform.up * upwardsVelocity;
            Vector3 forwardVelocityVector = transform.forward * forwardVelocity;
            Vector3 totalVelocity = upwardVelocityVector + forwardVelocityVector;
            rigidbody.linearVelocity = totalVelocity;

        }

        private void Update()
        {
            if (spellOwner != null && spellOwner.characterBaseTargetManager.currentTarget != null)
            {
                transform.LookAt(spellOwner.characterBaseTargetManager.currentTarget.transform.position);

                rigidbody.linearVelocity += transform.forward;
            }
        }

        public void OnCollision(CharacterBaseManager characterToDamage, Vector3 contactPoint)
        {
            if (!CanIDamageThisTarget(characterToDamage))
            {
                return;
            }

            TakeDamageEffect takeDamageEffect = Instantiate(characterToDamage.characterEffectsManager.characterEffectsDatabase.takeDamageEffect);

            spellOwner.characterBaseMagicManager.CalculateCurrentSpellDamage();
            takeDamageEffect.damage = spellOwner.characterBaseMagicManager.currentSpellDamage;

            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.attacker = spellOwner;
            takeDamageEffect.receiver = characterToDamage;
            takeDamageEffect.angleHitFrom = Vector3.SignedAngle(spellOwner.transform.forward, characterToDamage.transform.forward, Vector3.up);

            characterToDamage.characterEffectsManager.ProcessInstantEffect(takeDamageEffect);

            hitTargets.Add(characterToDamage);

            // Process hit vfx
            if (spellExplosionUponHittingTarget != null)
            {
                Instantiate(spellExplosionUponHittingTarget, contactPoint, Quaternion.identity);
            }
        }

        bool CanIDamageThisTarget(CharacterBaseManager characterToDamage)
        {
            if (characterToDamage.transform.root == spellOwner.transform.root)
            {
                return false;
            }

            if (hitTargets.Contains(characterToDamage))
            {
                return false;
            }

            return true;
        }
    }
}
