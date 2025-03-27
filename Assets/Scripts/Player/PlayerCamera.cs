namespace AF
{
    using Cinemachine;
    using UnityEngine;

    public class PlayerCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera cinemachineVirtualCamera;
        public GameSettings gameSettings;

        void Start()
        {
            cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

            UpdateCameraDistance();
        }

        public void UpdateCameraDistance()
        {
            cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = gameSettings.GetCameraDistance();
        }
    }

}
