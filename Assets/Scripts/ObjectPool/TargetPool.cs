using UnityEngine;
using UnityEngine.Pool;

public class TargetPool : MonoBehaviour
{
    private IObjectPool<TargetPool> bulletPool;

    public void SetPool(IObjectPool<TargetPool> pool)
    {
        bulletPool = pool;
    }

    public void OnBecameInvisible()
    {
        bulletPool.Release(this);
    }

}
