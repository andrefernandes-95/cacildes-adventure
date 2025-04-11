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

        protected abstract void ShowItemIcon();

        protected virtual void HideItemIcon()
        {
            equippedItemContainer.gameObject.SetActive(false);
        }

        protected virtual void PlayPopEffect()
        {
            UIUtils.PlayPopEffect(gameObject);

            uiQuickItemSoundpack.Play(audioSource);
        }
    }
}
