using UnityEngine;

public class IsCoverAvaliableNode : Node
{
    private AbstractEntity entity;
    private Cover[] avaliableCovers;
    private Transform targetTransform;
    private Transform originTransform;
    private GetFloatValue SightConeRange;

    public IsCoverAvaliableNode(AbstractEntity entity, Cover[] avaliableCovers, Transform target, Transform origin, GetFloatValue SightConeRange)
    {
        this.entity = entity;
        this.avaliableCovers = avaliableCovers;
        targetTransform = target;
        originTransform = origin;
        this.SightConeRange = SightConeRange;
    }

    public override NodeState Evaluate()
    {
        SetState();

        Vector3 bestSpot = FindBestCoverSpot();
        entity.SetCurrentDestination(bestSpot);
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private Vector3 FindBestCoverSpot()
    {
        Vector3 currentDestination = entity.GetCurrentDestination();
        if (currentDestination != Vector3.zero)
        {
            if (CheckIfSpotIsValid(currentDestination))
            {
                return currentDestination;
            }
        }

        float minAngle = 90;
        Vector3 bestSpot = Vector3.zero;
        for (int i = 0; i < avaliableCovers.Length; i++)
        {
            Vector3 bestSpotInCover = FindBestSpotInCover(avaliableCovers[i], ref minAngle);
            if (bestSpotInCover != Vector3.zero)
            {
                bestSpot = bestSpotInCover;
            }
        }
        return bestSpot;
    }

    private Vector3 FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        Transform[] avaliableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for (int i = 0; i < avaliableSpots.Length; i++)
        {
            Vector3 direction = targetTransform.position - avaliableSpots[i].position;
            if (CheckIfSpotIsValid(avaliableSpots[i].position))
            {
                float angle = Vector3.Angle(avaliableSpots[i].forward, direction);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestSpot = avaliableSpots[i];
                }
            }
        }
        return bestSpot != null ? bestSpot.position : Vector3.zero;
    }

    private bool CheckIfSpotIsValid(Vector3 coverPoint)
    {
        bool sideOfWall = false;
        bool wayFreeFromTarget = false;

        RaycastHit hit;
        Vector3 coverToTarget = targetTransform.position - coverPoint;
        if (Physics.Raycast(coverPoint, coverToTarget, out hit))
        {
            if (hit.collider.transform != targetTransform)
            {
                sideOfWall = true;
            }
        }

        Vector3 originToCover = coverPoint - originTransform.position;
        Vector3 originToTarget = targetTransform.position - originTransform.position;

        float cosOfMaxSightAngle = Mathf.Cos(Mathf.Deg2Rad * SightConeRange() / 2);
        float coverOriginTargetAngle = Vector3.Dot(originToCover, originToTarget) / (originToCover.magnitude * originToTarget.magnitude);

        if (coverOriginTargetAngle < cosOfMaxSightAngle)
        {
            wayFreeFromTarget = true;
        }

        return (sideOfWall && wayFreeFromTarget);
    }

    private void SetState()
    {
        if (entity.GetEntityState() != EntityState.HIDE)
        {
            entity.SetEntityState(EntityState.HIDE);
        }
    }
}

public class CopyOfIsCoverAvaliableNode : Node
{
    private AbstractEntity entity;
    private Cover[] avaliableCovers;
    private Transform targetTransform;
    private Transform originTransform;
    private GetFloatValue SightConeRange;

    public CopyOfIsCoverAvaliableNode(AbstractEntity entity, Cover[] avaliableCovers, Transform target, Transform origin, GetFloatValue SightConeRange)
    {
        this.entity = entity;
        this.avaliableCovers = avaliableCovers;
        targetTransform = target;
        originTransform = origin;
        this.SightConeRange = SightConeRange;
    }

    public override NodeState Evaluate()
    {
        SetState();

        Vector3 bestSpot = FindBestCoverSpot();
        entity.SetCurrentDestination(bestSpot);
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private Vector3 FindBestCoverSpot()
    {
        Vector3 currentDestination = entity.GetCurrentDestination();
        if (currentDestination != Vector3.zero)
        {
            if (CheckIfSpotIsValid(currentDestination))
            {
                return currentDestination;
            }
        }

        float minAngle = 90;
        Vector3 bestSpot = Vector3.zero;
        for (int i = 0; i < avaliableCovers.Length; i++)
        {
            Vector3 bestSpotInCover = FindBestSpotInCover(avaliableCovers[i], ref minAngle);
            if (bestSpotInCover != Vector3.zero)
            {
                bestSpot = bestSpotInCover;
            }
        }
        return bestSpot;
    }

    private Vector3 FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        Transform[] avaliableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for (int i = 0; i < avaliableSpots.Length; i++)
        {
            Vector3 direction = targetTransform.position - avaliableSpots[i].position;
            if (CheckIfSpotIsValid(avaliableSpots[i].position))
            {
                float angle = Vector3.Angle(avaliableSpots[i].forward, direction);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestSpot = avaliableSpots[i];
                }
            }
        }
        return bestSpot != null ? bestSpot.position : Vector3.zero;
    }

    private bool CheckIfSpotIsValid(Vector3 coverPoint)
    {
        bool sideOfWall = false;
        bool wayFreeFromTarget = false;

        RaycastHit hit;
        Vector3 coverToTarget = targetTransform.position - coverPoint;
        if (Physics.Raycast(coverPoint, coverToTarget, out hit))
        {
            if (hit.collider.transform != targetTransform)
            {
                sideOfWall = true;
            }
        }

        Vector3 originToCover = coverPoint - originTransform.position;
        Vector3 originToTarget = targetTransform.position - originTransform.position;

        float cosOfMaxSightAngle = Mathf.Cos(Mathf.Deg2Rad * SightConeRange() / 2);
        float coverOriginTargetAngle = Vector3.Dot(originToCover, originToTarget) / (originToCover.magnitude * originToTarget.magnitude);

        if (coverOriginTargetAngle < cosOfMaxSightAngle)
        {
            wayFreeFromTarget = true;
        }

        return (sideOfWall && wayFreeFromTarget);
    }

    private void SetState()
    {
        if (entity.GetEntityState() != EntityState.HIDE)
        {
            entity.SetEntityState(EntityState.HIDE);
        }
    }
}
