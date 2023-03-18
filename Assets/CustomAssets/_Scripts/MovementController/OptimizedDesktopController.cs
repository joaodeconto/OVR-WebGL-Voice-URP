using BWV.Interface;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;

namespace BWV.Player
{
    public class OptimizedDesktopController : MonoBehaviour, ICharacterInput
    {
        [SerializeField] private PlayerOptionsSO playerOptions;
        [SerializeField] private Camera playerCamera;

        private Quaternion targetRotation;
        private Rigidbody playerRigidbody;
        private PlayerInput playerInput;
        private float xRotation = 0;

        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int Strafe = Animator.StringToHash("Strafe");

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            targetRotation = transform.rotation;
            playerCamera = playerCamera == null ? Camera.main : playerCamera;
        }

        private void Update()
        {
            Vector2 moveInput = GetMoveInput();
            Vector2 viewInput = GetViewInput();

            playerRigidbody.useGravity = !playerOptions.gravityOn;

            if (playerOptions.movePlayer)
            {
                float runningMultiplier = GetRunMultiplier();
                float currentSpeed = playerOptions.speed * runningMultiplier;

                Vector3 moveHorizontal = transform.right * moveInput.x;
                Vector3 moveVertical;

                if (playerOptions.flyForward)
                {
                    moveVertical = playerCamera.transform.forward * moveInput.y;
                }
                else
                {
                    moveVertical = transform.forward * moveInput.y;
                }
                Vector3 velocity = (moveHorizontal + moveVertical).normalized * currentSpeed;
                transform.position += velocity * Time.deltaTime;
            }

            if (playerOptions.moveViewX || playerOptions.moveViewY)
            {
                // Calculate the target rotation based on input
                xRotation -= viewInput.y;
                xRotation = Mathf.Clamp(xRotation, playerOptions.xMin, playerOptions.xMax);
                targetRotation = Quaternion.Euler(xRotation, targetRotation.eulerAngles.y + viewInput.x, 0);

                // Smoothly rotate towards the target rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation.eulerAngles.y, 0), Time.deltaTime * playerOptions.smoothLookFactor);
                playerCamera.transform.localRotation = Quaternion.Lerp(playerCamera.transform.localRotation, Quaternion.Euler(targetRotation.eulerAngles.x, 0, targetRotation.eulerAngles.z), Time.deltaTime * playerOptions.smoothLookFactor);
            }

            if (playerOptions.flyPlayer)
            {
                float flyDirection = GetFlyDirection();
                Vector3 flyVertical = new Vector3(0, flyDirection, 0);
                Vector3 velocity = flyVertical * playerOptions.flySpeed;
                transform.position += velocity * Time.deltaTime;
            }
            if(transform.position.y < -100) RestorePlayerPosition();
        }

        void RestorePlayerPosition()
        {
            transform.position = new Vector3(0, 2, 0);
        }

        public Vector2 GetMoveInput()
        {
            return playerInput.actions["Move"].ReadValue<Vector2>();
        }

        public float GetRunMultiplier()
        {
            return playerInput.actions["Run"].ReadValue<float>() == 1 ? playerOptions.speedRunningMultiplier : 1;
        }

        public Vector2 GetViewInput()
        {
            return playerInput.actions["Look"].ReadValue<Vector2>() * playerOptions.lookSensitivity;
        }

        public float GetFlyDirection()
        {
            return playerInput.actions["Fly"].ReadValue<float>();
        }

        public bool GetFire()
        {
            return Input.GetButtonDown("Fire1");
        }
    }
}

