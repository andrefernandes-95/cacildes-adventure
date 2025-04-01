using UnityEngine;

namespace AF
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        [SerializeField] CharacterBaseManager characterBaseManager;

        [SerializeField] VisualEffectsDatabase visualEfffectsDatabase;

        [Header("Effects Database")]
        public CharacterEffectsDatabase characterEffectsDatabase;

        public void ProcessInstantEffect(InstantCharacterEffect instantCharacterEffect)
        {
            instantCharacterEffect.ProcessEffect(characterBaseManager);
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            Instantiate(visualEfffectsDatabase.bloodSplatter, contactPoint, Quaternion.identity);
        }
    }
}
