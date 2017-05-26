using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float mouseSensMultiplyer;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private GameObject healthDisplay;

    private float verticalRotatin = 0f;
    private float upDownRange = 85f;
    private float dash = 1f;
    private int maxHealth = 100;
    private int currentHealth;

    private Rigidbody rb;
    private Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = gameObject.GetComponent<Rigidbody>();

        currentHealth = maxHealth;
        healthDisplay.GetComponent<Text>().text = currentHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Cursor.lockState = CursorLockMode.Locked;

        float movH = Input.GetAxis("Horizontal");
        float movV = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * movH;
        Vector3 movVertical = transform.forward * movV;

        if (Input.GetButtonDown("Jump"))
            rb.AddForce(-Physics.gravity * jumpPower);
        if (Input.GetKey(KeyCode.LeftShift))
            dash = dashSpeed;
        else
            dash = 1f;

        Vector3 velocity = (movHorizontal + movVertical).normalized * speed * dash;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Move(velocity);
            Turning();
        }
    }

    void Move(Vector3 _Velocity)
    {
        velocity = _Velocity;

        if (velocity != Vector3.zero)
        {
            rb.MovePosition(transform.position + velocity * Time.deltaTime);
        }
    }

    void Turning()
    {
        //Player turning
        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensMultiplyer;
        transform.Rotate(0, rotLeftRight, 0);

        //Camera turning
        verticalRotatin -= Input.GetAxis("Mouse Y") * mouseSensMultiplyer;
        verticalRotatin = Mathf.Clamp(verticalRotatin, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotatin, 0, 0);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthDisplay.GetComponent<Text>().text = currentHealth.ToString();

        if(currentHealth <= 0)
        {
            currentHealth = maxHealth;
            Debug.Log("Player Died");
            transform.position = new Vector3(0, 1.1f, 0);
        }
    }
}
