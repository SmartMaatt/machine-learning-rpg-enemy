using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SpeedController))]
[RequireComponent(typeof(AnimationRiggingController))]
public abstract class AbstractEntity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected NavMeshAgent navAgent;
    [SerializeField] protected SpeedController speedController;
    [SerializeField] protected AnimationRiggingController animationRiggingController;
    [SerializeField] protected GameObject player;
    [SerializeField] protected Cover[] avaliableCovers;
    public LayerMask SolidGround;
    public LayerMask SolidWall;
    public LayerMask PlayerLayer;

    [Header("Health and armor")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected float lowHealthThreshold;
    [SerializeField] protected float criticalLowHealthThreshold;
    [Range(0,1)]
    [SerializeField] protected float coverHealProbability;
    [SerializeField] protected float dieAwaitTime;
    [SerializeField] protected int damage;
    [SerializeField] protected bool blocking;
    [SerializeField] protected bool immortal = false;
    private int blockingIndex = 0;

    [Header("Patroling")]
    public float restSpeed;
    public float walkSpeed;
    public float acceleration;
    public float walkPointRange;
    public float maxRestTime;
    public float minRestTime;
    [Range(10, 180)]
    public float turnMaxAngle;
    public float rotateAcceleration;

    [Header("Chasing")]
    public float runSpeed;
    public float obstacleRange;
    public float accelerationChaseBonus;

    [Header("Attacking")]
    public float breakAcceleration;
    public float timeBetweenAttacks;
    public float minTimeAttackStartDelay;
    public float maxTimeAttackStartDelay;

    [Header("Senses")]
    public float sightRange;
    [Range(10, 180)]
    public float sightConeRange;
    public float hearRange;
    public float attackRange;

    [Space]
    [SerializeField] protected Node decisionTreeTopNode;
    [SerializeField] protected Vector3 currentDestination;
    [SerializeField] protected EntityState entityState;

    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        speedController = GetComponent<SpeedController>();
        animationRiggingController = GetComponent<AnimationRiggingController>();
        avaliableCovers = FindObjectsOfType<Cover>();
        entityState = EntityState.WANDER;
        SetPlayer();
    }

    /*>>> General methods <<<*/
    public bool IsDead()
    {
        if (health == 0.0f)
        {
            return true;
        }
        return false;
    }

    public void RunCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }


    /*>>> Getters <<<*/
    public NavMeshAgent GetNavMeshAgent()
    {
        return navAgent;
    }

    public Transform GetNavMeshAgentTransform()
    {
        return navAgent.transform;
    }

    public SpeedController GetSpeedController()
    {
        return speedController;
    }

    public AnimationRiggingController GetAnimationRiggingController()
    {
        return animationRiggingController;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public Vector3 GetCurrentDestination()
    {
        return currentDestination;
    }

    public EntityState GetEntityState()
    {
        return entityState;
    }


    /*>>> Setters <<<*/
    public void SetNavAgentDestination(Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }

    public void SetPlayer()
    {
        player = FindObjectsOfType<PlayerMovement>()[0].transform.gameObject;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void ChangeHealth(float value)
    {
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0.0f)
        {
            health = 0.0f;
            Die();
        }
    }

    public void SetBlock(bool blocking)
    {
        this.blocking = blocking;
    }

    public void SetCurrentDestination(Vector3 currentDestination)
    {
        this.currentDestination = currentDestination;
    }

    public void SetEntityState(EntityState entityState)
    {
        this.entityState = entityState;
    }

    public void AddBlocking()
    {
        blockingIndex = Mathf.Clamp(blockingIndex++, 0, 100);
        blocking = blockingIndex > 0;
    }

    public void RemoveBlocking()
    {
        blockingIndex = Mathf.Clamp(blockingIndex--, 0, 100);
        blocking = blockingIndex > 0;
    }

    /*>>> ABSTRACT <<<*/
    protected abstract void ConstructBehaviourTree();
    public abstract void Die();
    public abstract void GetHit(float damage);
    public abstract void Attack();
}
