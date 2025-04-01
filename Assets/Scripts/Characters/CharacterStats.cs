namespace AF
{
    using UnityEngine;

    public class CharacterStats : CharacterBaseStats
    {
        [Header("Stats")]
        public int vitality = 1;
        public int endurance = 1;
        public int intelligence = 1;
        public int strength = 1;
        public int dexterity = 1;

        [Header("Other Stats")]
        public int reputation = 1;

        public override int GetDexterity()
        {
            return dexterity;
        }

        public override int GetEndurance()
        {
            return endurance;
        }

        public override int GetIntelligence()
        {
            return intelligence;
        }

        public override int GetStrength()
        {
            return strength;
        }

        public override int GetVitality()
        {
            return vitality;
        }

        public override int GetReputation()
        {
            return reputation;
        }

        public override int GetCurrentLevel()
        {
            return vitality + endurance + strength + dexterity + intelligence;
        }

        public override void ResetStats()
        {
            this.vitality = this.endurance = this.intelligence = this.strength = this.intelligence = 1;
        }
    }
}
