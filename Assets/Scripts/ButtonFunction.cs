using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunction : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip[] otherClip;

    public void TimeToStop()
    {
        source.volume = 0.3f;
        source.clip = otherClip[2];
        source.Play();
    }

    public void PizzaTime()
    {
        source.volume = 0.3f;
        source.clip = otherClip[1];
        source.Play();
    }

    //load level

    //quit
    //credits
    
}
