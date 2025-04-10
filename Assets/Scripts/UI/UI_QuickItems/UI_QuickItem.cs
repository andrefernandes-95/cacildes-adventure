using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class UI_QuickItem : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] protected CharacterBaseManager character;

        [Header("Has Item Equipped UI")]
        [SerializeField] protected GameObject equippedItemContainer;
        [SerializeField] protected Image equippedItemIcon;

        [Header("UI Sound")]
        AudioSource audioSource => GetComponent<AudioSource>();
        [SerializeField] Soundpack uiQuickItemSoundpack;

        bool hasInitialized = false;

        protected abstract void ShowItemIcon();

        protected virtual void HideItemIcon()
        {
            equippedItemContainer.gameObject.SetActive(false);
        }

        protected virtual void PlayPopEffect()
        {
            if (hasInitialized)
            {
                UIUtils.PlayPopEffect(gameObject);
            }
            else
            {
                hasInitialized = true;
            }

            uiQuickItemSoundpack.Play(audioSource);
        }

    }
}
