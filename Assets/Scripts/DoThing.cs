using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoThing : MonoBehaviour
{

    public bool thing;

    public GameObject[] Start;

    public void TiggerTheThings()
    {
        for (int i = 0; i < Start.Length; i++)
        {
            Start[i].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (thing)
        {
            if(other.tag == "Player")
            {
                for (int i = 0; i < Start.Length; i++)
                {
                    Start[i].SetActive(true);
                }
            }
        }
    }
}
