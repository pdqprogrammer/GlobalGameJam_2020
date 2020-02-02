using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebScript : MonoBehaviour
{
    public float jumpHeight = 4;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("PullableObject"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag.Equals ("Player"))
        {
            collision.gameObject.GetComponent <PlayerController>().PlayerJump (jumpHeight);
        }
    }
}
