using TMPro;
using UnityEngine;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class UI_SwitchCombatModeHUD : MonoBehaviour
    {
        [Header("UI Parts")]
        [SerializeField] TextMeshProUGUI active;
        [SerializeField] TextMeshProUGUI inactive;

        [Header("Character")]
        [SerializeField] CharacterBaseManager characterBaseManager;

        [Header("Sounds")]
        [SerializeField] Soundpack uiButtonPressSoundpack;
        [SerializeField] Soundpack equippingSoundpack;

        AudioSource audioSource => GetComponent<AudioSource>();

        private void Awake()
        {
            characterBaseManager.characterBaseTwoHandingManager.onTwoHandingModeChanged.AddListener(UpdateTwoHandIndicator);
        }

        void UpdateTwoHandIndicator()
        {
            if (characterBaseManager.characterBaseTwoHandingManager.isTwoHanding)
            {
                active.text = GetTwoHandLabel();
                inactive.text = GetOneHandLabel();
            }
            else
            {
                active.text = GetOneHandLabel();
                inactive.text = GetTwoHandLabel();
            }

            UIUtils.PlayPopEffect(this.gameObject);

            uiButtonPressSoundpack.Play(audioSource);
            equippingSoundpack.Play(audioSource);
        }

        string GetOneHandLabel() => Glossary.IsPortuguese() ? "Empunho de uma mão" : "One handing weapon";
        string GetTwoHandLabel() => Glossary.IsPortuguese() ? "Empunho de duas mãos" : "Two handing weapon";

    }

}
