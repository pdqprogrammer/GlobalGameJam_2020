using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinBehavior : MonoBehaviour
{
    //States curState;

    public Animator animator;

    //enum States { Rigid, Bounce, Locked }

    [SerializeField]
    GameObject spring;

    private void Start()
    {
        //curState = States.Rigid;

        animator = this.gameObject.GetComponent<Animator>();
        //Bounce();
        //animator.Play("Pin_Static");

    }

    //called in SpringBehavior ln 26
    public void dothething(GameObject _spring)
    {
        _spring = spring;
       // if (curState == States.Rigid)
       //     Bounce();
    }

    //set to do animation
    public void Bounce()
    {
        Debug.Log("Bouncing");
        animator.SetBool("Bounce", true);
        //curState = States.Bounce;
        //gameObject.GetComponent<BoxCollider>().enabled = true;
        animator.Play("Pin_Bounce");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LockThis();
        }
    }

    private void LockThis()
    {
        Debug.Log("Locked");
        //curState = States.Locked;
        animator.SetBool("Bounce", false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Rigidbody rig = gameObject.GetComponent<Rigidbody>();
        rig.constraints = RigidbodyConstraints.FreezeAll;
        //spring.GetComponent<MeshRenderer>().enabled = false;
        //animator.SetBool("Bounce", false);
        //animator.Play("Pin_Fall");
    }


}
