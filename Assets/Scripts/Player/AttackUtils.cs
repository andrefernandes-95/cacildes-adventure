using UnityEngine;

namespace AF
{
    public static class AttackUtils
    {

        public static (bool, bool, bool) CompareDamage(int current, int next)
        {
            if (next > current)
            {
                return (true, false, false);
            }
            else if (next < current)
            {
                return (false, true, false);
            }
            else
            {
                return (false, false, true);
            }
        }

        public static int GetScalingBonus(float value, Scaling damageScaling)
        {
            if (damageScaling == Scaling.S)
            {
                value *= 4;
            }
            else if (damageScaling == Scaling.A)
            {
                value *= 3.2f;
            }
            else if (damageScaling == Scaling.B)
            {
                value *= 2.4f;
            }
            else if (damageScaling == Scaling.C)
            {
                value *= 1.8f;
            }
            else if (damageScaling == Scaling.D)
            {
                value *= 1.2f;
            }
            return Mathf.FloorToInt(value);

        }

    }
}
