using UnityEngine;
using UnityEngine.InputSystem;

public class AdvancedDesktopController : MonoBehaviour
{
    [Header("Movement")]
    public bool movePlayer = true;
    public  bool moveX = true;
    public  bool moveY = true;
    public float speed = 2.0f;
    public float speedRunningMultiplier = 2f;

    [Header ("Fly")]
    public bool flyPlayer = true;
    public bool flyForward = true;
    public float flySpeed = 2.0f;

    [Header("Sensitivity")]
    public bool moveViewX = true;
    public bool moveViewY = true;
    public float lookSensitivity = .5f;

    [Header("View Rotation Clamp")]
    public float xMax = 320f;
    public float xMin = 60f;

    [Header("Camera")]
    public Camera playerCamera;

    private float xRotation = 0; // set the initial value of xRotation to 0
    private Vector2 moveInput;
    private Vector2 viewInput;
    private float currentSpeed;
    private Quaternion targetRotation;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        targetRotation = transform.rotation;
        currentSpeed = speed;
        playerCamera = playerCamera == null ? GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() : playerCamera;
        
    }

    private void Update()
    {
        if (movePlayer)
        { 
            moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
            float runningMultiplier = playerInput.actions["Run"].ReadValue<float>() == 1 ? speedRunningMultiplier : 1;
            currentSpeed = speed * runningMultiplier;

            Vector3 moveHorizontal = transform.right * moveInput.x;
            Vector3 moveVertical;

            if (flyForward)
            {
                moveVertical = playerCamera.transform.forward * moveInput.y;
            }
            else
                moveVertical = transform.forward * moveInput.y;
            

            Vector3 velocity = (moveHorizontal + moveVertical).normalized * currentSpeed;
            transform.position += velocity * Time.deltaTime;
        }

        if (moveViewX || moveViewY)
        {
            viewInput = playerInput.actions["Look"].ReadValue<Vector2>() * lookSensitivity;

            xRotation -= viewInput.y; // subtract viewInput.y from xRotation to invert the controls
            xRotation = Mathf.Clamp(xRotation, xMin, xMax);
            targetRotation = Quaternion.Euler(xRotation, targetRotation.eulerAngles.y + viewInput.x, 0);

            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            playerCamera.transform.localRotation = Quaternion.Euler(targetRotation.eulerAngles.x, 0, targetRotation.eulerAngles.z);
        }

        if (flyPlayer)
        {
            float flyDirection = playerInput.actions["Fly"].ReadValue<float>();
            Vector3 flyVertical = new(0,flyDirection,0);
            Vector3 velocity = flyVertical * flySpeed;
            transform.position += velocity * Time.deltaTime;
        }
    }
}