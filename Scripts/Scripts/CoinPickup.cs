﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;

    public float waitToBeCollected;

    void Start()
    {
        
    }

    void Update()
    {
        if(waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && waitToBeCollected <= 0)
        {

            AudioManager.instance.PlaySFX(5);

            LevelManager.instance.GetCoins(coinValue);

            Destroy(gameObject);
        }
    }
}
