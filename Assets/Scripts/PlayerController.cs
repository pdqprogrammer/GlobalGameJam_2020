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

    public float jumpFallAcceleration = 0.3f;

    private float jumpApexY;
    private float jumpStartY;
    private float jumpTimeStart;

    public float maxSlideMultiplier = 2.0f;
    public float currSlideMultiplier = 1.0f;

    enum JumpEnvelope_t
    {
        jmpATTACK,
        jmpSUSTAIN,
        jmpFALL,
        jmpGROUNDED,
    };

    private JumpEnvelope_t jumpEnvelope = JumpEnvelope_t.jmpFALL;
    private float jumpFallSpeed = 0;

    Rigidbody rb;

    //

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        jumpEnvelope = JumpEnvelope_t.jmpGROUNDED;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onGround)
            return;

        if (Input.GetButtonDown("Jump") && !grabbing)
        {
            PlayerJump();
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

        switch (jumpEnvelope)
        {
        case JumpEnvelope_t.jmpATTACK:
            float elapsedTime = Mathf.Min ((Time.time - jumpTimeStart) * 3, 1.0f);
            if (elapsedTime == 1.0f)
            {
                jumpEnvelope = JumpEnvelope_t.jmpSUSTAIN;

                jumpFallSpeed = 0.0f;
            }

            Vector3 startPos = transform.position;
            startPos.y = jumpStartY;

            Vector3 endPos = transform.position;
            endPos.y = jumpApexY;

            transform.position = Vector3.Lerp (startPos, endPos, elapsedTime);
            break;
        case JumpEnvelope_t.jmpFALL:
            jumpFallSpeed += Time.deltaTime * jumpFallAcceleration/2;
            goto case JumpEnvelope_t.jmpSUSTAIN;
        case JumpEnvelope_t.jmpSUSTAIN:
            jumpFallSpeed += Time.deltaTime * jumpFallAcceleration/2;
            if (onGround)
            {
                rb.useGravity = true;
                break;
            }

            // stop half speed drop
            if (!Input.GetButton ("Jump"))
            {
                jumpEnvelope = JumpEnvelope_t.jmpFALL;
            }

            transform.position -= new Vector3 (0, jumpFallSpeed, 0);
            break;
        }
    }

    private void PlayerMove()
    {
        rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * playerSpeed * currSlideMultiplier;
    }

    private void PlayerJump()
    {
        jumpTimeStart = Time.time;
        jumpStartY = transform.position.y;
        jumpApexY = jumpStartY + jumpForce;
        jumpEnvelope = JumpEnvelope_t.jmpATTACK;
        rb.useGravity = false;

        onGround = false;
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
            Debug.Log("Touched the enemy");
        }

        //check if touching pullable object
        if (collision.gameObject.tag.Equals("PullableObject"))
        {
            if (!grabbing)
                nearPullObject = collision.gameObject;
        }

        //
        if (collision.gameObject.tag.Equals("BounceObject"))
        {
            PlayerJump();
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

    private void OnTriggerEnter(Collider other)
    {
        //may need to move to trigger
        if (other.gameObject.tag.Equals("Slider"))
        {
            currSlideMultiplier = maxSlideMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Slider"))
        {
            currSlideMultiplier = 1.0f;
        }
    }
}
