using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SpeedController))]
[RequireComponent(typeof(AnimationRiggingController))]
[RequireComponent(typeof(AnimationController))]
public abstract class AbstractEntity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected NavMeshAgent navAgent;
    [SerializeField] protected SpeedController speedController;
    [SerializeField] protected AnimationRiggingController animationRiggingController;
    [SerializeField] protected AnimationController animationController;
    [SerializeField] protected RLAgent rlAgent;
    [SerializeField] protected GameObject enemy;
    [SerializeField] protected Cover[] avaliableCovers;
    public LayerMask SolidGround;
    public LayerMask SolidWall;
    public LayerMask PlayerLayer;

    [Header("UI configuration")]
    [SerializeField] protected PanelType uiPanelType;
    [SerializeField] protected string entityName;

    [Header("Health and armor")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected float lowHealthThreshold;
    [SerializeField] protected float criticalLowHealthThreshold;
    [SerializeField] protected float dieAwaitTime;
    [SerializeField] protected int damage;
    [SerializeField] protected bool blocking;
    [SerializeField] protected bool healing;
    [SerializeField] protected bool immortal = false;

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
    public float headPosition;
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

    protected Node decisionTreeTopNode;
    protected Vector3 currentDestination;
    protected EntityState entityState;
    protected PanelControll uiPanelController;

    protected PlayerController playerEnemyController;
    protected AbstractEntity entityEnemyController;

    public float enemyInterestTime;
    public float fullEpisodeTime;

    /*>>> Unity methods <<<*/
    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        speedController = GetComponent<SpeedController>();
        animationRiggingController = GetComponent<AnimationRiggingController>();
        animationController = GetComponent<AnimationController>();
        avaliableCovers = FindObjectsOfType<Cover>();

        entityState = EntityState.WANDER;
    }

    protected virtual void Start()
    {
        uiPanelController = Managers.UI.SetupUIPanelController(this.gameObject, uiPanelType);
        uiPanelController.SetupHealth(maxHealth, health);
        uiPanelController.SetupName(entityName);
        uiPanelController.SetupScore(0);

        playerEnemyController = enemy.GetComponent<PlayerController>();
        entityEnemyController = enemy.GetComponent<AbstractEntity>();
        ResetEnemyInterestTime();
    }

    protected virtual void Update()
    {
        AddEnemyInterestTime();
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

    private void AddEnemyInterestTime()
    {
        fullEpisodeTime += Time.deltaTime;
        if(IsInterestedInEnemy())
        {
            enemyInterestTime += Time.deltaTime;
        }
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

    public AnimationController GetAnimationController()
    {
        return animationController;
    }

    public AnimationRiggingController GetAnimationRiggingController()
    {
        return animationRiggingController;
    }

    public RLAgent GetRLAgent()
    {
        return rlAgent;
    }

    public GameObject GetEnemy()
    {
        return enemy;
    }

    public float GetEnemyHeadPosition()
    {
        if (playerEnemyController != null)
        {
            return playerEnemyController.GetHeadPosition();
        }

        if (entityEnemyController != null)
        {
            return entityEnemyController.headPosition;
        }

        return 0f;
    }

    public Vector3 GetCurrentDestination()
    {
        return currentDestination;
    }

    public EntityState GetEntityState()
    {
        return entityState;
    }

    public PanelType GetUIPanelType()
    {
        return uiPanelType;
    }

    public PanelControll GetUIPanelControll()
    {
        return uiPanelController;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetNormalizedHealth()
    {
        return (health / maxHealth);
    }

    public string GetEntityName()
    {
        return entityName;
    }

    public float GetEnemyInterestTime()
    {
        return enemyInterestTime;
    }

    public float GetFullEpisodeTime()
    {
        return fullEpisodeTime;
    }

    public float GetPercentageEnemyInteresetTime()
    {
        return (enemyInterestTime * 100 / fullEpisodeTime);
    }

    public bool IsBlocking()
    {
        return blocking;
    }

    public bool IsHealing()
    {
        return healing;
    }

    public bool IsAttacking()
    {
        return entityState == EntityState.ATTACK;
    }

    public bool IsChasing()
    {
        return entityState == EntityState.CHASE;
    }

    public bool IsHidding()
    {
        return entityState == EntityState.HIDE;
    }

    public bool IsWandering()
    {
        return entityState == EntityState.WANDER;
    }

    public bool IsInterestedInEnemy()
    {
        return (entityState == EntityState.ATTACK || entityState == EntityState.CHASE || entityState == EntityState.HIDE);
    }

    public bool IsHealthLow()
    {
        return health < lowHealthThreshold;
    }

    public bool IsHealthCriticalLow()
    {
        return health < criticalLowHealthThreshold;
    }


    /*>>> Setters <<<*/
    public void SetRLAgent(RLAgent rlAgent)
    {
        this.rlAgent = rlAgent;
    }

    public void SetNavAgentDestination(Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }

    public void AddRLReward(float reward)
    {
        try
        {
            rlAgent.AddRLReward(reward);
        }
        catch (NullReferenceException err)
        {
            Debug.LogError(err.Message);
        }
    }

    public void SetRLReward(float reward)
    {
        try
        {
            rlAgent.SetReward(reward);
        }
        catch (NullReferenceException err)
        {
            Debug.LogError(err.Message);
        }
    }

    public void SetPlayerAsEnemy()
    {
        enemy = FindObjectsOfType<PlayerMovement>()[0].transform.gameObject;
    }

    public void SetEnemy(GameObject player)
    {
        this.enemy = player;
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

        uiPanelController.ChangeHealth(health);
    }

    public void InstantKill()
    {
        ChangeHealth(-maxHealth);
    }

    public void RefilHealth()
    {
        health = maxHealth;
        uiPanelController.ChangeHealth(health);
    }

    public void SetBlocking(bool blocking)
    {
        this.blocking = blocking;
    }

    public void SetHealing(bool healing)
    {
        this.healing = healing;
    }

    public void SetCurrentDestination(Vector3 currentDestination)
    {
        this.currentDestination = currentDestination;
    }

    public void SetEntityState(EntityState entityState)
    {
        this.entityState = entityState;
    }

    public void ResetEnemyInterestTime()
    {
        this.enemyInterestTime = 0.0f;
        this.fullEpisodeTime = 0.0f;
    }

    /*>>> ABSTRACT <<<*/
    protected abstract void ConstructBehaviourTree();
    public abstract void Die();
    public abstract void GetHit(float damage);
    public abstract void GetMagicHit(float damage, int spellType);
    public abstract void Attack();
}
