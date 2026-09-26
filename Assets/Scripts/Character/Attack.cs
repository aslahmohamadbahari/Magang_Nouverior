using UnityEngine;
using System;

[Serializable]
public class Attack 
{
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float ChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Energy { get; private set; }
}
