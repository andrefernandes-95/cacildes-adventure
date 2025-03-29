using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    /// <summary>
    /// ScriptableObject database that manages character assets and provides efficient lookup functionality.
    /// Allows characters to be registered in the editor and retrieved by their name identifier at runtime.
    /// </summary>
    [CreateAssetMenu(fileName = "Actors Database", menuName = "System/New Actors Database", order = 0)]
    public class ActorsDatabase : ScriptableObject
    {
        SerializedDictionary<string, Character> actors = new();

        public Character GetCharacterById(string characterId)
        {
            if (!actors.ContainsKey(characterId))
            {
                Debug.LogWarning($"Requested character not found: {characterId}");
                return null;
            }

            return actors[characterId];
        }
    }
}