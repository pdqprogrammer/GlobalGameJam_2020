using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinBehavior : MonoBehaviour
{
    Animator anim;
    
    public void Bounce()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("bounce", true);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            anim.SetBool("bounce", false);
        }
    }
}
