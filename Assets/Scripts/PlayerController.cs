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

    public bool onGround = false;

    private bool grabbing = false;
    private GameObject nearPullObject = null;

    public float jumpFallAcceleration = 0.3f;

    private float jumpApexY;
    private float jumpStartY;
    private float jumpTimeStart;

    public float maxSlideMultiplier = 2.0f;
    private float currSlideMultiplier = 1.0f;
    private bool onSlic = false;

    public AudioClip audioJump;
    public AudioClip audioPull;
    public AudioClip audioPain;
    public AudioClip audioStep;
    public AudioClip audioLand;

    private PlayerStatsScript playerStats;

    enum JumpEnvelope_t
    {
        jmpATTACK,
        jmpSUSTAIN,
        jmpFALL,
        jmpLEDGEDROP,
    };

    private JumpEnvelope_t jumpEnvelope = JumpEnvelope_t.jmpLEDGEDROP;
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
            PlayerJump (jumpForce);
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
        PlayerRotate();//set  player rotation

        switch (jumpEnvelope)
        {
            case JumpEnvelope_t.jmpATTACK:
                float ofAthirdSecond = (Time.time - jumpTimeStart) * 3;
                float elapsedTime = Mathf.Min (Mathf.Log (ofAthirdSecond * 9 + 1, 10), 1.0f);
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

                jumpFallSpeed = Mathf.Min (jumpFallSpeed, playerSpeed*2);
                transform.position -= new Vector3(0, jumpFallSpeed, 0);
                break;
            case JumpEnvelope_t.jmpLEDGEDROP:
                jumpFallSpeed += Time.deltaTime * jumpFallAcceleration/2;
                jumpFallSpeed = Mathf.Min (jumpFallSpeed, playerSpeed*2);
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

    private void PlayerRotate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);
        }
    }

    public void PlayerJump (float jumpheight)
    {
        jumpTimeStart = Time.time;
        jumpStartY = transform.position.y;
        jumpApexY = jumpStartY + jumpheight;
        jumpEnvelope = JumpEnvelope_t.jmpATTACK;
        rb.useGravity = false;

        onGround = false;
        AudioSource.PlayClipAtPoint(audioJump, this.transform.position);
    }

    private void PlayerGrab()
    {
        nearPullObject.transform.parent = this.transform;
        grabbing = true;
        nearPullObject.GetComponent<Rigidbody>().useGravity = false;
        grabObjConstraints = nearPullObject.GetComponent<Rigidbody>().constraints;
        nearPullObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        AudioSource.PlayClipAtPoint (audioPull, this.transform.position);

        //Vector3 nearObjectPosition = nearPullObject.transform.position;

        //nearPullObject.transform.position = nearObjectPosition;
    }

    private void PlayerRelease()
    {
        grabbing = false;
        if (nearPullObject == null)
            return;

        nearPullObject.transform.parent = null;

        nearPullObject.GetComponent<Rigidbody>().useGravity = true;
        nearPullObject.GetComponent<Rigidbody>().constraints = grabObjConstraints;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //collision checks
        if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals ("Untagged"))
        {
            if (!onGround)
            {
                currSlideMultiplier = onSlic ? maxSlideMultiplier : 1.0f;
                AudioSource.PlayClipAtPoint (audioLand, this.transform.position);
            }

            onGround = true;
            rb.useGravity = true;
            this.transform.parent = collision.transform;
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            //add behavior for hits
            playerStats.DamagePlayer();
            Debug.Log("Touched the enemy. Lives: " + playerStats.GetHealth());
            AudioSource.PlayClipAtPoint (audioPain, this.transform.position);
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
            PlayerJump (jumpForce);
        }
    }

    private void OnCollisionStay (Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals ("Untagged"))
        {
            onGround = true;
            rb.useGravity = true;

            jumpEnvelope = JumpEnvelope_t.jmpSUSTAIN;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals ("Untagged"))
        {
            if (jumpEnvelope != JumpEnvelope_t.jmpATTACK)
            {
                jumpEnvelope = JumpEnvelope_t.jmpLEDGEDROP;
                jumpFallSpeed = 0;
            }

            rb.useGravity = false;
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
        return !onGround || jumpEnvelope == JumpEnvelope_t.jmpLEDGEDROP;
    }
}
