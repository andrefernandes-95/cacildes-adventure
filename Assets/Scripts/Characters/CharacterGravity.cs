using UnityEngine;

namespace AF
{
    public class CharacterGravity : MonoBehaviour
    {
        [Header("Components")]
        public CharacterBaseManager characterBaseManager;

        [Tooltip("The height the character can jump")]
        public float JumpHeight = 1.2f;
        public float JumpHeightBonus = 0f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Character Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool isGrounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        private float _verticalVelocity;
        public float VerticalVelocity
        {
            get { return _verticalVelocity; }
        }

        private float _terminalVelocity = 53.0f;

        float fallBegin;

        void Update()
        {
            GroundedCheck();

            ApplyGravity();
        }

        private void GroundedCheck()
        {
            isGrounded = Physics.CheckSphere(
                new(
                    characterBaseManager.transform.position.x,
                    characterBaseManager.characterController.transform.position.y - GroundedOffset,
                    characterBaseManager.characterController.transform.position.z),
                GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore) || characterBaseManager.characterController.isGrounded;
        }

        void ApplyGravity()
        {
            if (isGrounded)
            {
                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime; //+ (playerManager.playerCombatController.isJumpAttacking ? jumpAttackVelocity : 0f);
            }

            EnforceVerticalVelocity();
        }

        void EnforceVerticalVelocity()
        {
            if (characterBaseManager.characterController.enabled && !characterBaseManager.agent.enabled)
            {
                characterBaseManager.characterController.Move(new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);
            }
        }

        public void Jump(float jumpHeightBonus = 0)
        {
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            _verticalVelocity = Mathf.Sqrt((JumpHeight + jumpHeightBonus) * -2f * Gravity);
        }

        public void UpdateFallBegin() => fallBegin = characterBaseManager.transform.position.y;

        public float GetFallHeight()
        {
            float difference = fallBegin - characterBaseManager.transform.position.y;
            return difference;
        }

        public float GetHeightFromGround()
        {
            // Define a ray starting from the player's position pointing downwards
            Ray ray = new Ray(characterBaseManager.transform.position, Vector3.down);

            // Perform the raycast to detect the ground
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, GroundLayers, QueryTriggerInteraction.Ignore))
            {
                // Return the distance from the player to the ground
                return hit.distance;
            }

            // If no ground is detected, return a large value or handle it as needed
            return Mathf.Infinity;
        }
    }
}
