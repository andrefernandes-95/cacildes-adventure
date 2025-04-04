namespace AF
{
    using UnityEngine;

    public class AttachToCharacterModel : MonoBehaviour
    {
        [Header("Assign Via Name")]
        [SerializeField] string boneName = "";

        [Header("Or Assign Via Direct Reference")]
        [SerializeField] Transform boneToAssignAsParent;

        [Header("Position & Rotation")]
        [SerializeField] Vector3 localPositionWhenParenting;
        [SerializeField] Vector3 localRotationWhenParenting;

        void Awake()
        {
            if (boneToAssignAsParent != null)
            {
                transform.parent = boneToAssignAsParent;
            }
            else if (!string.IsNullOrEmpty(boneName))
            {
                CharacterBaseManager character = GetComponentInParent<CharacterBaseManager>();
                if (character != null)
                {
                    Transform foundBone = Utils.FindChildByName(character.transform, boneName);
                    if (foundBone != null)
                    {
                        transform.parent = foundBone;
                    }
                    else
                    {
                        Debug.LogWarning($"Bone '{boneName}' not found in character model.");
                    }
                }
                else
                {
                    Debug.LogError("CharacterManager not found in parent hierarchy.");
                }
            }

            // Set local position & rotation after parenting
            transform.localPosition = localPositionWhenParenting;
            transform.localRotation = Quaternion.Euler(localRotationWhenParenting);
        }
    }
}
