using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float health;
    public float maxHealth;

    private PlayerMovement playerMovement;

    private void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(health < 0)
        {
            Die();
        }
    }

    public void ChangeHealth(float hurt)
    {
        health += hurt;
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    private void Die()
    {
        Debug.Log("I've never died before!");
    }

    public PlayerMovement GetPlayerMovement()
    {
        return playerMovement;
    }
}
