using System;
using System.Linq;
using AF.Events;
using GameAnalyticsSDK;
using NaughtyAttributes;
using TigerForge;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace AF
{
    [CreateAssetMenu(menuName = "Quests / New Quest Objective")]

    public class QuestObjective : ScriptableObject
    {
        [Header("Quest")]
        public QuestParent quest;

        [ShowAssetPreview]
        public Sprite objectiveImage;

        public Location location;

        [Header("Label")]
        [TextArea(minLines: 3, maxLines: 9)] public string enObjective;
        [TextArea(minLines: 3, maxLines: 9)] public string ptObjective;

    }
}
