using UnityEngine;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySoundpackOnEnable : MonoBehaviour
    {
        AudioSource audioSource => GetComponent<AudioSource>();

        [SerializeField] Soundpack soundpack;

        void OnEnable()
        {
            if (soundpack != null)
            {
                soundpack.Play(audioSource);
            }
        }
    }
}
