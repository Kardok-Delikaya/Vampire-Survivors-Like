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
        SpriteRenderer sprite;
        Spawner spawner;
        float attackCoolDown;
        bool active;
        bool beingPushed;
        int force;
        Vector3 direction;

        [SerializeField] float health;
        [SerializeField] float speed;
        [SerializeField] int damage;
        [SerializeField] GameObject damagePopUp;
        [SerializeField] int xpRewardCount;
        [SerializeField] bool canBePushed;

        void Awake()
        {
            sprite=GetComponent<SpriteRenderer>();
            player = FindAnyObjectByType<Player>();
            gameManager = FindAnyObjectByType<GameManager>();
            spawner=FindAnyObjectByType<Spawner>();
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            direction = (player.transform.position - transform.position).normalized;

            FarDestroy();

            if (beingPushed)
                return;

            Movement();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject==player.gameObject)
            {
                if (!active)
                {
                    Damage();
                    attackCoolDown = 1;
                    active = true;
                }
                else
                {
                    attackCoolDown -= Time.deltaTime;
                    if (attackCoolDown <= 0)
                    {
                        active = false;
                    }
                }
            }
        }

        void Movement()
        {
            rb.linearVelocity = direction * speed;

            if (player.transform.position.x - transform.position.x > 0)
            {
                sprite.flipX = false;
            }
            else if (player.transform.position.x - transform.position.x < 0)
            {
                sprite.flipX = true;
            }
        }

        void Damage()
        {
            player.TakeDamage(damage);
        }

        public void TakeDamage(int damage, int power)
        {
            health -= damage;

            GameObject DamagePopUp = Instantiate(damagePopUp, transform.position, transform.rotation) as GameObject;
            DamagePopUp.transform.parent = null;
            DamagePopUp.GetComponentInChildren<TMPro.TextMeshPro>().text = damage+"";

            if (health <= 0)
            {
                Death();
                return;
            }

            if (canBePushed)
            {
                StartCoroutine(GetPushedBack(power));
            }
        }

        IEnumerator GetPushedBack(int power)
        {
            beingPushed = true;
            rb.linearVelocity = -direction * speed*power;

            yield return new WaitForSeconds(.2f);

            beingPushed = false;
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