using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpeedController : MonoBehaviour
{
    private AbstractEntity entity;
    private NavMeshAgent navAgent;
    private AnimationController animationController;

    private float currentMoveSpeed;
    private float currentMaxSpeed;
    private bool reachedSpeedPoint;
    private float currentAcceleration;
    private float currentWalkAnimationState;


    /*>>> Unity methods <<<*/
    private void Start()
    {
        entity = GetComponent<AbstractEntity>();
        animationController = entity.GetAnimationController();
        navAgent = entity.GetNavMeshAgent();

        currentMoveSpeed = entity.restSpeed;
        currentMaxSpeed = entity.restSpeed;
        SetNavAgentSpeed(currentMoveSpeed);

        currentAcceleration = entity.acceleration;
    }

    private void Update()
    {
        if (ReachedSpeedPoint())
        {
            ChangeSpeed();
        }
        RotateToPoint(entity.GetCurrentDestination(), entity.rotateAcceleration);
    }


    /*>>> Getters <<<*/
    public Vector3 GetNavAgentSteeringTarget()
    {
        return navAgent.steeringTarget;
    }


    /*>>> Setters <<<*/
    public void SetNavAgentSpeed(float speed)
    {
        navAgent.speed = speed;
    }

    public void SetCurrentMaxSpeed(float maxSpeed)
    {
        currentMaxSpeed = maxSpeed;
        SetCurrentWalkAnimationState(maxSpeed);
    }

    public void SetAcceleration(float acceleration)
    {
        currentAcceleration = acceleration;
    }

    public void AddBonusAcceleration(float bonus)
    {
        currentAcceleration = currentAcceleration * bonus;
    }

    public void ResetToDefaultAcceleration()
    {
        currentAcceleration = entity.acceleration;
    }

    private void SetCurrentWalkAnimationState(float speed)
    {
        if (speed == entity.walkSpeed)
        {
            currentWalkAnimationState = animationController.walk;
        }
        else if (speed == entity.runSpeed)
        {
            currentWalkAnimationState = animationController.run;
        }
        else
        {
            currentWalkAnimationState = animationController.noWalk;
        }

        animationController.SetWalkAnimationValue(currentWalkAnimationState);
    }


    /*>>> Utility methods <<<*/
    private bool ReachedSpeedPoint()
    {
        return currentMoveSpeed != currentMaxSpeed;
    }

    private void ChangeSpeed()
    {
        if (System.Math.Round(currentMoveSpeed, 1) == currentMaxSpeed)
        {
            currentMoveSpeed = currentMaxSpeed;
            reachedSpeedPoint = true;
        }
        else if (currentMoveSpeed < currentMaxSpeed)
        {
            currentMoveSpeed += Time.deltaTime * currentAcceleration;
        }
        else if (currentMoveSpeed > currentMaxSpeed)
        {
            currentMoveSpeed -= Time.deltaTime * currentAcceleration;
        }

        SetNavAgentSpeed(currentMoveSpeed);
    }

    public void RotateToPoint(Vector3 pointToRotate, float rotateAcceleration)
    {
        Vector3 flatPointToRotate = new Vector3(pointToRotate.x, 0f, pointToRotate.z);
        Vector3 flatOwnPosition = new Vector3(transform.position.x, 0f, transform.position.z);

        Vector3 lookRotation = flatPointToRotate - flatOwnPosition;
        if (lookRotation != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), rotateAcceleration);
    }

    public void ExplodePush(Vector3 pushDirection, float force)
    {
        StartCoroutine(ExplodePushExecutive(pushDirection, force));
    }

    private IEnumerator ExplodePushExecutive(Vector3 pushDirection, float force)
    {
        float time = 0.5f;
        Vector3 currentPosition = transform.position;
        Vector3 pushPosition = currentPosition + (pushDirection * force);
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime / time;
            transform.position = Vector3.Lerp(currentPosition, pushPosition, elapsedTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
