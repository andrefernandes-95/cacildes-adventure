using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "Items / Item / New Arrow")]
    public class Arrow : ConsumableProjectile
    {

        public bool isBolt = false;
        public bool isRifleBullet = false;
        public bool loseUponFiring = true;

    }
}
