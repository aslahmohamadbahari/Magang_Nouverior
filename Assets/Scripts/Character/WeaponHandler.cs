
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogic,
        projectileTransform, projectileParent, projectile = null;

    private Attack currentAttack;
    private PoolManager poolManager;

    private const float decreaseAmount = 1f;

    private bool isFiring = false;
    [SerializeField] private float fireRate = .5f;
    [SerializeField] private float nextFireTime;

    private void Awake()
    {
        currentAttack = GetComponent<Attack>();
        poolManager = GetComponent<PoolManager>();
    }

    public void EnableWeapon()
    {
        weaponLogic.SetActive(true);
    }

    public void DisableWeapon()
    {
        weaponLogic.SetActive(false);
    }

    public void EventMagicFire()
    {
        //GameObject projectile = Instantiate(projectileTransform,
        //projectileParent.transform.position, transform.rotation);

        //projectile.transform.parent = projectileParent.transform;

        //currentAttack?.ChancesHit(decreaseAmount);
    }

    public void RangerFire()
    {
        if (Time.time >= nextFireTime)
        {
            poolManager.Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void EventFiring()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Firing");
    }
}
