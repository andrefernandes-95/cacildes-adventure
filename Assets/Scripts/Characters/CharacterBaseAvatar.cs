using UnityEngine;

namespace AF
{
    public class CharacterBaseAvatar : MonoBehaviour
    {
        public CharacterBaseManager characterBaseManager;

        [Header("Default Character Model")]
        public GameObject defaultCharacterModel;

        [Header("Container to spawn the avatar")]
        public Transform avatarContainerReference;

        [HideInInspector] public GameObject currentAvatarInstance;

        [Header("Debug")]
        public CharacterAvatar characterAvatarToTest;
        public bool clearCurrentAvatar = false;

        // Private
        Avatar defaultAnimatorAvatar;



        private void Awake()
        {
            defaultAnimatorAvatar = characterBaseManager.animator.avatar;
        }

        public void SetCurrentAvatar(CharacterAvatar characterAvatar)
        {

            GameObject characterPrefabInstance = Instantiate(
                characterAvatar.characterPrefab, characterBaseManager.characterBaseAvatar.avatarContainerReference);
            characterPrefabInstance.transform.localPosition = Vector3.zero;
            characterPrefabInstance.transform.localRotation = Quaternion.identity;

            this.currentAvatarInstance = characterPrefabInstance;

            // Update Weapon Pivots
            characterBaseManager.characterWeapons.AssignWeaponPivotsToAvatar(characterAvatar, currentAvatarInstance);

            // Update animator avatar
            Avatar avatar = currentAvatarInstance.GetComponent<Animator>().avatar;
            if (avatar != null)
            {
                characterBaseManager.animator.avatar = avatar;
            }

            defaultCharacterModel.SetActive(false);
        }

        public void ClearCurrentAvatar()
        {
            defaultCharacterModel.SetActive(true);
            characterBaseManager.characterWeapons.RestoreDefaultsForWeaponPivots();
            characterBaseManager.animator.avatar = defaultAnimatorAvatar;

            if (currentAvatarInstance != null)
            {
                Destroy(currentAvatarInstance);
                currentAvatarInstance = null;
            }
        }

        // TODO: Delete later, just for testing purposes
        void Update()
        {
            if (characterAvatarToTest != null && currentAvatarInstance == null)
            {
                SetCurrentAvatar(characterAvatarToTest);
                characterAvatarToTest = null;
            }


            if (clearCurrentAvatar)
            {
                clearCurrentAvatar = false;

                ClearCurrentAvatar();
            }
        }
    }
}
