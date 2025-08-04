using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyManager : MonoBehaviour, IDamage
{
    private Vector3 direction;
    private PlayerManager player;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private float attackCoolDown;
    private bool active;
    private bool beingPushed;

    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private GameObject damagePopUp;
    [SerializeField] private int xpRewardCount;
    [SerializeField] private bool canBePushed;

    private void Awake()
    {
        sprite=GetComponent<SpriteRenderer>();
        player = GameManager.Instance.player;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
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

    private void Movement()
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

    private void Damage()
    {
        player.TakeDamage(damage);
    }

    public void TakeDamage(int damage, int power)
    {
        health -= damage;

        var DamagePopUp = Instantiate(damagePopUp, transform.position, transform.rotation) as GameObject;
        DamagePopUp.transform.SetParent(null);
        DamagePopUp.GetComponentInChildren<TMPro.TextMeshPro>().text = damage+"";

        if (health <= 0)
        {
            HandleDeath();
            return;
        }

        if (canBePushed)
        {
            StartCoroutine(GetPushedBack(power));
        }
    }

    private IEnumerator GetPushedBack(int power)
    {
        beingPushed = true;
        rb.linearVelocity = -direction * speed*power;

        yield return new WaitForSeconds(.2f);

        beingPushed = false;
    }

    private void HandleDeath()
    {
        GameManager.Instance.HandleKill();
        player.SpawnXP(transform, xpRewardCount);
        GameManager.Instance.spawner.enemyCount--;
        Destroy(gameObject);
    }

    private void FarDestroy()
    {
        var distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > 50)
        {
            Destroy(gameObject);
        }
    }
}