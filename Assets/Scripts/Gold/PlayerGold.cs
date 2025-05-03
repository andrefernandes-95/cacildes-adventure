using UnityEngine;

namespace AF
{
    public class PlayerGold : CharacterBaseGold
    {
        [SerializeField] PlayerStatsDatabase playerStatsDatabase;

        public override int GetCurrentGold()
        {
            return playerStatsDatabase.gold;
        }

        public override void SetCurrentGold(int value)
        {
            playerStatsDatabase.gold = value;
        }

        public override void AddGold(int value)
        {
            playerStatsDatabase.gold += value;
        }

        public override void RemoveGold(int value)
        {
            playerStatsDatabase.gold -= value;

            if (playerStatsDatabase.gold < 0)
            {
                playerStatsDatabase.gold = 0;
            }
        }
    }
}
