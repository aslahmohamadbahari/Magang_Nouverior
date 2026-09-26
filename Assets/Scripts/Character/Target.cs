using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public enum TargetName
    {
        Boss,
        Ranger,
        Enemy,
        None,
    }

    public enum Alliance
    {
        Ally,
        Enemy,
    }

    public TargetName targetJob;
    [field: SerializeField] public Alliance alliance { get; private set; }
    public ProgressionWeightCharacter progressCharacter;
    public float TotalWeight = 0f;
    public float amountWeight = 1f;

    public event Action<Target> OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public TargetName GetTargetJob()
    {
        return targetJob;
    }

    public Alliance GetAlliance()
    {
        return alliance;
    }

    public float GetWeight()
    {
        return TotalWeight;
    }

    public float GetAmountWeight()
    {
        return amountWeight;
    }

    public void SetWeight(float newWeight)
    {
        TotalWeight = TotalWeight + newWeight;
    }
}

[Serializable]
public class ProgressionWeightCharacter
{
    public ThreatWeightCharacter threatCharacter;

    private CharacterClass characterType;
    private float totalWeight;
    private float amountWeight;
    
    public CharacterClass characterClass()
    {
        characterType = threatCharacter.jobName;
        return characterType;
    }
    public float TotalWeight()
    {
        totalWeight = threatCharacter.totalWeightCharacter;
        return totalWeight;
    }
    public float AmountWeight()
    {
        amountWeight = threatCharacter.amountWeightCharacter;
        return amountWeight;
    }
}