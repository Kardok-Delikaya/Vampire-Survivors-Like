using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Bomb : MonoBehaviour
    {
        public int damage;
        public int speed;
        public float stayTime;
        void FixedUpdate()
        {
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

            stayTime -= Time.fixedDeltaTime;
            if (stayTime < 0)
            {
                Destroy(gameObject);
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
    }
}