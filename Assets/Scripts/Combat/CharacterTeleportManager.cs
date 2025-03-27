using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace AF
{
    public class CharacterTeleportManager : MonoBehaviour
    {
        [Header("Components")]
        public CharacterManager characterManager;

        [Header("Teleport Options")]
        public float minimumTeleportRadiusFromTarget = 5f;
        public float maximumTeleportRadiusFromTarget = 10f;
        public bool teleportNearPlayer = false;

        PlayerManager _playerManager;

        public UnityEvent onTeleport;

        [Header("Layer Mask")]
        public string areaName = "";

        /// <summary>
        /// UnityEvent
        /// </summary>
        public void TeleportEnemy()
        {
            Vector3 randomPoint = teleportNearPlayer
                ? Camera.main.transform.position + Camera.main.transform.forward * -2f
                : RandomNavmeshPoint(GetPlayerManager().transform.position, maximumTeleportRadiusFromTarget, minimumTeleportRadiusFromTarget);

            characterManager.Teleport(randomPoint);

            Vector3 lookRot = randomPoint - characterManager.transform.position;
            lookRot.y = 0;
            characterManager.transform.rotation = Quaternion.LookRotation(lookRot);

            onTeleport?.Invoke();
        }

        Vector3 RandomNavmeshPoint(Vector3 center, float radius, float minDistance)
        {
            for (int i = 0; i < 10; i++) // You can adjust the number of attempts
            {
                Vector3 randomDirection = Random.insideUnitSphere * radius;
                randomDirection += center;

                if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, radius, string.IsNullOrEmpty(areaName) ? -1 : NavMesh.GetAreaFromName(areaName)) && Vector3.Distance(navHit.position, center) >= minDistance)
                {
                    return new Vector3(navHit.position.x, GetPlayerManager().transform.position.y, navHit.position.z);
                }
            }

            Debug.LogWarning("Failed to find a valid teleportation position after multiple attempts.");
            return GetPlayerManager().transform.position + GetPlayerManager().transform.forward * -1; // Return zero if no valid position is found after attempts
        }

        PlayerManager GetPlayerManager()
        {
            if (_playerManager == null) { _playerManager = FindAnyObjectByType<PlayerManager>(FindObjectsInactive.Include); }
            return _playerManager;
        }

    }
}
