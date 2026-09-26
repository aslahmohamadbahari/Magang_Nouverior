
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Target parentWeight;
    [SerializeField] private float speed = 0f;

    private TargetPool poolTarget;

    private void Awake()
    {
        parentWeight = GetComponentInParent<Target>();
        poolTarget = GetComponent<TargetPool>();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out var target))
        {
            if (target.GetTargetJob() != parentWeight.GetTargetJob())
            {
                parentWeight.SetWeight(parentWeight.GetAmountWeight());
            }

            poolTarget.OnBecameInvisible();
            //Destroy(gameObject);
        }
    }
}