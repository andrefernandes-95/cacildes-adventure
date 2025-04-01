using UnityEngine;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class CharacterSoundManager : MonoBehaviour
    {
        AudioSource audioSource => GetComponent<AudioSource>();

        [Header("Movement")]
        [SerializeField] protected Soundpack dodgeRolls;

        [Header("Damage Grunts")]
        [SerializeField] protected Soundpack damageGrunts;
        [SerializeField] protected Soundpack attackingGrunts;

        [Header("Effects")]
        [SerializeField] protected Soundpack physicalDamage;

        AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public void PlaySoundpack(Soundpack soundpack)
        {
            if (soundpack == null)
            {
                return;
            }

            PlaySoundFX(ChooseRandomSFXFromArray(soundpack.clips));
        }

        void PlaySoundFX(AudioClip audioClip, float volume = 1f, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.pitch = 1;
            audioSource.PlayOneShot(audioClip, volume);

            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }

        public void PlayRoll()
        {
            if (dodgeRolls == null || dodgeRolls.clips.Length <= 0)
            {
                return;
            }

            PlaySoundFX(ChooseRandomSFXFromArray(dodgeRolls.clips));
        }

        public void PlayPhysicalDamage()
        {
            if (physicalDamage == null || physicalDamage.clips.Length <= 0)
            {
                return;
            }

            PlaySoundFX(ChooseRandomSFXFromArray(physicalDamage.clips));
        }

        public void PlayDamageGrunt()
        {
            if (damageGrunts == null || damageGrunts.clips.Length <= 0)
            {
                return;
            }

            PlaySoundFX(ChooseRandomSFXFromArray(damageGrunts.clips));
        }

        public void PlayAttackGrunt()
        {
            if (attackingGrunts == null || attackingGrunts.clips.Length <= 0)
            {
                return;
            }

            PlaySoundFX(ChooseRandomSFXFromArray(attackingGrunts.clips));
        }

    }
}
