using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    public class WorldWeapon : MonoBehaviour
    {
        [Header("Animations")]
        public SerializedDictionary<string, AnimationClip> oneHanding;
        public SerializedDictionary<string, AnimationClip> twoHanding;

    }
}
