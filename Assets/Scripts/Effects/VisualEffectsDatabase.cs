using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(fileName = "Visual Effects Database", menuName = "System/New Visual Effects Database", order = 0)]
    public class VisualEffectsDatabase : ScriptableObject
    {
        public GameObject bloodSplatter;
    }

}
