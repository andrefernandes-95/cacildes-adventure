using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    public class UnarmedWorldWeapon : WorldWeapon
    {

        [Header("Attack Actions")]
        public List<AttackAction> rightBumperActions = new();
        public List<AttackAction> rightTriggerActions = new();
        public List<AttackAction> leftBumperActions = new();

        [Header("Collider")]
        public UnarmedDamageCollider unarmedDamageCollider;

        public override void EnableCollider()
        {
            unarmedDamageCollider.EnableCollider();
        }

        public override void DisableCollider()
        {
            unarmedDamageCollider.DisableCollider();
        }

    }
}
