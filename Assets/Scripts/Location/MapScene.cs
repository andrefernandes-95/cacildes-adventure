using System;
using System.Linq;
using AF.Events;
using AYellowpaper.SerializedCollections;
using GameAnalyticsSDK;
using NaughtyAttributes;
using TigerForge;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace AF
{
    [CreateAssetMenu(menuName = "Data / New Location")]

    public class Location : ScriptableObject
    {
        [InfoBox("This file name must match the scene's file name")]

        [TextArea(minLines: 3, maxLines: 9)] public string englishSceneName;
        [TextArea(minLines: 3, maxLines: 9)] public string portugueseSceneName;

        [Header("Teleports")]
        public SerializedDictionary<string, Location> teleportLocations = new SerializedDictionary<string, Location>();
    }
}
