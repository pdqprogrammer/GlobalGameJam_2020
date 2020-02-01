using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject hiddenPin;
    GameObject Pin;
    GameObject Spring;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PullableObject")
        {
            Pin = other.gameObject;
            Destroy(Pin);
            hiddenPin.SetActive(true);

            hiddenPin.GetComponent<PinBehavior>().Bounce();
        }
    }
}
