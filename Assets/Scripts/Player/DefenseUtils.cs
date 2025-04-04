using UnityEngine;

namespace AF
{
    public static class DefenseUtils
    {
        /// <summary>
        /// // Endurance gives +1 defense per 2 points
        /// </summary>
        /// <param name="endurance"></param>
        /// <returns></returns>
        public static int GetPhysicalDefenseFromEndurance(int endurance)
        {
            int defense = 0;

            defense += (int)Mathf.Floor(endurance / 2);

            return defense;
        }

        /// <summary>
        /// // Vitality gives +1 defense per 4 points
        /// </summary>
        /// <param name="endurance"></param>
        /// <returns></returns>
        public static int GetPhysicalDefenseFromVitaly(int vitality)
        {
            int defense = 0;

            defense += (int)Mathf.Floor(vitality / 4);

            return defense;
        }

        /// <summary>
        /// // Strength gives +1 defense per 1 point
        /// </summary>
        /// <param name="endurance"></param>
        /// <returns></returns>
        public static int GetPhysicalDefenseFromStrength(int strength)
        {
            int defense = 0;

            defense += (int)Mathf.Floor(strength);

            return defense;
        }

        /// <summary>
        /// // Intelligence gives +1 elemental defense per 1 point
        /// </summary>
        /// <param name="endurance"></param>
        /// <returns></returns>
        public static int GetElementalDefenseFromIntelligence(int intelligence)
        {
            int defense = 0;

            defense += (int)Mathf.Floor(intelligence);

            return defense;
        }

        /// <summary>
        /// // Reputation gives +1 holy / darkness defense per 1 point
        /// </summary>
        /// <param name="endurance"></param>
        /// <returns></returns>
        public static int GetElementalDefenseFromReputation(int reputation)
        {
            int defense = 0;

            defense += (int)Mathf.Floor(reputation);

            return defense;
        }

        public static (bool, bool, bool) CompareDamageNegation(int current, int next)
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
    }
}
