
using System;
using UnityEngine;

namespace AF
{

    public static class Formulas
    {
        [Header("Scaling Multipliers")]
        public static float E = 0;
        public static float D = .45f;
        public static float C = .85f;
        public static float B = 1.55f;
        public static float A = 2.45f;
        public static float S = 3.25f;
        public static float levelMultiplier = 1.25f;

        public static float GetScalingMultiplier(string scalingTier)
        {
            switch (scalingTier)
            {
                case "E":
                    return E;
                case "D":
                    return D;
                case "C":
                    return C;
                case "B":
                    return B;
                case "A":
                    return A;
                case "S":
                    return S;
                default:
                    break;
            }

            return 1f;
        }

        public static int CalculateStatForLevel(int baseValue, int level, float multiplier)
        {
            return (int)Mathf.Sqrt(level * multiplier) * 2 + baseValue + level;
        }

        public static int CalculateAIHealth(int baseValue, int currentLevel)
        {
            return baseValue + Mathf.RoundToInt((baseValue / 4) * Mathf.Pow(1.025f, currentLevel));
        }

        public static int CalculateAIAttack(int baseValue, int currentLevel)
        {
            return baseValue + Mathf.RoundToInt((baseValue / 2) * Mathf.Pow(1.15f, currentLevel));
        }

        public static int CalculateAIPosture(int baseValue, int currentLevel)
        {
            return baseValue + Mathf.RoundToInt((baseValue / 4) * Mathf.Pow(1.05f, currentLevel));
        }

        public static int CalculateAIGenericValue(int baseValue, int currentLevel)
        {
            return baseValue + Mathf.RoundToInt((baseValue / 2) * Mathf.Pow(1.05f, currentLevel));
        }

        public static int CalculateSpellValue(int baseValue, int currentIntelligence)
        {
            if (currentIntelligence <= 1)
            {
                currentIntelligence = 0;
            }

            return baseValue + Mathf.RoundToInt((baseValue / 4) * Mathf.Pow(1.04f, currentIntelligence));
        }

        public static int GetBonusFromWeapon(int currentStatLevel, string weaponScaling)
        {
            float scalingMultiplier = GetScalingMultiplier(weaponScaling);

            if (scalingMultiplier <= 0)
            {
                return 0;
            }

            // Each x levels, increase the multiplying scale to keep the function growth adjusted to later levels
            int levelInterval = 5 + (currentStatLevel / 5);

            return (int)LogarithmicFunctionValue(currentStatLevel, levelInterval * scalingMultiplier / 1.5f);
        }

        public static float LogarithmicFunctionValue(float x, float scaleFactor = 10f)
        {
            float horizontalOffset = .5f; // Offset the function so it starts giving higher values after the initial levels

            // Use Math.Log to calculate natural log in C#
            float result = scaleFactor * Mathf.Log(x - horizontalOffset);
            return Mathf.Max(1, result <= 0 ? 1 : result);
        }

        public static int GetBonusFromWeapon(int currentStatLevel, float v)
        {
            throw new NotImplementedException();
        }
    }

}