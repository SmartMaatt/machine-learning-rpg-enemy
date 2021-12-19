using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Walk params")]
    public float noWalk = 0.0f;
    public float walk = 0.5f;
    public float run = 1.0f;
    
    public void SetWalkAnimationValue(float value)
    {
        animator.SetFloat("Speed", value, 0.1f, Time.deltaTime);
    }

    public void SetHealAnimation(bool active)
    {
        animator.SetBool("Heal", active);
    }

    public void PlayGetHitAnimation()
    {
        animator.SetTrigger("GetHit");
    }

    public void PlayBlockAnimation()
    {
        animator.SetTrigger("Shield");
    }
}