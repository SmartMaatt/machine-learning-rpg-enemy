using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    private void Start()
    {
        health = maxHealth;
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
}
