using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public List<Target> Targets
    {
        get { return targets; }
        private set { }
    }

    private List<Target> targets = new List<Target>();

    private NPCStateMachine player;

    private const string PlayerTag = "Player";

    private void Awake()
    {
        player = GetComponentInParent<NPCStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out var target)) return;

        if (targets.Contains(target)) return;

        targets.Add(target);
        target.OnDestroyed += OnTargetDestroyed;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out var target)) return;

        if (target.CompareTag(PlayerTag))
        {
            player.FollowPlayer.isFollowing = true;
        }

        RemoveTarget(target);
    }


    private void RemoveTarget(Target target)
    {
        if (target == null) return;

        if (targets.Remove(target))
        {
            target.OnDestroyed -= OnTargetDestroyed;
        }
    }
    private void OnTargetDestroyed(Target target)
    {
        RemoveTarget(target);
    }
}
