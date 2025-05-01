using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 50;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDir;
    Rigidbody rb;
    public float groundDrag;
    public float currentSpeed = 0;

    public float jumpForce;
    public float jumpCooldown;
    public float airMult;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;

    [Header("Animation")]
    public GameObject charRig;
    private Animator anim;

    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip hitSound;

    public bool attackCooldown = false;

    public PlayerDataStorage playerDataStorage;


    private void Awake()
    {
        playerDataStorage = GameObject.FindGameObjectWithTag("Data").GetComponent<PlayerDataStorage>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = charRig.GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetMouseButtonDown(0) && attackCooldown == false)
        {
            StartCoroutine(Attack()); 
        }
    }

    private void Update()
    {
        MyInput();
        GroundCheck();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        moveDir = horizontalInput * orientation.right + verticalInput * orientation.forward;

        rb.AddForce(moveDir.normalized * speed * 10f, ForceMode.Force);
        charRig.transform.rotation = Quaternion.LookRotation(orientation.forward);

        if (moveDir.magnitude != 0f)
        {
            anim.SetBool("isWalking", true); 
        }
        else { anim.SetBool("isWalking", false); }
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f - 0.2f, ground);

        if (grounded)
        {
            rb.drag = groundDrag;
            readyToJump = true;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private IEnumerator Attack()
    {
        anim.SetTrigger("isAttacking");
        audioSource.PlayOneShot(attackSound);
        charRig.transform.localPosition = Vector3.zero;
        attackCooldown = true;
        yield return new WaitForSeconds(1);
        attackCooldown = false;

    }
}
