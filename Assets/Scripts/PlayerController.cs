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

    public float liftHeight = 0.3f;

    public bool onGround = true;

    private bool grabbing = false;
    private GameObject nearPullObject = null;

    public float jumpFallAcceleration = 0.3f;

    private float jumpApexY;
    private float jumpStartY;
    private float jumpTimeStart;

    public float maxSlideMultiplier = 2.0f;
    private float currSlideMultiplier = 1.0f;
    private bool onSlic = false;

    private PlayerStatsScript playerStats;

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
    RigidbodyConstraints grabObjConstraints;

    //

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerStats = gameObject.GetComponent<PlayerStatsScript>();
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

        if (Input.GetButtonDown("Grab") && nearPullObject != null)
        {
            PlayerGrab();

            Vector3 nearPullPos = nearPullObject.transform.position;
            nearPullPos.y += liftHeight;

            nearPullObject.transform.position = nearPullPos;
        }

        if (Input.GetButtonUp("Grab") && grabbing)
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

                transform.position = Vector3.Lerp(startPos, endPos, elapsedTime);
                break;
            case JumpEnvelope_t.jmpFALL:
                if (onGround)
                    break;

                jumpFallSpeed += Time.deltaTime * jumpFallAcceleration / 2;
                goto case JumpEnvelope_t.jmpSUSTAIN;
            case JumpEnvelope_t.jmpSUSTAIN:
                if (onGround)
                    break;
                else
                    jumpFallSpeed += Time.deltaTime * jumpFallAcceleration / 2;


                // stop half speed drop
                if (!Input.GetButton("Jump"))
                {
                    jumpEnvelope = JumpEnvelope_t.jmpFALL;
                }

                transform.position -= new Vector3(0, jumpFallSpeed, 0);
                break;
        }
    }

    private void PlayerMove()
    {
        if (!onGround)
        {
            Vector3 reducedAirSpeed = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * (playerSpeed * 0.05f);

            rb.velocity += reducedAirSpeed;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, playerSpeed * currSlideMultiplier);
        }
        else
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * playerSpeed * currSlideMultiplier;
        }
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
        nearPullObject.GetComponent<Rigidbody>().useGravity = false;
        grabObjConstraints = nearPullObject.GetComponent<Rigidbody>().constraints;
        nearPullObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        //Vector3 nearObjectPosition = nearPullObject.transform.position;

        //nearPullObject.transform.position = nearObjectPosition;
    }

    private void PlayerRelease()
    {
        nearPullObject.transform.parent = null;
        grabbing = false;

        nearPullObject.GetComponent<Rigidbody>().useGravity = true;
        nearPullObject.GetComponent<Rigidbody>().constraints = grabObjConstraints;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //collision checks
        if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals ("Untagged"))
        {
            //if (!onGround)
            //  currSlideMultiplier = onSlic ? maxSlideMultiplier : 1.0f;
            onGround = true;
            rb.useGravity = true;
            this.transform.parent = collision.transform;
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            //add behavior for hits
            playerStats.DamagePlayer();
            Debug.Log("Touched the enemy. Lives: " + playerStats.GetHealth());
        }

        //check if touching pullable object
        if (collision.gameObject.tag.Equals("PullableObject"))
        {
            if (!grabbing)
            {
                nearPullObject = collision.gameObject;
            }
        }

        //
        if (collision.gameObject.tag.Equals("BounceObject"))
        {
            PlayerJump();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals ("Untagged"))
        {
            //onGround = false;
            this.transform.parent = null;
        }

        //check if not touching pullable object when not pulling
        if (collision.gameObject.tag.Equals("PullableObject"))
        {
            if (!grabbing)
            {
                nearPullObject = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Slider"))
        {
            currSlideMultiplier = maxSlideMultiplier;
            onSlic = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Slider"))
        {
            if (onGround)
            {
                currSlideMultiplier = 1.0f;
            }
            onSlic = false;
        }
    }

    public bool InAir()
    {
        return !onGround;
    }
}
