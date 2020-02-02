using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{

    private Rigidbody rig;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
           rig = other.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.W))
                rig.useGravity = false;
            else
                rig.useGravity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Leaving trigger");
        rig.useGravity = true;
    }
}
