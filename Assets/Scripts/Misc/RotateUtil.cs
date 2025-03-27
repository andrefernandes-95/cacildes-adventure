using UnityEngine;

namespace AF
{
    public class RotateUtil : MonoBehaviour
    {
        // Rotation speed
        public float rotationSpeed = 100f;

        // Flags to choose the axis of rotation
        public bool rotateAroundX = false;
        public bool rotateAroundY = false;
        public bool rotateAroundZ = false;

        public bool shouldStop = false;

        public void SetShouldStop(bool value)
        {
            this.shouldStop = value;
        }

        void Update()
        {
            if (shouldStop)
            {
                return;
            }

            Rotate();
        }

        private void Rotate()
        {
            float xRotation = rotateAroundX ? rotationSpeed * Time.deltaTime : 0f;
            float yRotation = rotateAroundY ? rotationSpeed * Time.deltaTime : 0f;
            float zRotation = rotateAroundZ ? rotationSpeed * Time.deltaTime : 0f;

            transform.Rotate(xRotation, yRotation, zRotation);
        }
    }
}
