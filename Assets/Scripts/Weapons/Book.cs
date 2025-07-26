using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Book : MonoBehaviour
    {
        Vector3 pos;

        public int damage;
        public float time;
        public int exitTime;
        public int backTime;
        public bool active;
        public float x, y;
        public float area;
        [HideInInspector] public List<Collider2D> enemiesHasBeenHitted;
        [HideInInspector] public LayerMask damageableLayer;

        void FixedUpdate()
        {
            if (active)
            {
                Collider2D[] damageableObjects = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size * area, 0, damageableLayer);
                if (damageableObjects.Length != 0)
                {
                    foreach (Collider2D damageable in damageableObjects)
                    {
                        if (!enemiesHasBeenHitted.Contains(damageable))
                        {
                            enemiesHasBeenHitted.Add(damageable);
                            damageable.GetComponent<IDamage>().TakeDamage(damage, 1);
                        }

                    }
                }
            }
            transform.rotation = Quaternion.Euler(pos);
            if (exitTime > 0f)
            {
                Move();
            }
            if (backTime > 0f)
            {
                ReverseMove();
            }
        }
        public void Move()
        {
            transform.localPosition += new Vector3(2.5f * x / 20, 2.5f * y / 20, 0);
            transform.localScale += new Vector3(0.05f, 0.05f, 0);
            exitTime--;
        }
        public void ReverseMove()
        {
            transform.localPosition -= new Vector3(2.5f * x / 20, 2.5f * y / 20, 0);
            transform.localScale -= new Vector3(0.05f, 0.05f, 0);
            backTime--;
        }
    }
}