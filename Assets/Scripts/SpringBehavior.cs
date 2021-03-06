﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject hiddenPin;
    GameObject Pin;
    GameObject Spring;

    public bool doThing;

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetButton("Grab") || Input.GetKey(KeyCode.P))
        {
            //Debug.Log("YOURE HOLDING AN OBJECT");
            if (other.gameObject.tag == "PullableObject")
            {
                //Debug.Log("Dont start anim");
            }
        }
        else
        {
            //Debug.Log("YOURE NOT HOLDING AND OBJEECT");
            if (other.gameObject.tag == "PullableObject")
            {
                Pin = other.gameObject;
                Destroy(Pin);
                hiddenPin.SetActive(true);

                hiddenPin.GetComponent<PinBehavior>().Bounce(true);
                //Debug.Log("DO THE THING");
                if (doThing)
                {
                    gameObject.GetComponent<DoThing>().TiggerTheThings();
                    doThing = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButton("Grab") || Input.GetKey(KeyCode.P))
        {
            //Debug.Log("YOURE HOLDING AN OBJECT");
            if (other.gameObject.tag == "PullableObject")
            {
                //Debug.Log("Dont start anim");
            }
        }
        else
        {
            //Debug.Log("YOURE NOT HOLDING AND OBJEECT");
            if (other.gameObject.tag == "PullableObject")
            {
                Pin = other.gameObject;
                Destroy(Pin);
                hiddenPin.SetActive(true);

                hiddenPin.GetComponent<PinBehavior>().Bounce(false);
                //Debug.Log("DO THE THING");
                if (doThing)
                {
                    gameObject.GetComponent<DoThing>().TiggerTheThings();
                    doThing = true;
                }
            }
        }
    }
}
