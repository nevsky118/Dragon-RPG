using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    float damageCaused;

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
        Debug.Log("damageableComponent = ", damageableComponent);
        if(damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
        Destroy(gameObject);
    }
}
