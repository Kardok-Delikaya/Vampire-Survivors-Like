using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class MagicBall : MonoBehaviour, IThrowable
    {
        int count;
        List<Collider2D> hittedEnemies=new List<Collider2D>();

        bool hasEvolved;
        int damage;
        float speed;
        float stayTime;

        void FixedUpdate()
        {
            stayTime -= Time.fixedDeltaTime;
            if (stayTime < 0)
                Destroy(gameObject);
            

            transform.position += transform.up * speed * Time.deltaTime;

            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, .3f);

            if (enemies.Length!=0)
            {
                foreach (Collider2D c in enemies)
                {
                    if (c.GetComponent<IDamage>() != null)
                    {
                        IDamage obje = c.GetComponent<IDamage>();
                        if (obje != null && !hittedEnemies.Contains(c))
                        {
                            hittedEnemies.Add(c);
                            obje.TakeDamage(damage, 1);
                        }

                    }
                }
            }
            
            if (hasEvolved)
                CheckLines();
            
        }

        void CheckLines()
        {
            if (transform.localPosition.y > 9 && count != 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180 - transform.eulerAngles.z);
                count = 1;
                hittedEnemies.Clear();
            }
            else if (transform.localPosition.y < -9 && count != 2)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180 - transform.eulerAngles.z);
                count = 2;
                hittedEnemies.Clear();
            }
            else if (transform.localPosition.x > 18 && count != 3)
            {
                transform.rotation = Quaternion.Euler(0, 0, -transform.eulerAngles.z);
                count = 3;
                hittedEnemies.Clear();
            }
            else if (transform.localPosition.x < -18 && count != 4)
            {
                transform.rotation = Quaternion.Euler(0, 0, -transform.eulerAngles.z);
                count = 4;
                hittedEnemies.Clear();
            }
        }
        public void Equalize(int damage, int health, float stayingTime, float speed, bool hasEvolved)
        {
            this.damage = damage;
            this.stayTime = stayingTime;
            this.speed = speed;
            this.hasEvolved = hasEvolved;
        }
    }
}