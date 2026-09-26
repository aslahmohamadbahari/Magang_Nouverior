using UnityEngine;

public class NPCIdleState : NPCBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public NPCIdleState(NPCStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (ClosestPlayer(stateMachine.FollowPlayer.player.transform.position, stateMachine.FollowPlayer.isFollowing))
        {

            stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
            FaceTarget(stateMachine.FollowPlayer.player.transform.position, deltaTime);
            MoveRangeToTarget(stateMachine.FollowPlayer.player.transform.position, deltaTime);
            return;
        }

        Move(deltaTime);
        if (IsInChasingRange())
        {
            stateMachine.currentTarget = GetHighestThreatWeightTarget();
            stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
            
            MoveRangeToTarget(stateMachine.currentTarget.transform.position, deltaTime);

            if(IsInTargetingRange(stateMachine.currentTarget, stateMachine.TargetingRangeMax))
            {
                stateMachine.SwitchState(new NPCTargetingState(stateMachine));
            }

            return;
        }

        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit() {}
}
