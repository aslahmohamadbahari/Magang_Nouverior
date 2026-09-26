using UnityEngine;

public class NPCAttackingState : NPCBaseState
{

    private readonly int AttackHash = Animator.StringToHash("Attack");

    private const float CrossFadeDuration = 0.1f;

    public NPCAttackingState(NPCStateMachine stateMachine) : base(stateMachine) {}


    public override void Enter()
    {
        stateMachine.currentEnergy = stateMachine.Energy;

        stateMachine.currentTarget = GetHighestThreatWeightTarget();
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.FollowPlayer.isFollowing)
        {
            stateMachine.SwitchState(new NPCIdleState(stateMachine));
            return;
        }

        if (stateMachine.currentEnergy <= 0 || stateMachine.currentTarget == null)
        {
            stateMachine.SwitchState(new NPCTargetingState(stateMachine));
            return;
        }

        Move(deltaTime);
        FaceTarget(stateMachine.currentTarget.transform.position, deltaTime);
    }

    public override void Exit()
    {

    }
}
