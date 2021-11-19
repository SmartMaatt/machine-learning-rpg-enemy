using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpeedController : MonoBehaviour
{
    private AbstractEntity _entity;
    private NavMeshAgent _navAgent;

    private float _currentMoveSpeed;
    private float _currentMaxSpeed;
    private bool _reachedSpeedPoint;
    private float _currentAcceleration;

    private void Start()
    {
        _entity = GetComponent<AbstractEntity>();
        _navAgent = _entity.GetNavMeshAgent();

        _currentMoveSpeed = _entity.restSpeed;
        _currentMaxSpeed = _entity.restSpeed;
        SetNavAgentSpeed(_currentMoveSpeed);

        _currentAcceleration = _entity.acceleration;
    }

    private void Update()
    {
        if(ReachedSpeedPoint())
        {
            ChangeSpeed();
        }
        RotateToPoint(_entity.GetCurrentDestination(), _entity.rotateAcceleration);
    }

    private bool ReachedSpeedPoint()
    {
        if (_currentMoveSpeed != _currentMaxSpeed)
            return true;
        else
            return false;
    }

    private void ChangeSpeed()
    {
        if (System.Math.Round(_currentMoveSpeed, 1) == _currentMaxSpeed)
        {
            _currentMoveSpeed = _currentMaxSpeed;
            _reachedSpeedPoint = true;
        }
        else if (_currentMoveSpeed < _currentMaxSpeed)
        {
            _currentMoveSpeed += Time.deltaTime * _currentAcceleration;
        }
        else if (_currentMoveSpeed > _currentMaxSpeed)
        {
            _currentMoveSpeed -= Time.deltaTime * _currentAcceleration;
        }

        SetNavAgentSpeed(_currentMoveSpeed);
    }


    /*>>> Getters <<<*/
    public Vector3 GetNavAgentSteeringTarget()
    {
        return _navAgent.steeringTarget;
    }


    /*>>> Setters <<<*/
    public void SetNavAgentSpeed(float speed)
    {
        _navAgent.speed = speed;
    }

    public void SetCurrentMaxSpeed(float maxSpeed)
    {
        _currentMaxSpeed = maxSpeed;
    }

    public void SetAcceleration(float acceleration)
    {
        _currentAcceleration = acceleration;
    }

    public void AddBonusAcceleration(float bonus)
    {
        _currentAcceleration = _currentAcceleration * bonus;
    }

    public void ResetToDefaultAcceleration()
    {
        _currentAcceleration = _entity.acceleration;
    }

    public void RotateToPoint(Vector3 pointToRotate, float rotateAcceleration)
    {
        Vector3 flatPointToRotate = new Vector3(pointToRotate.x, 0f, pointToRotate.z);
        Vector3 flatOwnPosition = new Vector3(transform.position.x, 0f, transform.position.z);

        Vector3 lookRotation = flatPointToRotate - flatOwnPosition;
        if (lookRotation != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), rotateAcceleration * Time.deltaTime);
    }
}
