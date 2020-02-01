using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //player settings
    public float playerSpeed = 3.0f;
    public float jumpForce = 3.0f;
    public float gravityModifier = 10.0f;

    public bool onGround = true;

    private bool grabbing = false;
    private GameObject nearPullObject = null;

    private float jumpApexY;
    private float jumpStartY;
    private float jumpTimeStart;
    private bool jumpAttackApex;
    //private float yDown;

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
        if (!onGround)
            return;

        if (Input.GetButtonDown("Jump") && !grabbing)
        {
            PlayerJump();
            onGround = false;
        }

        if (Input.GetButtonDown ("Grab") && nearPullObject != null)
        {
            PlayerGrab();
        }

        if (Input.GetButtonUp ("Grab") && grabbing)
        {
            PlayerRelease();
        }
    }

    void FixedUpdate()
    {
        PlayerMove();

        if (jumpAttackApex)
        {
            float elapsedTime = Mathf.Min ((Time.time - jumpTimeStart) * 3, 1.0f);
            if (elapsedTime == 1.0f)
            {
                jumpAttackApex = false;
            }

            Vector3 startPos = transform.position;
            startPos.y = jumpStartY;

            Vector3 endPos = transform.position;
            endPos.y = jumpApexY;

            transform.position = Vector3.Lerp (startPos, endPos, elapsedTime);
        }
    }

    private void PlayerMove()
    {
        rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * playerSpeed;
    }

    private void PlayerJump()
    {
        jumpTimeStart = Time.time;
        jumpStartY = transform.position.y;
        jumpApexY = jumpStartY + jumpForce;
        jumpAttackApex = true;
    }

    private void PlayerGrab()
    {
        nearPullObject.transform.parent = this.transform;
        grabbing = true;
    }

    private void PlayerRelease()
    {
        //add functionality for letting go of an object currently held onto
        nearPullObject.transform.parent = null;
        grabbing = false;
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
            if (!grabbing)
                nearPullObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //check if not touching pullable object when not pulling
        if (collision.gameObject.tag.Equals("PullableObject"))
        {
            if (!grabbing)
                nearPullObject = null;
        }
    }
}
