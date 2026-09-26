
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
public class ThreatWeightCharacter : ScriptableObject
{
    public CharacterClass jobName;
    public float damage;
    public float totalWeightCharacter;
    public float amountWeightCharacter;
}