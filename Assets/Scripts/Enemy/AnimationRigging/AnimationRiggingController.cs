using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;

public class AnimationRiggingController : MonoBehaviour
{
    [Header("Head rigging")]
    [SerializeField] private float playerHeadPosition;
    [SerializeField] private GameObject headTarget;
    [SerializeField] private MultiAimConstraint headAimComponent;
    [SerializeField] private MultiAimConstraint chestAimComponent;

    [Header("Arms rigging")]
    [SerializeField] private TwoBoneIKConstraint leftArmComponent;
    [SerializeField] private GameObject leftArmTarget;
    [SerializeField] private Vector3 leftArmTargetDefaultPosition;

    [Space]
    [SerializeField] private TwoBoneIKConstraint rightArmComponent;
    [SerializeField] private GameObject rightArmTarget;
    [SerializeField] private Vector3 rightArmTargetDefaultPosition;

    private Transform player;
    private AbstractEntity entity;

    private void Start()
    {
        player = FindObjectsOfType<PlayerMovement>()[0].transform;
        entity = GetComponent<AbstractEntity>();
    }

    private void Update()
    {
        SetHeadTargetPoint();
    }

    public void SetHeadTargetPoint()
    {
        EntityState currentEntityState = entity.GetEntityState();
        if (currentEntityState == EntityState.ATTACK || currentEntityState == EntityState.CHASE)
        {
            ChangeWeightOfHead(1f, 0.3f);
            ChangeTargetToPlayer(playerHeadPosition);
        }
        else
        {
            ChangeWeightOfHead(0f, 0f);
            ChangeTargetToDefault();
        }
    }

    public void ChangeWeightOfHead(float headWeight, float chestWeight)
    {
        headAimComponent.weight = Mathf.Clamp(headWeight, 0f, 1f);
        chestAimComponent.weight = Mathf.Clamp(chestWeight, 0f, 1f);
    }

    public void ChangeTargetToPlayer(float playerHead)
    {
        headTarget.transform.position = player.position + new Vector3(0f, playerHead, 0f);
    }

    public void ChangeTargetToDefault()
    {
        headTarget.transform.position = transform.position;
    }

    public void ThrowCastSpell(float time, Transform castSpell)
    {
        StartCoroutine(ThrowCastSpellController(time, castSpell));
    }

    private Vector3 CalculateRelativeTargetPosition(Vector3 parentPos, Vector3 objPos)
    {
        return parentPos + objPos;
    }

    private IEnumerator ThrowCastSpellController(float time, Transform castSpell)
    {
        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime / time;
            leftArmComponent.weight = elapsedTime;
            rightArmComponent.weight = elapsedTime;

            leftArmTarget.transform.position = castSpell.transform.position;
            rightArmTarget.transform.position = castSpell.transform.position;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(time / 3);
        Vector3 currentPosLeftTarget = leftArmTarget.transform.position;
        Vector3 currentPosRightTarget = rightArmTarget.transform.position;

        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime / time;
            leftArmComponent.weight = elapsedTime;
            rightArmComponent.weight = elapsedTime;

            leftArmTarget.transform.position = Vector3.Lerp(currentPosLeftTarget, CalculateRelativeTargetPosition(transform.position, leftArmTargetDefaultPosition), 1 - elapsedTime);
            rightArmTarget.transform.position = Vector3.Lerp(currentPosRightTarget, CalculateRelativeTargetPosition(transform.position, rightArmTargetDefaultPosition), 1 - elapsedTime);
            yield return new WaitForEndOfFrame();
        }
    }
}