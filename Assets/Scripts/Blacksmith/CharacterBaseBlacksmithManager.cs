namespace AF
{
    using AF.StateMachine;
    using UnityEngine;

    public class CharacterBaseBlacksmithManager : MonoBehaviour
    {
        public BlacksmithAnvil currentAnvil;
        public BlacksmithHammer blacksmithHammer;
        public CharacterBaseManager character;
        public bool isWorking = false;

        [Header("Sounds")]
        public Soundpack hammeringSounds;

        [Header("States")]
        public BlacksmithAnvilState blacksmithAnvilState;

        private void Awake()
        {
            blacksmithAnvilState = Instantiate(blacksmithAnvilState);
        }

        private void Start()
        {
            blacksmithHammer.gameObject.SetActive(false);
        }

        public virtual void BeginJob(BlacksmithAnvil anvil)
        {
            currentAnvil = anvil;

            character.canMove = false;
            character.canRotate = false;

            currentAnvil.FaceAnvil(character);
            currentAnvil.ShowAnvilSword();

            character.characterWeapons.HideEquipment();
            blacksmithHammer.gameObject.SetActive(true);
            isWorking = true;
            character.SetAnimatorBool(AnimatorParametersConstants.IsUsingAnvil, true);
            character.PlayBusyAnimationWithRootMotion(AnimatorClipNames.UsingAnvil);
            character.characterBaseStateMachine.ChangeToState(blacksmithAnvilState);
        }

        public virtual void EndJob()
        {
            isWorking = false;
            character.characterWeapons.ShowEquipment();
            blacksmithHammer.gameObject.SetActive(false);
            character.SetAnimatorBool(AnimatorParametersConstants.IsUsingAnvil, false);
            currentAnvil.HideAnvilSword();

            character.canMove = true;
            character.canRotate = true;
        }

        public void PlayHammer()
        {
            if (hammeringSounds != null)
            {
                hammeringSounds.Play(character);
            }

            currentAnvil.PlayAnvilParticles();
        }
    }
}
