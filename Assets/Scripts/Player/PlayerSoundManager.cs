using UnityEngine;

namespace AF
{
    public class PlayerSoundManager : MonoBehaviour
    {
        public PlayerManager playerManager;

        public AudioSource audioSource;

        [Header("Rage Sounds")]
        public AudioClip maleRageSound;
        public AudioClip femaleRageSound;

        /// <summary>
        /// Unity Event
        /// </summary>
        public void PlayRageSound()
        {
            if (playerManager.playerAppearance.GetBodyType() > 0)
            {
                audioSource.PlayOneShot(femaleRageSound);
            }
            else
            {
                audioSource.PlayOneShot(maleRageSound);
            }
        }
    }
}
