using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AF
{

    [CreateAssetMenu(fileName = "Player Customization", menuName = "System/New Player Customization", order = 0)]
    public class PlayerCustomization : ScriptableObject
    {

        [Header("Options")]
        public bool isMale = true;

        [Header("Common Parts")]
        public List<string> hairs = new();
        public List<string> eyebrows = new();
        public List<string> beards = new();

        [Header("Male Parts")]
        public List<string> maleHead = new();

        public List<string> maleTorso = new();
        public List<string> maleHands = new();
        public List<string> maleLegs = new();

        [Header("Female Parts")]
        public List<string> femaleHead = new();

        public List<string> femaleTorso = new();
        public List<string> femaleHands = new();
        public List<string> femaleLegs = new();

        [Header("Default Values")]
        public List<string> defaultHairs = new();
        public List<string> defaultEyebrows = new();
        public List<string> defaultBeards = new();
        public List<string> defaulMaleHead = new();
        public List<string> defaultFemalehead = new();

        [Header("Colors")]
        public Color hairColor;
        public Color eyeColor;
        public Color skinColor;
        public Color scarColor;

        [Header("Defaults")]
        public Color defaultHairColor;
        public Color defaultEyeColor;
        public Color defaultSkinColor;
        public Color defaultScarColor;

#if UNITY_EDITOR
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                Clear();
            }
        }
#endif

        void Clear()
        {
            maleHead = defaulMaleHead;
            femaleHead = defaultFemalehead;

            hairs = defaultHairs;
            eyebrows = defaultEyebrows;
            beards = defaultBeards;

            hairColor = defaultHairColor;
            eyeColor = defaultEyeColor;
            skinColor = defaultSkinColor;
            scarColor = defaultScarColor;
        }

    }

}