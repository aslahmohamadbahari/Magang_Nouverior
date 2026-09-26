
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public Target weightEnemy;
    public Attack attack;
    public WeaponHandler weaponHandler;

    const float decreaseAmount = 1f;

    private void Awake()
    {
        weightEnemy = GetComponentInParent<Target>();
        attack = GetComponentInParent<Attack>();
        weaponHandler = GetComponentInParent<WeaponHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent<NPCController>(out var npc))
        //{
        //    weightEnemy.SetWeight(weightEnemy.GetAmountWeight());
        //}

        if (other.TryGetComponent<EnemyController>(out var enemy))
        {
            //attack?.ChancesHit(decreaseAmount);
            weaponHandler.DisableWeapon();
        }
    }
}