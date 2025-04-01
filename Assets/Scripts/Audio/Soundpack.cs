using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(fileName = "Soundpack", menuName = "SFX/New Sound Pack", order = 0)]
    public class Soundpack : ScriptableObject
    {
        public AudioClip[] clips;
    }

}
