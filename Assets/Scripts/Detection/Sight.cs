
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace AF.Detection
{
    public class Sight : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] CharacterManager character;

        [Header("Settings")]
        [SerializeField] float detectionRadius = 15f;
        [SerializeField] float minimumDetectionAngle = -35;
        [SerializeField] float maximumDetectionAngle = 35;

        [Header("Layer Masks")]
        public LayerMask environmentBlockLayer;
        public LayerMask characterLayer;

        public void FindATargetViaLineOfSight()
        {
            if (character.targetManager.currentTarget != null)
            {
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(character.transform.position, detectionRadius, characterLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterBaseManager targetCharacter = colliders[i].transform.GetComponent<CharacterBaseManager>();

                if (targetCharacter == null)
                {
                    continue;
                }

                // Target is self? Ignore
                if (targetCharacter.transform.root == character.transform.root)
                {
                    continue;
                }

                if (targetCharacter.health.IsDead())
                {
                    continue;
                }

                if (IsFriendlyTowardsTarget(targetCharacter))
                {
                    continue;
                }

                // If a potential target is found, it has to be in front of us
                Vector3 possibleTargetDirection = targetCharacter.transform.position - character.transform.position;
                float viewableAngleOfPossibleTarget = Vector3.Angle(possibleTargetDirection, character.transform.forward);

                if (viewableAngleOfPossibleTarget > minimumDetectionAngle && viewableAngleOfPossibleTarget < maximumDetectionAngle)
                {
                    // Lastly, check for environment blocks
                    bool isObstructed = Physics.Linecast(character.lockOnReference.position, character.lockOnReference.position, environmentBlockLayer);
                    if (!isObstructed)
                    {
                        character.targetManager.SetTarget(targetCharacter);
                    }
                }
            }
        }


        public bool IsFriendlyTowardsTarget(CharacterBaseManager target)
        {
            if (character.IsFromSameFaction(target))
            {
                return true;
            }

            return false;
        }

    }
}
