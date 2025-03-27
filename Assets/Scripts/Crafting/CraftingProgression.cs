using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Localization;

namespace AF
{
    [CreateAssetMenu(menuName = "Crafting / New Crafting Progression")]
    public class CraftingProgression : ScriptableObject
    {
        public int baseGold = 100;

        public SerializedDictionary<int, SerializedDictionary<UpgradeMaterial, int>> requiredItems;


    }

}
