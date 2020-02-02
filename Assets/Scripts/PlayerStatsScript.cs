﻿using System.Collections;
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

    Vector3 lastCheckPoint;
    public float respawnHeight = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
        ResetAirTime();

        playerController = gameObject.GetComponent<PlayerController>();
        lastCheckPoint = transform.position;
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

    public void FallRespawn()
    {
        transform.position = lastCheckPoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            lastCheckPoint = transform.position;
            lastCheckPoint.y += respawnHeight;
        }
    }
}
