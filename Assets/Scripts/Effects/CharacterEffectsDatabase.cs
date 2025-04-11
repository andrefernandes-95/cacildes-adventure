using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(fileName = "Character Effects Database", menuName = "System/New Character Effects Database", order = 0)]
    public class CharacterEffectsDatabase : ScriptableObject
    {
        [Header("Instant Effects")]
        public TakeDamageEffect takeDamageEffect;
        public TakeBlockedDamageEffect takeBlockedDamageEffect;
    }

}
