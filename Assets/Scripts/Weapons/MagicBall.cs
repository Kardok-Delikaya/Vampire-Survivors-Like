using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class MagicBall : MonoBehaviour, IThrowable
    {
        public bool hasEvolved;
        public int damage;
        public float speed;
        int count;
        public float stayingTime;
        public List<Collider2D> hittedEnemies;
        void FixedUpdate()
        {
            stayingTime -= Time.fixedDeltaTime;
            transform.position += transform.up * Time.deltaTime * 15 * speed;
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
            if (stayingTime < 0)
            {
                Destroy(gameObject);
            }
            if (hasEvolved)
            {
                CheckLines();
            }
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
        public void Equalize(int damage, int can, float stayingTime, float speed, bool hasEvolved)
        {
            this.damage = damage;
            this.stayingTime = stayingTime;
            this.speed = speed;
            this.hasEvolved = hasEvolved;
        }
    }
}