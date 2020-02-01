using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBackup : MonoBehaviour
{
    //player settings
    public float playerSpeed = 3.0f;
    public float jumpForce = 250.0f;
    public float gravityModifier = 10.0f;

    public bool onGround = true;
    public bool touchingPull = false;

    Rigidbody rb;

    //

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && onGround)
        {
            PlayerJump();
            onGround = false;
        }

        if (Input.GetButtonUp("Grab"))
        {
            PlayerRelease();
        }
    }

    void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * playerSpeed, 0, Input.GetAxis("Vertical") * playerSpeed);
    }

    private void PlayerJump()
    {
        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        //rb.AddForce(transform.TransformDirection(Vector3.up) * jumpForce * 10);
        //rb.velocity += new Vector3(0, jumpForce, 0);
        //rb.velocity += jumpForce * Vector3.up;
    }

    private void PlayerRelease()
    {
        //add functionality for letting go of an object currently held onto
    }

    private void OnCollisionEnter(Collision collision)
    {
        //collision checks
        if (collision.gameObject.tag.Equals("Ground"))
            onGround = true;

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            //add behavior for hits
        }

        //check if touching pullable object
        if (collision.gameObject.tag.Equals("PullableObject"))
        {
            touchingPull = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //check if not touching pullable object when not pulling
        if (collision.gameObject.tag.Equals("PullableObject"))
        {
            touchingPull = false;
        }
    }
}
