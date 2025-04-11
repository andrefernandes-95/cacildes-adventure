using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    public class UnarmedWorldWeapon : BaseWorldWeapon
    {
        public ActionItem actionItem;

        [Header("Collider")]
        public UnarmedDamageCollider unarmedDamageCollider;

        public override void OpenDamageCollider()
        {
            unarmedDamageCollider.EnableCollider();
        }

        public override void CloseDamageCollider()
        {
            unarmedDamageCollider.DisableCollider();
        }

        // Not applicable
        public override void SetWeaponInstance(WeaponInstance weaponInstance)
        {
        }
    }
}
