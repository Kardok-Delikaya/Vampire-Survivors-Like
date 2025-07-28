using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class MagicBall : MonoBehaviour, IThrowable
    {
        List<Collider2D> hittedEnemies=new List<Collider2D>();
        int checkCount;
        int damage;
        float speed;
        float stayTime; bool hasEvolved;
        LayerMask damageableLayer;

        void FixedUpdate()
        {
            stayTime -= Time.fixedDeltaTime;
            if (stayTime < 0)
                Destroy(gameObject);
            
            transform.position += transform.up * speed * Time.deltaTime;

            Collider2D[] damageableObjects = Physics2D.OverlapCircleAll(transform.position, .3f,damageableLayer);

            if (damageableObjects.Length != 0)
            {
                foreach (Collider2D objects in damageableObjects)
                {
                    IDamage damageable = objects.GetComponent<IDamage>();

                    if (!hittedEnemies.Contains(objects))
                    {
                        hittedEnemies.Add(objects);
                        damageable.TakeDamage(damage, 1);
                    }
                }
            }
            
            if (hasEvolved)
                CheckLines();
        }

        void CheckLines()
        {
            if (transform.localPosition.y > 9 && checkCount != 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180 - transform.eulerAngles.z);
                checkCount = 1;
                hittedEnemies.Clear();
            }
            else if (transform.localPosition.y < -9 && checkCount != 2)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180 - transform.eulerAngles.z);
                checkCount = 2;
                hittedEnemies.Clear();
            }
            else if (transform.localPosition.x > 18 && checkCount != 3)
            {
                transform.rotation = Quaternion.Euler(0, 0, -transform.eulerAngles.z);
                checkCount = 3;
                hittedEnemies.Clear();
            }
            else if (transform.localPosition.x < -18 && checkCount != 4)
            {
                transform.rotation = Quaternion.Euler(0, 0, -transform.eulerAngles.z);
                checkCount = 4;
                hittedEnemies.Clear();
            }
        }

        public void Equalize(int damage, int health, float stayingTime, float speed, bool hasEvolved, LayerMask damageableLayer)
        {
            this.damage = damage;
            this.stayTime = stayingTime;
            this.speed = speed;
            this.hasEvolved = hasEvolved;
            this.damageableLayer = damageableLayer;
        }
    }
}