using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationRotation : MonoBehaviour
{
    [SerializeField] private AbstractEntity entity;
    [SerializeField] private Vector3 playerHead;
    private EntityState currentState;

    private void Update()
    {
        currentState = entity.GetEntityState();
        if (currentState == EntityState.ATTACK || currentState == EntityState.CHASE)
        {
            transform.rotation = Quaternion.LookRotation((entity.GetPlayer().transform.position + playerHead) - transform.position);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(entity.GetCurrentDestination() - transform.position);
        }
    }
}
