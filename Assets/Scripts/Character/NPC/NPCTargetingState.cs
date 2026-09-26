using UnityEngine;

public class NPCTargetingState : NPCBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;
    
    private float strafeDirection;
    private float strafeTimer;
    private float distanceToTarget;

    private int currentTargetRange;

    public NPCTargetingState(NPCStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.isRepositioning = false;
        strafeTimer = StrafeDirectionTiming(stateMachine.timeRepositioning);
        
        stateMachine.currentTarget = GetHighestThreatWeightTarget();
        
        currentTargetRange = GetTargetingRange();
        //distanceToTarget = Vector3.Distance(stateMachine.transform.position, stateMachine.currentTarget.transform.position);

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {

        if (!IsInChasingRange() ||
            stateMachine.FollowPlayer.isFollowing)
        {
            stateMachine.SwitchState(new NPCIdleState(stateMachine));
            return;
        }

        if (stateMachine.isRepositioning && strafeTimer <= 0f)
        {
            stateMachine.SwitchState(new NPCAttackingState(stateMachine));
            return;
        }

        float currentDistance = Vector3.Distance(
            stateMachine.transform.position,
            stateMachine.currentTarget.transform.position);

        const float tolerance = 0.1f;
        bool tooFar = currentDistance > currentTargetRange + tolerance;
        bool tooClose = currentDistance < currentTargetRange - tolerance;

        UpdateAnimator(stateMachine.isRepositioning, tooFar, tooClose, deltaTime);
        FaceTarget(stateMachine.currentTarget.transform.position, deltaTime);
    }

    private void UpdateAnimator(bool repositioning, bool tooFar, bool tooClose, float deltaTime)
    {
        if (repositioning)
        {
            strafeTimer -= deltaTime;
            if (strafeDirection == 0f)
            {
                stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
            }
            else
            {
                float value = strafeDirection > 0 ? -1f : 1f;
                stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
            }

            MoveRangeToTarget(RepositionAroundTarget(), deltaTime);
        }
        else
        {
            if (!tooFar && !tooClose)
            {
                stateMachine.isRepositioning = true;
                stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
            }
            else
            {
                float value = tooFar ? 1f : -1f;
                stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);

                bool shouldMoveForward = tooFar;
                MoveForwardToTarget(stateMachine.currentTarget.transform.position, shouldMoveForward, deltaTime);
            }
        }
    }
    public override void Exit()
    {
        stateMachine.isRepositioning = false;
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private Vector3 RepositionAroundTarget()
    {
        if (Mathf.Approximately(strafeDirection, 0f))
        {
            strafeDirection = Random.value > 0.5f ? 1f : -1f;
        }

        Vector3 directionToTarget = 
            (stateMachine.transform.position - 
            stateMachine.currentTarget.transform.position).normalized;

        Vector3 strafeDir = Vector3.Cross(Vector3.up, directionToTarget) * strafeDirection;

        Vector3 desiredPos = stateMachine.currentTarget.transform.position +
                     directionToTarget * currentTargetRange +
                     strafeDir * Mathf.Clamp(stateMachine.strafeSpeed, 0.1f, 10f);

        return desiredPos;
    }

    private float StrafeDirectionTiming(float strafeTime)
    {
        strafeTime = Random.Range(3f, stateMachine.timeRepositioning);

        return strafeTime;
    }
}