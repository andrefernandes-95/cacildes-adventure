using System.Collections.Generic;
using AF.Health;
using UnityEngine;
using UnityEngine.AI;

namespace AF
{
    public abstract class DamageCollider : MonoBehaviour
    {
        private CharacterBaseManager damageOwner;
        List<DamageReceiver> damagedTargets = new();

        protected Vector3 contactPoint;

        protected virtual void Awake()
        {
            ClearDamagedTargets();

            if (damageOwner == null)
            {
                damageOwner = GetComponentInParent<CharacterBaseManager>();
            }
        }

        void ClearDamagedTargets()
        {
            damagedTargets.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            DamageReceiver targetToDamage = GetTargetFromCollider(other);

            if (targetToDamage != null && CanAttackTarget(targetToDamage))
            {
                contactPoint = other.ClosestPointOnBounds(transform.position);

                ProcessDamageEffect(damageOwner, targetToDamage);
            }
        }

        DamageReceiver GetTargetFromCollider(Collider other)
        {
            DamageReceiver target = null;

            target = other.GetComponent<DamageReceiver>();
            if (target == null)
            {
                target = other.GetComponentInParent<DamageReceiver>();
            }

            return target;
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

        protected abstract void ProcessDamageEffect(CharacterBaseManager attacker, DamageReceiver damageReceiver);
    }
}
