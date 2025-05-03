using UnityEngine;

namespace AF
{
    public class CharacterGold : CharacterBaseGold
    {
        [SerializeField] int gold = 0;

        public override int GetCurrentGold()
        {
            return gold;
        }

        public override void SetCurrentGold(int value)
        {
            gold = value;
        }

        public override void AddGold(int value)
        {
            gold += value;
        }

        public override void RemoveGold(int value)
        {
            gold -= value;

            if (gold < 0)
            {
                gold = 0;
            }
        }
    }
}
