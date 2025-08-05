using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    private Vector3 pos;

    public int damage;
    public float time;
    public int exitTime;
    public int backTime;
    public bool active;
    public float x, y;
    public float area;
    [HideInInspector] public List<Collider2D> enemiesHasBeenHitted;
    [HideInInspector] public LayerMask damageableLayer;

    private void FixedUpdate()
    {
        if (active)
        {
            var damageableObjects = Physics2D.OverlapBoxAll(transform.position,
                GetComponent<BoxCollider2D>().size * area, 0, damageableLayer);
            if (damageableObjects.Length != 0)
            {
                foreach (var damageable in damageableObjects)
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