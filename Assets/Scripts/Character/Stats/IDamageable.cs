using System;

public interface IDamageable
{
    public Action<float> OnDamage { get; set; }
    void ReceiveDamage(float damage);
}
