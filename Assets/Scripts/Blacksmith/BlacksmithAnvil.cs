namespace AF
{
    using UnityEngine;

    public class BlacksmithAnvil : MonoBehaviour
    {
        public Transform characterPositionReference;

        public GameObject anvilSwordBeingWorkedOn;

        public ParticleSystem anvilParticles;

        private void Awake()
        {
            HideAnvilSword();
        }

        public void ShowAnvilSword()
        {
            if (anvilSwordBeingWorkedOn != null)
            {
                anvilSwordBeingWorkedOn.SetActive(true);
            }
        }

        public void HideAnvilSword()
        {
            if (anvilSwordBeingWorkedOn != null)
            {
                anvilSwordBeingWorkedOn.SetActive(false);
            }
        }

        public void FaceAnvil(CharacterBaseManager character)
        {
            character.TeleportToPosition(characterPositionReference.position);

            Vector3 direction = transform.position - character.transform.position;
            direction.y = 0f; // Ignore the y-axis for rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            character.transform.rotation = targetRotation;
        }

        public void PlayAnvilParticles()
        {
            if (anvilParticles != null)
            {
                anvilParticles.Play();
            }
        }
    }
}
