using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class GenericTrigger : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Canvas uiActivationPopup;

        [Header("Events")]
        [SerializeField] private UnityEvent onActivate;
        StarterAssetsInputs starterAssetsInputs;
        bool hasInteracted = false;

        [Header("Sound")]
        public AudioClip uiAppearSound;
        public AudioClip uiDisappearSound;
        public AudioClip uiActivateSound;

        AudioSource audioSource => GetComponent<AudioSource>();

        float popDuration = 0.25f;
        float popScaleMultiplier = 1.2f;

        private void Awake()
        {
            audioSource.spatialBlend = 1f;
            audioSource.playOnAwake = false;
        }

        private void Start()
        {
            HideUiActivationPopup(false);
        }

        private void OnEnable()
        {
            if (starterAssetsInputs == null)
            {
                starterAssetsInputs = FindAnyObjectByType<StarterAssetsInputs>(FindObjectsInactive.Include);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (hasInteracted)
            {
                return;
            }

            if (other.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
            {
                ShowUiActivationPopup();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (hasInteracted)
            {
                return;
            }

            if (other.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
            {
                if (starterAssetsInputs.interact)
                {
                    starterAssetsInputs.interact = false; // Reset the input to prevent multiple activations
                    onActivate?.Invoke(); // Invoke the event
                    HideUiActivationPopup(false); // Hide the UI after activation
                    hasInteracted = true;

                    if (uiActivateSound != null)
                    {
                        audioSource.PlayOneShot(uiActivateSound);
                    }

                    OnActivate(playerManager);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
            {
                if (!hasInteracted)
                {
                    HideUiActivationPopup();
                }

                hasInteracted = false;
            }
        }

        void ShowUiActivationPopup()
        {
            if (uiActivationPopup != null)
            {
                uiActivationPopup.gameObject.SetActive(true);
                PlayPopEffect(uiActivationPopup.gameObject, true);

                if (uiAppearSound != null)
                {
                    audioSource.PlayOneShot(uiAppearSound);
                }
            }
        }

        void HideUiActivationPopup(bool playSound = true)
        {
            if (uiActivationPopup != null)
            {
                PlayPopEffect(uiActivationPopup.gameObject, false);

                if (playSound)
                {
                    audioSource.PlayOneShot(uiDisappearSound);
                }
            }
        }

        public void PlayPopEffect(GameObject target, bool activate)
        {
            if (target == null) return;

            RectTransform rectTransform = target.GetComponent<RectTransform>();
            if (rectTransform == null) return;

            Vector3 originalScale = rectTransform.localScale;

            rectTransform.DOKill(); // Kill any active tweens

            // Create pop animation sequence
            Sequence popSeq = DOTween.Sequence();
            popSeq.Append(rectTransform.DOScale(originalScale * popScaleMultiplier, popDuration * 0.4f).SetEase(Ease.OutBack));
            popSeq.Append(rectTransform.DOScale(originalScale, popDuration * 0.6f).SetEase(Ease.OutQuad));

            // Add callback to deactivate object after animation
            popSeq.OnComplete(() => target.SetActive(activate));
        }

        protected virtual void OnActivate(CharacterBaseManager character)
        {

        }
    }
}
