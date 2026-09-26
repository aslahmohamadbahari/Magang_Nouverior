
using UnityEngine;

public abstract class NPCBaseState : State
{
    protected NPCStateMachine stateMachine;

    public NPCBaseState(NPCStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void MoveForwardToTarget(Vector3 currentTarget, bool moveForward, float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = currentTarget;
            Vector3 desiredVelocity = moveForward
                ? stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed
                : -stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed;

            Move(desiredVelocity, deltaTime);
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    protected void MoveRangeToTarget(Vector3 currentTarget, float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = currentTarget;
            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    protected bool ClosestPlayer(Vector3 memberPos, bool closest)
    {
        if (!closest) return false;

        if (Vector3.Distance(stateMachine.transform.position, memberPos) <= 1f) return false;

        return true;
    }


    protected void FaceTarget(Vector3 currentTarget, float deltaTime)
    {
        Vector3 direction = (currentTarget - stateMachine.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation, deltaTime * 10f);
    }

    protected bool IsInChasingRange()
    {
        Target highestWeightTarget = GetHighestThreatWeightTarget();

        if (highestWeightTarget == null) return false;

        float distanceToTarget = Vector3.Distance(stateMachine.transform.position, highestWeightTarget.transform.position);
        return distanceToTarget <= stateMachine.ChasingRange;
    }

    protected Target GetHighestThreatWeightTarget()
    {
        if (stateMachine.Targeter.Targets == null || stateMachine.Targeter.Targets.Count == 0)
        {
            return null;
        }

        Target highestWeightTarget = null;
        float maxWeight = float.MinValue;

        foreach (var targetItem in stateMachine.Targeter.Targets)
        {
            if (targetItem == null) continue;

            float weight = targetItem.GetWeight();
            if (weight > maxWeight)
            {
                maxWeight = weight;
                highestWeightTarget = targetItem;
            }
        }

        return highestWeightTarget;
    }

    protected int GetTargetingRange()
    {
        int distanceToTarget = UnityEngine.Random.Range(stateMachine.TargetingRangeMin, stateMachine.TargetingRangeMax);
        return distanceToTarget;
    }

    protected bool IsInTargetingRange(Target currentTarget, float targetingRange)
    {
        if (currentTarget == null) return false;
        float npcDistanceSqr = (currentTarget.transform.position - stateMachine.transform.position).sqrMagnitude;
        return npcDistanceSqr <= targetingRange * targetingRange; 
    }
}