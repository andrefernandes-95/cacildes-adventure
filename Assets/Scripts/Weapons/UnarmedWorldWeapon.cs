using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    public class UnarmedWorldWeapon : WorldWeapon
    {

        [Header("Attack Actions")]
        public List<AttackAction> rightLightAttacks = new();
        public List<AttackAction> leftLightAttacks = new();

        [Header("Collider")]
        public UnarmedDamageCollider unarmedDamageCollider;

        public override void EnableCollider()
        {
            unarmedDamageCollider.enabled = true;
        }

        public override void DisableCollider()
        {
            unarmedDamageCollider.enabled = false;
        }

    }
}
