using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace VSLike
{
    public class PlayerManager : MonoBehaviour
    {
        Animator anim;
        Rigidbody2D rb;
        GameManager gameManager;
        [HideInInspector] public SpriteRenderer sprite;
        [HideInInspector] public Vector2 pos = new Vector2(0, 0);

        [Header("Character Stats")]
        public int maxHealth;
        public int shield;
        public float armor;
        public float speed;
        public int regenerate;
        public float magnet;
        [SerializeField] Items persistantUpgrade;
        int health;
        float healthRegenerateTimer;

        [Header("HUD")]
        [SerializeField] GameObject healthBar;
        [SerializeField] GameObject shieldBar;
        [SerializeField] GameObject damagePopUp;

        [Header("XP")]
        public int xpCount;
        [SerializeField] GameObject xp;
        public LootableObject spawnedRedXP;

        [Header("Layers")]
        [SerializeField] LayerMask lootLayer;

        void Start()
        {
            gameManager=FindAnyObjectByType<GameManager>();
            sprite = GetComponent<SpriteRenderer>();
            GetPersistantUpgrades();
            health = maxHealth;
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            CollectNearbyLoots();

            if (regenerate > 0)
                Regenerate();
        }

        public void HandleMovement(InputAction.CallbackContext context)
        {
            pos.x = context.ReadValue<Vector2>().x;
            pos.y = context.ReadValue<Vector2>().y;

            if (pos.x > .1 && sprite.flipX)
            {
                sprite.flipX = false;
            }

            if (pos.x < -.1 && !sprite.flipX)
            {
                sprite.flipX = true;
            }

            rb.linearVelocity = new Vector2(pos.x * speed, pos.y * speed);
            anim.SetFloat("Speed", pos.sqrMagnitude);
        }

        void CollectNearbyLoots()
        {
            Collider2D[] loots;
            loots = Physics2D.OverlapCircleAll(transform.position, magnet, lootLayer);

            if (loots.Length > 0)
            {
                for (int i = 0; i < loots.Length; i++)
                {
                    LootableObject loot = loots[i].GetComponent<LootableObject>();
                    loot.transform.position = Vector3.MoveTowards(loot.transform.position, transform.position, 0.1f * magnet);

                    if (Vector3.Distance(loots[i].transform.position, transform.position) < .3f)
                    {
                        AddLoot(loot.id, loot.count);
                        Destroy(loot.gameObject);
                    }
                }
            }
        }

        void AddLoot(int id, int count)
        {
            switch (id)
            {
                case 0:
                    gameManager.GetXP(count);
                    xpCount--;
                    break;
                case 1:
                    GetHealth(10);
                    break;
                case 2:
                    gameManager.GetGold();
                    break;
                default:
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

            if (damage > 0)
            {
                Armor(ref damage);

                health -= (int)damage;

                if (health <= 0)
                {
                    gameManager.Death();
                }

                healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1, 1);
                Message("-" + (int)damage, Color.red);
            }
        }

        void Shield(ref float damage)
        {
            float tempDamage = damage;
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

            gameManager.playerSpecTexts[1].text = shield + "";
            Message("-" + tempDamage, Color.blue);
            ShieldValue();
        }

        void Armor(ref float damage)
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

        public void GetHealth(int healAmount)
        {
            health += healAmount;
            if (health > maxHealth) health = maxHealth;
            healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1, 1);
            Message("+" + healAmount, Color.green);
        }

        public void GetSield(int shieldAmount)
        {
            shield += shieldAmount;
            if (shield > 100) shield = 100;

        }

        public void Message(string message, Color color)
        {
            GameObject messagePopUp = Instantiate(damagePopUp, transform.position, transform.rotation) as GameObject;
            messagePopUp.transform.parent = null;
            messagePopUp.GetComponentInChildren<TMPro.TextMeshPro>().text = message;
            messagePopUp.GetComponentInChildren<TMPro.TextMeshPro>().color = color;
        }

        void Regenerate()
        {
            if (healthRegenerateTimer <= 0)
            {
                if (health < maxHealth)
                {
                    GetHealth(regenerate);
                    healthRegenerateTimer = 5;
                }
            }
            else
            {
                healthRegenerateTimer -= Time.fixedDeltaTime;
            }
        }

        public void SpawnXP(Transform transform, int xpRewardCount)
        {
            if (xpCount > 50)
            {
                if (spawnedRedXP != null)
                {
                    spawnedRedXP.count += xpRewardCount;
                }
                else
                {
                    GameObject XP = Instantiate(xp, transform.position, transform.rotation);
                    XP.GetComponent<LootableObject>().count = xpRewardCount;
                    XP.GetComponent<SpriteRenderer>().color = Color.red;
                    spawnedRedXP = XP.GetComponent<LootableObject>();
                    xpCount++;
                }
            }
            else
            {
                GameObject XP = Instantiate(xp, transform.position, transform.rotation);
                XP.GetComponent<LootableObject>().count = xpRewardCount;
                xpCount++;
            }
        }

        void GetPersistantUpgrades()
        {
            maxHealth += persistantUpgrade.values.maxHealth;
            armor += persistantUpgrade.values.armor;
            speed += persistantUpgrade.values.speed;
            regenerate += persistantUpgrade.values.regenerate;
            magnet += persistantUpgrade.values.magnet;
        }
    }
}