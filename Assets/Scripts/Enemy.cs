using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Enemy : MonoBehaviour, IDamage
    {
        Player player;
        Rigidbody2D rb;
        GameManager gameManager;
        Spawner spawner;
        float timer1, timer2;
        bool active;
        int force;

        public float health;
        [SerializeField] float speed;
        [SerializeField] int damage;
        [SerializeField] GameObject damageMessage;
        [SerializeField] int wide;
        [SerializeField] int xpRewardCount;

        void Awake()
        {
            player = FindObjectOfType<Player>();
            gameManager = FindObjectOfType<GameManager>();
            spawner=FindObjectOfType<Spawner>();
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if (timer2 <= 0)
            {
                Movemment();
                FarDestroy();
            }
            else
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                rb.linearVelocity = -direction * force * 5;
                timer2 -= Time.fixedDeltaTime;
            }
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject==player.gameObject)
            {
                if (!active)
                {
                    Damage();
                    timer1 = 1;
                    active = true;
                }
                else
                {
                    timer1 -= Time.deltaTime;
                    if (timer1 <= 0)
                    {
                        active = false;
                    }
                }
            }
        }
        void Movemment()
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
            if (player.transform.position.x - transform.position.x > 0)
            {
                transform.localScale = new Vector3(wide, wide, 1);
            }
            else if (player.transform.position.x - transform.position.x < 0)
            {
                transform.localScale = new Vector3(-wide, wide, 1);
            }
        }
        void Damage()
        {
            player.TakeDamage(Random.Range(damage * 3 / 5, damage * 6 / 5));
        }
        public void TakeDamage(int damage, int power)
        {
            int damageReceived = Random.Range(damage * 4 / 5, damage * 6 / 5);
            health -= damageReceived;

            GameObject HasarMesaj = Instantiate(damageMessage, transform.position, transform.rotation) as GameObject;
            HasarMesaj.transform.parent = null;
            HasarMesaj.GetComponentInChildren<TMPro.TextMeshPro>().text = damageReceived + "";

            if (health <= 0)
            {
                Death();
            }

            if (wide == 1)
            {
                force = power;
                timer2 = .2f;
            }
        }
        void Death()
        {
            gameManager.Kill();
            player.SpawnXP(transform, xpRewardCount);
            spawner.enemyCount--;
            Destroy(gameObject);
        }

        void FarDestroy()
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > 50)
            {
                Destroy(gameObject);
            }
        }
    }
}