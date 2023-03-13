using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;

public class OptimizedDesktopController : MonoBehaviour
{
    [SerializeField] private PlayerOptionsSO playerOptions;
    [SerializeField] private Camera playerCamera;

    private Quaternion targetRotation;
    private PlayerInput playerInput;
    private float xRotation = 0;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        targetRotation = transform.rotation;
        playerCamera = playerCamera == null ? Camera.main : playerCamera;
    }

    private void Update()
    {
        Vector2 moveInput = Vector2.zero;
        Vector2 viewInput = Vector2.zero;

        if (playerOptions.movePlayer)
        {
            moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
            float runningMultiplier = playerInput.actions["Run"].ReadValue<float>() == 1 ? playerOptions.speedRunningMultiplier : 1;
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
            viewInput = playerInput.actions["Look"].ReadValue<Vector2>() * playerOptions.lookSensitivity;

            xRotation -= viewInput.y;
            xRotation = Mathf.Clamp(xRotation, playerOptions.xMin, playerOptions.xMax);
            targetRotation = Quaternion.Euler(xRotation, targetRotation.eulerAngles.y + viewInput.x, 0);

            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            playerCamera.transform.localRotation = Quaternion.Euler(targetRotation.eulerAngles.x, 0, targetRotation.eulerAngles.z);
        }

        if (playerOptions.flyPlayer)
        {
            float flyDirection = playerInput.actions["Fly"].ReadValue<float>();
            Vector3 flyVertical = new Vector3(0, flyDirection, 0);
            Vector3 velocity = flyVertical * playerOptions.flySpeed;
            transform.position += velocity * Time.deltaTime;
        }
    }
}
