using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas;

    void Update()
    {
        if (player.GetComponent<PlayerStatsScript>().GetHealth() <= 0)
        {
            Debug.Log(player.GetComponent<PlayerStatsScript>().GetHealth());
            canvas.SetActive(true);
        }
    }
}
