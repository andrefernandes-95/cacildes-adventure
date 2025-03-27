using UnityEngine;
using Cinemachine;

namespace AF
{
    public class LockOnCameraCollision : MonoBehaviour
    {
        public Transform player; // Reference to the player's transform
        public float minCameraDistance = 1.0f; // Minimum distance the camera can be from the player
        public float maxCameraDistance = 5.0f; // Maximum distance the camera can be from the player
        public float zoomSpeed = 5.0f; // Speed at which the camera zooms in and out

        private CinemachineVirtualCamera cinemachineVirtualCamera;
        private CinemachineFramingTransposer cinemachineFramingTransposer;

        bool isZoomedIn = false;
        Vector3 lastPlayerPosition;

        public LayerMask layersToConsider;

        void Start()
        {
            cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            cinemachineFramingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        void LateUpdate()
        {
            Vector3 currentCameraPosition = Camera.main.transform.position;
            Vector3 playerPosition = player.position;

            if (isZoomedIn && !lastPlayerPosition.Equals(playerPosition))
            {
                isZoomedIn = false;
            }

            // Perform a linecast between the camera and the player
            if (Physics.Linecast(currentCameraPosition, playerPosition, out RaycastHit hit, layersToConsider))
            {
                // If the linecast hits an obstacle before reaching the player
                if (hit.transform != player)
                {
                    float desiredDistance = Mathf.Clamp(hit.distance, minCameraDistance, maxCameraDistance);
                    cinemachineFramingTransposer.m_CameraDistance = Mathf.Lerp(cinemachineFramingTransposer.m_CameraDistance, desiredDistance, Time.deltaTime * zoomSpeed * 2);

                    isZoomedIn = true;
                    lastPlayerPosition = playerPosition;
                }
                else if (!isZoomedIn)
                {
                    cinemachineFramingTransposer.m_CameraDistance = Mathf.Lerp(cinemachineFramingTransposer.m_CameraDistance, maxCameraDistance, Time.deltaTime * zoomSpeed);
                }
            }
            else if (!isZoomedIn)
            {
                // If the linecast doesn't hit anything, reset to the max distance
                cinemachineFramingTransposer.m_CameraDistance = Mathf.Lerp(cinemachineFramingTransposer.m_CameraDistance, maxCameraDistance, Time.deltaTime * zoomSpeed);
            }
        }
    }
}
