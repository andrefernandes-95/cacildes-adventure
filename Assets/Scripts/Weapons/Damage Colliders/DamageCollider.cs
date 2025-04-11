using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public abstract class DamageCollider : MonoBehaviour
    {
        protected List<DamageReceiver> damagedTargets = new();
        protected Vector3 contactPoint;
        new Collider collider => GetComponent<Collider>();

        protected virtual void Awake()
        {
            ClearDamagedTargets();

            DisableCollider();
        }

        public virtual void EnableCollider()
        {
            collider.enabled = true;
        }

        public virtual void DisableCollider()
        {
            collider.enabled = false;
        }

        void ClearDamagedTargets()
        {
            damagedTargets.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            DamageReceiver targetToDamage = GetTargetFromCollider(other);

            if (targetToDamage != null)
            {
                contactPoint = other.ClosestPointOnBounds(transform.position);

                // Can Block?
                if (CanBlockDamage(targetToDamage.character))
                {
                    ProcessTakeBlockedDamageEffect(targetToDamage);
                }
                else
                {
                    ProcessDamageEffect(targetToDamage);
                }
            }
        }

        public virtual bool CanBlockDamage(CharacterBaseManager target)
        {
            if (!target.characterBlockController.isBlocking)
            {
                return false;
            }

            // Is target facing this damage source?
            Vector3 directionAttackToTargetThatIsBlocking = transform.position - target.transform.position;
            if (Vector3.Dot(directionAttackToTargetThatIsBlocking, target.transform.forward) > 0.3)
            {
                return true;
            }

            return false;
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

        protected abstract void ProcessDamageEffect(DamageReceiver damageReceiver);

        protected abstract void ProcessTakeBlockedDamageEffect(DamageReceiver damageReceiver);
    }
}
