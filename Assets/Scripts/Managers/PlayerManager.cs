using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PasiveItems))]
[RequireComponent(typeof(WeaponManager))]
public class PlayerManager : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    [Header("Character Stats")]
    [SerializeField] private Items playerStats;
    [SerializeField] private Items persistantUpgrade;
    public int maxHealth { get;private set; }
    public int shield { get;private set; }
    public float armor { get;private set; }
    public float speed { get;private set; }
    public int regenerate { get;private set; }
    public float magnet { get;private set; }
    
    private int health;
    private float healthRegenerateTimer;

    [Header("HUD")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject shieldBar;
    [SerializeField] private GameObject damagePopUp;
    
    [Header("Layers")]
    [SerializeField] private LayerMask lootLayer;

    private int speedMash = Animator.StringToHash("Speed");

    public bool IsLeft()
    {
        return sprite.flipX;
    }

    public Vector2 Pos
    {
        get;
        private set;
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        GetStatUpgrades(playerStats);
        GetStatUpgrades(persistantUpgrade);
        health = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
        CollectNearbyLoots();

        if (regenerate > 0)
            Regenerate();
    }

    private void Movement()
    {
        rb.linearVelocity = new Vector2(Pos.x * speed, Pos.y * speed);

        anim.SetFloat(speedMash, Pos.sqrMagnitude);
    }
    
    public void HandleMovement(InputAction.CallbackContext context)
    {
        Pos = context.ReadValue<Vector2>();

        if (Pos.x > .1 && sprite.flipX)
        {
            sprite.flipX = false;
        }

        if (Pos.x < -.1 && !sprite.flipX)
        {
            sprite.flipX = true;
        }
    }

    private void CollectNearbyLoots()
    {
        var loots = Physics2D.OverlapCircleAll(transform.position, magnet, lootLayer);

        for (var i = 0; i < loots.Length; i++)
        {
            var loot = loots[i].GetComponent<LootableObject>();
            loot.transform.position = Vector3.MoveTowards(loot.transform.position, transform.position, magnet/10);

            if (Vector2.Distance(loots[i].transform.position, transform.position) < .3f)
            {
                AddLoot(loot);
                loot.gameObject.SetActive(false);
            }
        }
    }

    private void AddLoot(LootableObject loot)
    {
        switch (loot.id)
        {
            case 0:
                GameManager.Instance.GetXP(loot.count);
                break;
            case 1:
                GetHealth(10);
                break;
            case 2:
                GameManager.Instance.GetGold(Random.Range(10,50));
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) damage = 1;

        if (shield > 0)
        {
            Shield(ref damage);
        }

        if (!(damage > 0)) return;
        
        Armor(ref damage);

        health -= (int)damage;

        if (health <= 0)
        {
            GameManager.Instance.uiManager.OpenPauseMenu(true);
        }

        healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1, 1);
        Message($"-{(int)damage}", Color.red);
    }

    private void Shield(ref float damage)
    {
        var tempDamage = damage;
        damage -= shield;
        shield -= (int)tempDamage;

        if (damage < 0)
        {
            damage = 0;
        }
        if (shield < 0)
        {
            shield = 0;
        }

        Message($"-{tempDamage}", Color.blue);
        ShieldValue();
    }

    private void Armor(ref float damage)
    {
        damage -= armor;

        if (damage < 1)
        {
            damage = 1;
        }
    }

    public void ShieldValue()
    {
        shieldBar.transform.localScale = new Vector3((float)shield / 100, 1, 1);
    }

    private void GetHealth(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth) health = maxHealth;
        healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1, 1);
        Message($"+{healAmount}", Color.green);
    }

    public void Message(string message, Color color)
    {
        var messagePopUp = Instantiate(damagePopUp, transform.position, transform.rotation) as GameObject;
        messagePopUp.transform.SetParent(null);
        messagePopUp.GetComponentInChildren<TMPro.TextMeshPro>().text = message;
        messagePopUp.GetComponentInChildren<TMPro.TextMeshPro>().color = color;
    }

    private void Regenerate()
    {
        healthRegenerateTimer -= Time.deltaTime;
        
        if (healthRegenerateTimer <= 0)
        {
            GetHealth(regenerate);
            healthRegenerateTimer = 5;
        }
    }

    public void GainShield(int shieldAmount)
    {
        shield += shieldAmount;
        if (shield > 100) shield = 100;
        
        Message($"+{shieldAmount}", Color.blue);
    }

    public void GetStatUpgrades(Items upgrade)
    {
        maxHealth += upgrade.values.maxHealth;
        armor += upgrade.values.armor;
        speed += upgrade.values.speed;
        regenerate += upgrade.values.regenerate;
        magnet += upgrade.values.magnet;
    }
}