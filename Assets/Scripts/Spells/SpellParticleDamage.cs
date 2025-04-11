using UnityEngine;

namespace AF
{
    public class SpellParticleDamage : MonoBehaviour
    {
        [SerializeField] ProjectileSpellManager projectilSpellManager;

        private void OnParticleCollision(GameObject other)
        {
            if (other.TryGetComponent<DamageReceiver>(out var damageReceiver))
            {
                projectilSpellManager.OnCollision(damageReceiver.character, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
            }
        }
    }
}
