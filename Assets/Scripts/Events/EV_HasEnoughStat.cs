using System.Collections;
using AF.Stats;
using UnityEngine;

namespace AF
{

    public class EV_HasEnoughStat : EV_Condition
    {

        [Header("Stat")]
        public bool isVitality;
        public bool isEndurance;
        public bool isStrength;
        public bool isDexterity;
        public bool isIntelligence;

        public bool equal = false;
        public bool greaterOrEqualThan = false;
        public bool greaterThan = false;
        public bool lessThan = false;
        public bool lessOrEqualThan = false;

        public int value = 0;

        [Header("Components")]
        public StatsBonusController playerStatsBonusController;

        [Header("Databases")]
        public PlayerStatsDatabase playerStatsDatabase;

        public override IEnumerator Dispatch()
        {
            bool finalValue = false;

            int currentValue = 0;
            if (isVitality)
            {
                currentValue = playerStatsBonusController.GetCurrentVitality();
            }
            else if (isEndurance)
            {
                currentValue = playerStatsBonusController.GetCurrentEndurance();
            }
            else if (isStrength)
            {
                currentValue = playerStatsBonusController.GetCurrentStrength();
            }
            else if (isDexterity)
            {
                currentValue = playerStatsBonusController.GetCurrentDexterity();
            }
            else if (isIntelligence)
            {
                currentValue = playerStatsBonusController.GetCurrentIntelligence();
            }

            if (equal)
            {
                finalValue = currentValue == value;
            }
            else if (greaterOrEqualThan)
            {
                finalValue = currentValue >= value;
            }
            else if (greaterThan)
            {
                finalValue = currentValue > value;
            }
            else if (lessThan)
            {
                finalValue = currentValue < value;
            }
            else if (lessOrEqualThan)
            {
                finalValue = currentValue <= value;
            }

            yield return DispatchConditionResults(finalValue);
        }
    }

}
