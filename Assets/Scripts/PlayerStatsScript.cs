using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class PlayerStatsScript : MonoBehaviour
{
    public int playerMaxHealth = 3;
    private int currPlayerHealth;

    private bool hit = false;
    private float currInvincibleTime = 0.0f;
    public float maxInvincibleTime = 2.0f;

    private float airTime;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
        ResetAirTime();

        playerController = gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (hit)
        {
            currInvincibleTime += Time.deltaTime;

            if(currInvincibleTime > maxInvincibleTime)
            {
                currInvincibleTime = 0.0f;
                hit = false;

                Debug.Log("No Longer Invincible.");
            }
        }


        if (playerController.InAir())
        {
            airTime += Time.deltaTime;
        }
    }

    public int GetHealth()
    {
        return currPlayerHealth;
    }

    public void DamagePlayer()
    {
        if (!hit)
        {
            currPlayerHealth--;
            hit = true;
        }
    }

    public void HealPlayer(int healAmount)
    {
        currPlayerHealth += healAmount;
        
        if(currPlayerHealth > playerMaxHealth)
        {
            currPlayerHealth = playerMaxHealth;
        }
    }

    public void ResetHealth()
    {
        currPlayerHealth = playerMaxHealth;
    }

    public float GetAirTimer()
    {
        return airTime;
    }

    public void ResetAirTime()
    {
        airTime = 0.0f;
    }
}
