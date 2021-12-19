using System;
using System.Collections;
using UnityEngine;

public class WanderNode : Node
{
    private AbstractEntity entity;
    private SpeedController speedController;

    private GetFloatValue RestSpeed;
    private GetFloatValue Acceleration;

    private GetFloatValue WalkPointRange;
    private GetFloatValue MinRestTime;
    private GetFloatValue MaxRestTime;
    private GetFloatValue MaxTurnAngle;
    private Transform originTransform;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool enemyReadyToPatrol;
    private bool shouldIRest;

    public WanderNode(AbstractEntity entity, Transform origin, GetFloatValue[] delegates)
    {
        this.entity = entity;
        speedController = entity.GetSpeedController();
        originTransform = origin;

        walkPoint = Vector3.zero;
        walkPointSet = false;
        enemyReadyToPatrol = false;
        shouldIRest = false;

        try
        {
            RestSpeed = delegates[0];
            Acceleration = delegates[1];
            WalkPointRange = delegates[2];
            MinRestTime = delegates[3];
            MaxRestTime = delegates[4];
            MaxTurnAngle = delegates[5];
        }
        catch (ArgumentOutOfRangeException err)
        {
            RestSpeed = null;
            Acceleration = null;
            WalkPointRange = null;
            MinRestTime = null;
            MaxRestTime = null;
            MaxTurnAngle = null;
            Debug.Log("Wander node: " + err);
        }
    }

    public override NodeState Evaluate()
    {
        if(entity.GetEntityState() != EntityState.WANDER)
        {
            entity.SetEntityState(EntityState.WANDER);
            walkPointSet = false;
            enemyReadyToPatrol = false;
            shouldIRest = false;
        }

        if (!walkPointSet && !enemyReadyToPatrol)
        {
            SearchWalkPoint();
        }
        else if (walkPointSet && !enemyReadyToPatrol)
        {
            speedController.SetCurrentMaxSpeed(RestSpeed());
            speedController.SetAcceleration(Acceleration());
        }
        else if (walkPointSet && enemyReadyToPatrol)
        {
            float distance = Vector3.Distance(entity.GetCurrentDestination(), originTransform.position);

            if (distance < 0.2f)
            {
                walkPointSet = false;
                enemyReadyToPatrol = false;
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
        return NodeState.RUNNING;
    }

    private void SearchWalkPoint()
    {
        float randomX = UnityEngine.Random.Range(-WalkPointRange(), WalkPointRange());
        float randomZ = UnityEngine.Random.Range(-WalkPointRange(), WalkPointRange());

        RaycastHit hit;
        Vector3 _tmpWalkPoint = new Vector3(originTransform.position.x + randomX, originTransform.position.y + 500f, originTransform.position.z + randomZ);

        if (Physics.Raycast(_tmpWalkPoint, -originTransform.up, out hit, Mathf.Infinity, entity.SolidGround) &&
            Vector2.Angle(
                new Vector2(_tmpWalkPoint.x, _tmpWalkPoint.z),
                new Vector2(originTransform.position.x, originTransform.position.z)
            )
            < MaxTurnAngle())
        {
            walkPoint = hit.point;
            walkPointSet = true;
            entity.SetCurrentDestination(walkPoint);
            Debug.Log("New walk point " + walkPoint);

            if(shouldIRest)
            {
                float timeToRest = UnityEngine.Random.Range(MinRestTime(), MaxRestTime());
                entity.RunCoroutine(EnemyWalkingPause(timeToRest));
            }
            else
            {
                enemyReadyToPatrol = true;
                shouldIRest = true;
            }
        }
    }

    private IEnumerator EnemyWalkingPause(float timeOfRest)
    {
        Debug.Log("Started restring, sec: " + timeOfRest);
        yield return new WaitForSeconds(timeOfRest);
        enemyReadyToPatrol = true;
        shouldIRest = true;
        Debug.Log("Finished resting!");
    }
}
