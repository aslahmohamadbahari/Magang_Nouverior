using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float distanceToAttack;

    private Targeter targeter;
    private Animator animator;
    private Target target;


    private string attacking = "Attacking";
    private string running = "Running";
    private string idle = "Idle";

    private float rotationDamping = 15f;

    private void Awake()
    {
        targeter = GetComponentInChildren<Targeter>();
        animator = GetComponent<Animator>();
        target = GetComponent<Target>();
    }

    private void Update()
    {
        AttackingTarget();
    }

    private void AttackingTarget()
    {
        if (targeter.Targets.Count == 0 ||
            targeter.Targets.Count < 1)
        {
            animator.Play(idle);
            return;
        }

        foreach (var target in targeter.Targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > distanceToAttack)
            {
                animator.Play(idle);
                //animator.Play(running);
            }
            else
            {
                //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 2);
                animator.Play(attacking);
            }

            FaceMovementDirection(target.transform.position - transform.position, Time.deltaTime);
        }

    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        transform.rotation = Quaternion.Lerp(
             transform.rotation,
             Quaternion.LookRotation(movement),
             deltaTime * rotationDamping);
    }
}
