namespace AF
{
    using AF.Stats;
    using UnityEngine;

    public class PlayerStats : CharacterBaseStats
    {
        [SerializeField] PlayerStatsDatabase playerStatsDatabase;
        [SerializeField] StatsBonusController statsBonusController;

        public override int GetVitality()
        {
            return playerStatsDatabase.vitality + statsBonusController.GetCurrentVitalityBonus();
        }

        public override int GetEndurance()
        {
            return playerStatsDatabase.endurance + statsBonusController.GetCurrentEnduranceBonus();
        }

        public override int GetIntelligence()
        {
            return playerStatsDatabase.intelligence + statsBonusController.GetCurrentIntelligenceBonus();
        }


        public override int GetStrength()
        {
            return playerStatsDatabase.strength + statsBonusController.GetCurrentStrengthBonus();
        }

        public override int GetDexterity()
        {
            return playerStatsDatabase.dexterity + statsBonusController.GetCurrentDexterityBonus();
        }

        public override int GetReputation()
        {
            return playerStatsDatabase.reputation + statsBonusController.GetCurrentReputationBonus();
        }

        public override int GetCurrentLevel()
        {
            return playerStatsDatabase.GetCurrentLevel();
        }

        public override void ResetStats()
        {
            playerStatsDatabase.vitality = playerStatsDatabase.endurance = playerStatsDatabase.intelligence = playerStatsDatabase.strength
            = playerStatsDatabase.dexterity = 1;
        }
    }
}
