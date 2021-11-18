using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private float damage;
    [SerializeField] private AbstractEntity entity;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        entity.GetHit(30);
    }
}
