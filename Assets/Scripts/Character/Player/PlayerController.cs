using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationDamping;

    private readonly int SpeedHash = Animator.StringToHash("Speed");
    
    private const float AnimatorDampTime = 0.1f;


    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3
        {
            x = inputReader.MovementValue.x,
            y = 0,
            z = inputReader.MovementValue.y
        };


        characterController.Move(movement * movementSpeed * Time.deltaTime);

        UpdateAnimation(movement);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        transform.rotation = Quaternion.Lerp(
             transform.rotation,
             Quaternion.LookRotation(movement),
             deltaTime * rotationDamping);
    }

    private void UpdateAnimation(Vector3 movement)
    {
        if (inputReader.MovementValue == Vector2.zero)
        {
            animator.SetFloat(SpeedHash, movement.magnitude, AnimatorDampTime, Time.deltaTime);
            return;
        }

        animator.SetFloat(SpeedHash, movement.magnitude, AnimatorDampTime, Time.deltaTime);
        FaceMovementDirection(movement, Time.deltaTime);
    }

    //private void Dump()
    //{

    //    if (inputReader.IsAttacking && !isAttacking)
    //    {
    //        animator.Play(AttackHash);
    //        isAttacking = true;
    //    }

    //    if (isAttacking)
    //    {
    //        var state = animator.GetCurrentAnimatorStateInfo(0);

    //        if (state.shortNameHash != AttackHash ||
    //            state.normalizedTime >= 1f)
    //        {
    //            animator.CrossFade(IdleHash, CrossFadeDuration);
    //            isAttacking = false;
    //        }
    //    }
    //}
}