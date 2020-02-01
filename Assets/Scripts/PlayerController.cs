using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //player settings
    public float playerSpeed = 3.0f;
    public float jumpForce = 250.0f;
    public float gravityModifier = 10.0f;

    public bool onGround = true;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = new Vector3(Mathf.Lerp(0, Input.GetAxis("Horizontal") * playerSpeed, 0.8f), 0, Mathf.Lerp(0, Input.GetAxis("Vertical") * playerSpeed, 0.8f));

        PlayerMove();

        if (Input.GetButtonDown("Jump") && onGround)
        {
            PlayerJump();
            onGround = false;
        }

        if (Input.GetButtonDown("Grab"))
        {
            PlayerGrab();
        }
    }

    private void PlayerMove()
    {
        rb.AddForce(Vector3.right * Input.GetAxis("Horizontal") * playerSpeed);
        rb.AddForce(Vector3.forward * Input.GetAxis("Vertical") * playerSpeed);
    }

    private void PlayerJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void PlayerGrab()
    {
        //add functionality for pushing and pulling
    }

    private void OnCollisionEnter(Collision collision)
    {
        //collision checks
        if (collision.gameObject.tag.Equals("Ground"))
            onGround = true;
    }
}
