using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBehavior : MonoBehaviour
{
    GameObject pin;
    GameObject Spring;
    [SerializeField]
    GameObject hiddenPin;

    private void Start()
    {
        Spring = this.gameObject;
        //hiddenPin = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider name is " + other);
        if (other.gameObject.tag == "PullableObject")
        {
            Debug.Log("Setting Pin");
            pin = other.gameObject;
            Debug.Log("toggle spring collider");
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Debug.Log("do the thing");
            //pin.GetComponent<PinBehavior>().dothething(Spring);
            Destroy(pin);
            hiddenPin.SetActive(true);
            hiddenPin.GetComponent<PinBehavior>().Bounce();
        }
    }


}
