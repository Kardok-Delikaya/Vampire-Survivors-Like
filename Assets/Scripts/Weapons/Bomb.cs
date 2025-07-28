using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Bomb : MonoBehaviour, IThrowable
    {
        int damage;
        float speed;
        float stayTime;
        LayerMask damageableLayer;

        void FixedUpdate()
        {
            stayTime -= Time.fixedDeltaTime;
            if (stayTime < 0)
                Destroy(gameObject);

            transform.position += transform.up * Time.deltaTime * speed;

            Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, .3f);

            if (objs.Length!=0)
            {
                for(int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].GetComponent<IDamage>() != null)
                    {
                        BlowUp();
                        break;
                    }
                }
            }
            
        }
        void BlowUp()
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (Collider2D c in enemies)
            {
                if (c.GetComponent<IDamage>() != null)
                {
                    IDamage obj = c.GetComponent<IDamage>();
                    obj.TakeDamage(damage, 1);
                }
            }
            Destroy(gameObject);
        }

        public void Equalize(int damage, int health, float stayTime, float speed, bool hasEvolved, LayerMask damageableLayer)
        {
            this.damage = damage;
            this.stayTime = stayTime;
            this.speed = speed;
            this.damageableLayer = damageableLayer;
        }
    }
}