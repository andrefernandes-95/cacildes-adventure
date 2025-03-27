using AF.Stats;
using UnityEngine;

namespace AF
{
    public class PlayerPosture : CharacterAbstractPosture
    {
        public PlayerStatsDatabase playerStatsDatabase;
        public StatsBonusController statsBonusController;
        public PlayerManager playerManager;

        public float POSTURE_DECREASE_RATE_BONUS = 2.25f;

        public override int GetMaxPostureDamage()
        {
            return 100 + GetExtraPostureBasedOnStats();
        }
        int GetExtraPostureBasedOnStats()
        {
            return GetBonusByStat(statsBonusController.GetCurrentStrength()) + GetBonusByStat(statsBonusController.GetCurrentVitality());
        }

        int GetBonusByStat(int stat)
        {

            // Ensure strength is at least 1 to avoid log(0)
            if (stat < 1)
            {
                stat = 1;
            }

            // Apply a logarithmic function to the strength value
            // We use log base 10 for simplicity, adjust as needed
            double logValue = Mathf.Log10(stat);

            // Scale the result to get desired output range
            // Adjust the scaling factor (e.g., 50) and offset (e.g., 10) as needed
            int extraPosture = (int)(logValue * 50 + 10);

            return extraPosture;
        }

        public void ResetPosture()
        {
            currentPostureDamage = 0;
        }

        public override float GetPostureDecreateRate()
        {
            return POSTURE_DECREASE_RATE_BONUS + statsBonusController.postureDecreaseRateBonus;
        }

        public override bool CanPlayPostureDamagedEvent()
        {
            return playerManager.thirdPersonController.isSwimming == false;
        }
    }
}
