using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace VSLike
{
    public class Projectile : MonoBehaviour, IThrowable
    {
        List<Collider2D> enemiesHited = new List<Collider2D>();
        bool hasEvolved;
        int damage;
        int health;
        float speed;
        float stayTime;

        [SerializeField] GameObject arrow;

        public void Equalize(int damage, int health, float stayTime, float speed, bool hasEvolved)
        {
            this.damage = damage;
            this.health = health;
            this.speed = speed;
            this.hasEvolved = hasEvolved;
            this.stayTime = stayTime;
        }

        void FixedUpdate()
        {
            stayTime -= Time.fixedDeltaTime;
            if (stayTime < 0)
                Destroy(gameObject);


            transform.position += transform.up * speed * Time.deltaTime;

            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, .3f);

            if (enemies.Length > 0)
            {
                foreach (Collider2D enemy in enemies)
                {
                    if (enemy.GetComponent<IDamage>() != null && health != 0)
                    {
                        IDamage obj = enemy.GetComponent<IDamage>();

                        if (obj != null && !enemiesHited.Contains(enemy))
                        {
                            enemiesHited.Add(enemy);

                            health -= 1;

                            obj.TakeDamage(damage, 1);

                            if (health == 0)
                            {
                                Destroy(gameObject);
                            }
                            else if (hasEvolved&&arrow!=null)
                            {
                                GameObject projectile1 = Instantiate(arrow, transform.localPosition, transform.localRotation);
                                GameObject projectile2 = Instantiate(arrow, transform.localPosition, transform.localRotation);
                                projectile1.transform.SetParent(null);
                                projectile2.transform.SetParent(null);
                                projectile1.GetComponent<Projectile>().enemiesHited = enemiesHited;
                                projectile2.GetComponent<Projectile>().enemiesHited = enemiesHited;
                                projectile1.transform.eulerAngles = new Vector3(0, 0, projectile1.transform.eulerAngles.z + 15);
                                projectile2.transform.eulerAngles = new Vector3(0, 0, projectile2.transform.eulerAngles.z - 15);
                            }
                        }
                    }
                }
            }
        }
    }
}