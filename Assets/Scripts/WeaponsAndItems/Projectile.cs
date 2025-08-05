using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IThrowable
{
    [HideInInspector] public List<Collider2D> enemiesHited = new List<Collider2D>();
    private int damage;
    private int health;
    private float speed;
    private float stayTime;
    private bool hasEvolved;
    private LayerMask damageableLayer;
    [SerializeField] private bool isArrow;
    public void Equalize(int damage, int health, float stayTime, float speed, bool hasEvolved,
        LayerMask damageableLayer)
    {
        this.damage = damage;
        this.health = health;
        this.speed = speed;
        this.hasEvolved = hasEvolved;
        this.stayTime = stayTime;
        this.damageableLayer = damageableLayer;
    }

    private void FixedUpdate()
    {
        stayTime -= Time.fixedDeltaTime;
        if (stayTime < 0)
            Destroy(gameObject);

        transform.position += transform.up * (speed * Time.deltaTime);

        var damageableLayers = Physics2D.OverlapCircleAll(transform.position, .3f, damageableLayer);

        if (damageableLayers.Length > 0)
        {
            foreach (var damageable in damageableLayers)
            {

                if (!enemiesHited.Contains(damageable))
                {
                    enemiesHited.Add(damageable);

                    health -= 1;

                    damageable.GetComponent<IDamage>().TakeDamage(damage, 1);

                    if (health == 0)
                    {
                        Destroy(gameObject);
                    }
                    else if (hasEvolved && isArrow)
                    {
                        var projectile1 = Instantiate(gameObject, transform.position,
                            Quaternion.Euler(0, 0, transform.eulerAngles.z + 15), null);
                        var projectile2 = Instantiate(gameObject, transform.position,
                            Quaternion.Euler(0, 0, transform.eulerAngles.z - 15), null);
                        projectile1.GetComponent<IThrowable>().Equalize(damage, health, stayTime, speed, hasEvolved,
                            damageableLayer);
                        projectile2.GetComponent<IThrowable>().Equalize(damage, health, stayTime, speed, hasEvolved,
                            damageableLayer);
                        projectile1.GetComponent<Projectile>().enemiesHited = enemiesHited;
                        projectile2.GetComponent<Projectile>().enemiesHited = enemiesHited;
                    }
                }
            }
        }
    }
}