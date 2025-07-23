using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VSLike
{
    public class Player : MonoBehaviour
    {
        Animator anim;
        Rigidbody2D rb;
        InputHandler inputHandler;
        Vector2 pos;
        float waitingTimer;

        [Header("Character Stats")]
        public int maxHealth;
        public int health;
        public int shield;
        public float armor;
        public float speed;
        public int regenerate;
        public float magnet;
        [SerializeField] Items persistantUpgrade;

        [Header("HUD")]
        [SerializeField] GameObject healthBar;
        [SerializeField] GameObject shieldBar;
        [SerializeField] GameObject damageMessage;

        [Header("XP")]
        public int xpCount;
        [SerializeField] GameObject xp;
        public Item spawnedRedXP;

        void Start()
        {
            GetPersistantUpgrades();
            health = maxHealth;
            anim = GetComponent<Animator>();
            inputHandler = GetComponent<InputHandler>();
            rb = GetComponent<Rigidbody2D>();
            pos = new Vector3();
        }
        void Update()
        {
            float delta = Time.deltaTime;
            inputHandler.TickInput(delta);
            Movemment();
        }

        private void FixedUpdate()
        {
            GetItems();
            if (regenerate > 0)
                Regenerate();
        }

        void Movemment()
        {
            pos.x = inputHandler.horizontal;
            pos.y = inputHandler.vertical;
            if (pos.x > .1)
            {
                anim.SetFloat("Idle", 1);
            }
            else if (pos.x < -.1)
            {
                anim.SetFloat("Idle", -1);
            }
            rb.linearVelocity = new Vector2(pos.x * speed, pos.y * speed);
            anim.SetFloat("Hız", pos.sqrMagnitude);
        }

        void GetItems()
        {
            Collider2D[] objs;
            objs = Physics2D.OverlapCircleAll(transform.position, magnet);
            if (objs.Length > 0)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].GetComponent<Item>() != null)
                    {
                        Item obj = objs[i].GetComponent<Item>();
                        Vector3 direction = (transform.position - objs[i].transform.position).normalized;
                        objs[i].GetComponent<Rigidbody2D>().linearVelocity = direction * 20;
                        if (Vector3.Distance(objs[i].transform.position, transform.position) < .3f)
                        {
                            AddItem(obj.id, obj.count);
                            Destroy(obj.gameObject);
                        }
                    }
                }
            }
        }

        void AddItem(int id, int count)
        {
            switch (id)
            {
                case 0:
                    FindObjectOfType<GameManager>().GetXP(count);
                    xpCount--;
                    break;
                case 1:
                    GetHealth(count);
                    break;
                case 2:
                    FindObjectOfType<GameManager>().GetGold();
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
                    FindObjectOfType<GameManager>().Death();
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
            FindObjectOfType<GameManager>().characterSpecs[1].text = shield + "";
            Message("-" + tempDamage, Color.blue);
            ShieldValue();
        }

        void Armor(ref float hasar)
        {
            hasar -= armor;
            if (hasar < 1)
            {
                hasar = 1;
            }
        }

        public void ShieldValue()
        {
            shieldBar.transform.localScale = new Vector3((float)shield / 100, 1, 1);
        }
        
        public void GetHealth(int x)
        {
            health += x;
            if (health > maxHealth) health = maxHealth;
            healthBar.transform.localScale = new Vector3((float)health / maxHealth, 1, 1);
            Message("+" + x, Color.green);
        }

        public void GetSield(int x)
        {
            shield += x;
            if (shield > 100) shield = 100;

        }

        public void Message(string yazi, Color renk)
        {
            GameObject HasarMesaj = Instantiate(damageMessage, transform.position, transform.rotation) as GameObject;
            HasarMesaj.transform.parent = null;
            HasarMesaj.GetComponentInChildren<TMPro.TextMeshPro>().text = yazi;
            HasarMesaj.GetComponentInChildren<TMPro.TextMeshPro>().color = renk;
        }

        void Regenerate()
        {
            if (waitingTimer <= 0)
            {
                if (health < maxHealth)
                {
                    GetHealth(regenerate);
                    waitingTimer = 5;
                }
            }
            else
            {
                waitingTimer -= Time.fixedDeltaTime;
            }
        }

        public void SpawnXP(Transform transform,int xpRewardCount)
        {
            xpCount++;

            if (xpCount > 50)
            {
                if (spawnedRedXP != null)
                {
                    spawnedRedXP.count += xpRewardCount;
                }
                else
                {
                    GameObject XP = Instantiate(xp, transform.position, transform.rotation);
                    XP.GetComponent<Item>().count = xpRewardCount;
                    XP.GetComponent<SpriteRenderer>().color = Color.red;
                    spawnedRedXP = XP.GetComponent<Item>();
                }
            }
            else
            {
                GameObject XP = Instantiate(xp, transform.position, transform.rotation);
                XP.GetComponent<Item>().count = xpRewardCount;
            }
        }

        void GetPersistantUpgrades()
        {
            maxHealth += persistantUpgrade.values.maxHealth;
            armor+=persistantUpgrade.values.armor;
            speed+=persistantUpgrade.values.speed;
            regenerate+=persistantUpgrade.values.regenerate;
            magnet+=persistantUpgrade.values.magnet;
        }
    }
}