using UnityEngine;
using UnityEngine.Events;

namespace AF
{

    public class CharacterBaseTwoHandingManager : MonoBehaviour
    {
        [SerializeField] CharacterBaseManager character;
        public bool isTwoHanding = false;

        [Header("Transform References")]
        public Transform backHolster;

        GameObject backWeapon;

        public UnityEvent onTwoHandingModeChanged;

        public void ToggleTwoHandingMode()
        {
            if (isTwoHanding)
            {
                DisableTwoHanding();
            }
            else
            {
                EnableTwoHanding();
            }
        }

        public void EnableTwoHanding()
        {
            isTwoHanding = true;
            character.animator.SetBool(AnimatorParametersConstants.IsTwoHanding, true);

            OnTwoHandingChanged();
        }

        public void DisableTwoHanding()
        {
            isTwoHanding = false;
            character.animator.SetBool(AnimatorParametersConstants.IsTwoHanding, false);

            OnTwoHandingChanged();
        }

        public void EvaluateCurrentState()
        {

            if (isTwoHanding)
            {
                character.characterWeapons.equippedLeftWeaponInstance.gameObject.SetActive(false);

                if (character.characterWeapons.equippedRightWeaponInstance is MeleeWorldWeapon meleeWorldWeapon)
                {
                    meleeWorldWeapon.ActivateTwoHandPivots();
                }

                PutLeftWeaponInBack();
            }
            else
            {
                if (character.characterWeapons.equippedRightWeaponInstance is MeleeWorldWeapon meleeWorldWeapon)
                {
                    meleeWorldWeapon.RestoreDefaultPivots();
                }

                character.characterWeapons.equippedLeftWeaponInstance.gameObject.SetActive(true);

                RemoveWeaponsFromBack();
            }
        }

        public void OnTwoHandingChanged()
        {
            EvaluateCurrentState();

            onTwoHandingModeChanged?.Invoke();
        }


        void PutLeftWeaponInBack()
        {
            // Always make sure to delete any previous equipped items in the back so we don't end up with multiple instances
            RemoveWeaponsFromBack();

            if (character.characterWeapons.equippedLeftWeaponInstance == null)
            {
                return;
            }

            backWeapon = Instantiate(character.characterWeapons.equippedLeftWeaponInstance.gameObject, backHolster);
            backWeapon.transform.localRotation = Quaternion.identity;
            backWeapon.transform.localPosition = Vector3.zero;
            backWeapon.gameObject.SetActive(true);

            if (backWeapon.TryGetComponent(out MeleeWorldWeapon meleeWorldWeapon))
            {
                meleeWorldWeapon.ActivatBackPivots();
            }
        }

        void RemoveWeaponsFromBack()
        {
            if (backWeapon != null)
            {
                Destroy(backWeapon);
                backWeapon = null;
            }
        }
    }
}
