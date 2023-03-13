using UnityEngine;

public class DesktopController : MonoBehaviour
{
    #region Public Fields

    #endregion

    #region Private Fields

    private float m_MovX;
    private float m_MovY;
    private Vector3 m_moveHorizontal;
    private Vector3 m_movVertical;
    private Vector3 m_velocity;
    private Rigidbody m_Rigid;
    private CapsuleCollider m_CapsuleCollider;
    private float m_yRot;
    private float m_xRot;
    private Vector3 m_rotation;
    private Vector3 m_cameraRotation;
    private bool m_cursorIsLocked = true;
    private float speedRunningMultiplier = 2f;
    #endregion

    #region Private Serialize Fields

    [Header("The Camera the player looks through")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private bool movePlayer = true;
    [SerializeField] private bool moveX = true;
    [SerializeField] private bool moveY = true;
    [SerializeField] private bool hiddeCursor = false;

    [Header("Sensitivity")]
    [SerializeField] private bool moveViewX = true;
    [SerializeField] private bool moveViewY = true;
    [SerializeField] private float m_lookSensitivity = .5f;
    [SerializeField] private float speed = 2.0f;


    [Header("View Rotation Clamp")]
    [SerializeField] private float x_max = 320f;
    [SerializeField] private float x_min = 60f;

    #endregion

    // Use this for initialization
    private void Start()
    {
        m_Rigid = GetComponent<Rigidbody>();
        m_Camera = m_Camera == null ? GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() : m_Camera;
    }
    private void Update()
    {
        if (movePlayer)
        {
            if (moveX)
                m_MovX = Input.GetAxis("Horizontal");
            if (moveY)
                m_MovY = Input.GetAxis("Vertical");

            m_moveHorizontal = transform.right * m_MovX;
            m_movVertical = transform.forward * m_MovY;


            if (Input.GetKeyDown(KeyCode.LeftShift))
                speed *= speedRunningMultiplier;
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                speed /= speedRunningMultiplier;

            m_velocity = (m_moveHorizontal + m_movVertical).normalized * speed;
        }

        //mouse movement 
        if (moveViewY)
        {
            m_yRot = Input.GetAxisRaw("Mouse X");
            m_rotation = new Vector3(0, m_yRot, 0) * m_lookSensitivity;
        }
        if (moveViewX)
        {
            m_xRot = Input.GetAxisRaw("Mouse Y");
            m_cameraRotation = new Vector3(m_xRot, 0, 0) * m_lookSensitivity;
        }
        //apply camera rotation

        if (m_Camera != null)
        {
            m_Camera.transform.Rotate(-m_cameraRotation);
            float _x = m_Camera.transform.localRotation.eulerAngles.x;
            //Debug.LogWarning(m_Camera.transform.rotation.eulerAngles);
            if (_x > x_min && _x < x_max && m_cameraRotation.x < 0)
                m_Camera.transform.localRotation = Quaternion.Euler(x_min, m_Camera.transform.localRotation.y, m_Camera.transform.localRotation.z);
            else if (_x < x_max && _x > x_min && m_cameraRotation.x > 0)
                m_Camera.transform.localRotation = Quaternion.Euler(x_max, m_Camera.transform.localRotation.y, m_Camera.transform.localRotation.z);
        }
        if (m_rotation != Vector3.zero)
        {
            //rotate the camera of the player
            m_Rigid.MoveRotation(m_Rigid.rotation * Quaternion.Euler(m_rotation));
        }

        if (hiddeCursor)
            InternalLockUpdate();

    }

    private void FixedUpdate()
    {
        //move the actual player here
        if (m_velocity != Vector3.zero)
        {
            m_Rigid.MovePosition(m_Rigid.position + m_velocity * Time.fixedDeltaTime);
        }
    }

    //controls the locking and unlocking of the mouse
    private void InternalLockUpdate()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
            m_cursorIsLocked = false;

        else if (Input.GetMouseButtonUp(1))
            m_cursorIsLocked = !m_cursorIsLocked;

        if (m_cursorIsLocked)
            UnlockCursor();

        else if (!m_cursorIsLocked)
            LockCursor();
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}