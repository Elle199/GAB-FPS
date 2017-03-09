using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class FPSController : MonoBehaviour
{
    [SerializeField]
    private bool mouseLock;
    [SerializeField]
    private bool m_isWalking;
    [SerializeField]
    [Range(1f, 5f)]
    private float mouseSensMultiplyer;
    [SerializeField]
    [Range(1f, 10f)]
    private float movementSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float gravityMultiplier;

    private Camera mCamera;
    private bool m_Jump;
    private bool isJumping;
    private bool previouslyGrounded;
    private float verticalRotaion;
    private float upDownLookRange = 60f;
    private float verticalVelocity;
    private Vector2 m_Input;
    private Vector3 movement;
    private Vector3 m_MoveDir = Vector3.zero;
    private CollisionFlags ccCollisionFlags;
    CharacterController cc;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mCamera = Camera.main;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("#" + m_Jump + "#");
        //Debug.Log(cc.isGrounded);
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Cursor.lockState = CursorLockMode.Locked;

        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!previouslyGrounded && cc.isGrounded)
        {
            m_MoveDir.y = 0f;
            isJumping = false;
        }
        if (!cc.isGrounded && !isJumping && previouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        previouslyGrounded = cc.isGrounded;

        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensMultiplyer;
        transform.Rotate(0, rotLeftRight, 0);

        verticalRotaion -= Input.GetAxis("Mouse Y") * mouseSensMultiplyer;
        verticalRotaion = Mathf.Clamp(verticalRotaion, -upDownLookRange, upDownLookRange);
        mCamera.transform.localRotation = Quaternion.Euler(verticalRotaion, 0, 0);

        /*float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, cc.radius, Vector3.down, out hitInfo,
                           cc.height / 2f, ~0, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;

        if (cc.isGrounded)
        {
            m_MoveDir.y = -Physics.gravity.y;
            if (m_Jump)
            {
                m_MoveDir.y = jumpSpeed;
                m_Jump = false;
                isJumping = true;
            }
        }
        else
        {
            m_MoveDir += Physics.gravity * gravityMultiplier * Time.deltaTime;
        }

        ccCollisionFlags = cc.Move(m_MoveDir * Time.deltaTime);*/

    }

    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, cc.radius, Vector3.down, out hitInfo,
                           cc.height / 2f, ~0, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;

        if (cc.isGrounded)
        {
            m_MoveDir.y = -Physics.gravity.y;
            if (m_Jump)
            {
                m_MoveDir.y = jumpSpeed;
                m_Jump = false;
                isJumping = true;
            }
        }
        else
        {
            m_MoveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }

        ccCollisionFlags = cc.Move(m_MoveDir * Time.fixedDeltaTime);
    }

    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        bool waswalking = m_isWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_isWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
        // set the desired speed to be walking or running
        speed = m_isWalking ? movementSpeed : runSpeed;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }
    }
}
