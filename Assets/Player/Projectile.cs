using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;

    [SerializeField] GameObject shooter;
    float damageCaused;

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }

    public float GetDefaultLaunchSpeed()
    {
        return projectileSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {

        var layerCollidedWith = collision.gameObject.layer;
        if (layerCollidedWith != shooter.layer)
        {
            DamageDamageables(collision);
        }
    }

    private void DamageDamageables(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
        Debug.Log("damageableComponent = ", damageableComponent);
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
        Destroy(gameObject);
    }
}
