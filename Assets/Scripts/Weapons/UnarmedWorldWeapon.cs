using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    public class UnarmedWorldWeapon : WorldWeapon
    {
        public ActionItem actionItem;

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
