using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyManager : MonoBehaviour, IDamage
{
    private Vector3 direction;
    private PlayerManager player;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private float attackCoolDown;
    private bool beingPushed;

    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private GameObject damagePopUp;
    [SerializeField] private int xpRewardCount;
    [SerializeField] private bool canBePushed;

    [Header("Event")]
    public UnityEvent OnDeath;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite=GetComponent<SpriteRenderer>();
        player = GameManager.Instance.player;
    }

    private void FixedUpdate()
    {
        FarDestroy();
        direction = (player.transform.position - transform.position).normalized;
        attackCoolDown-= Time.deltaTime;

        if (beingPushed)
            return;

        Movement();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject==player.gameObject)
        {
            if (attackCoolDown <= 0)
            {
                player.TakeDamage(damage);
                attackCoolDown = 1;
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

    public void TakeDamage(int damage, int power)
    {
        health -= damage;

        if (health > 0)
        {
            var DamagePopUp = Instantiate(damagePopUp, transform.position, transform.rotation) as GameObject;
            DamagePopUp.transform.SetParent(null);
            DamagePopUp.GetComponentInChildren<TMPro.TextMeshPro>().text = damage + "";

            if (canBePushed)
            {
                StartCoroutine(GetPushedBack(power));
            }
        }
        else
        {
            HandleDeath();
            Destroy(gameObject);
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
        GameManager.Instance.SpawnXP(transform,xpRewardCount);
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