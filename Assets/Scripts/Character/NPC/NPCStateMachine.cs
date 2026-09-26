using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCStateMachine : StateMachine
{
    [Header("Component")]
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public MemberFollowAi FollowPlayer { get; private set; }
    
    [Header("Movement")]
    [field: SerializeField] public float MovementSpeed { get; private set; }
    
    [Header("Targeting")]
    [field: SerializeField] public float ChasingRange { get; private set; }
    [field: SerializeField] public int TargetingRangeMin { get; private set; }
    [field: SerializeField] public int TargetingRangeMax { get; private set; }
    [field: SerializeField] public int AttackRange { get; private set; }

    [Header("Combat")]
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Energy { get; private set; }
    
    [Header("StrafeDirection")]
    [field: SerializeField] public float strafeRadius { get; private set; }
    [field: SerializeField] public float strafeSpeed { get; private set; }
    [field: SerializeField] public int timeRepositioning { get; private set; }

    public Target currentTarget { get; set; }

    public float currentEnergy { get; set; }

    public bool isRepositioning { get; set; }

    private const string PlayerTag = "Player";


    private void Start()
    {
        Agent.updatePosition = false;
        Agent.updateRotation = false;

        isRepositioning = false;

        FollowPlayer.player = GameObject.FindGameObjectWithTag(PlayerTag);

        SwitchState(new NPCIdleState(this));
    }

    private void OnEnable()
    {
        
    }

    private void HandleTakeDamage()
    {

    }

    private void HandleDie()
    {

    }
 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChasingRange);
    }

    public void ReduceEnergy()
    {
        currentEnergy -= 1;
    }
}
