using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private TargetPool projectilePrefab;
    [SerializeField] private Transform shootTransform;
    [SerializeField] private Transform poolParent;
    [SerializeField] private int maxPoolSize = 10;

    private IObjectPool<TargetPool> projectilePool;

    private void Awake()
    {
        projectilePool = new ObjectPool<TargetPool>(
            CreateBullet,
            OnGet,
            OnRelease,
            OnEmpty,
            maxSize: maxPoolSize
        );
    }

    private TargetPool CreateBullet()
    {
        TargetPool bullet = Instantiate(projectilePrefab);
        bullet.SetPool(projectilePool);
        return bullet;
    }

    private void OnGet(TargetPool bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.transform.parent = poolParent;
    }

    private void OnRelease(TargetPool bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnEmpty(TargetPool bullet)
    {
        if (bullet == null) return;
        Destroy(bullet.gameObject);
    }

    public void Shoot()
    {
        TargetPool bullet = projectilePool.Get();
        bullet.transform.position = shootTransform.position;
        bullet.transform.rotation = shootTransform.rotation;
    }


}
