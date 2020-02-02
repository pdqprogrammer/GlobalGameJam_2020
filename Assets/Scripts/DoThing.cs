using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoThing : MonoBehaviour
{
    
    public GameObject[] Start;

    public void TiggerTheThings()
    {
        for (int i = 0; i < Start.Length; i++)
        {
            Start[i].SetActive(true);
        }
    }
}
