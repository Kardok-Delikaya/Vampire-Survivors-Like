using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Books : Weapons
    {
        float attackCoolDown;
        float stayTime;
        float rotationTimer;
        int bookCount = 1;
        bool active;
        [SerializeField] Sprite sprite;
        [SerializeField] Book[] books;
        [SerializeField] List<UpgradeData> newUpgrades;

        private void Awake()
        {
            for (int i = 0; i < books.Length; i++)
                books[i].damageableLayer = damageableLayer;
        }
        private new void FixedUpdate()
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * -180 * weaponValues.speed);
            attackCoolDown -= Time.fixedDeltaTime;

            if (attackCoolDown <= 0f)
            {
                Attack();
            }

            if (stayTime >= 0f)
            {
                stayTime -= Time.fixedDeltaTime;
            }
            else if (active)
            {
                active = false;
                for (int i = 0; i < bookCount; i++)
                {
                    books[i].active = false;
                    books[i].backTime = 20;
                }
            }

            if (rotationTimer > 0)
            {
                rotationTimer -= Time.fixedDeltaTime;
            }
            else
            {
                for (int i = 0; i < weaponValues.count; i++)
                {
                    books[i].enemiesHasBeenHitted.Clear();
                }
                rotationTimer = 1f / weaponValues.speed;
            }
        }
        public override void Attack()
        {
            transform.localScale = new Vector3(weaponValues.area, weaponValues.area, 1);
            bookCount = weaponValues.count;
            stayTime = weaponValues.stayTime;
            attackCoolDown = weaponValues.timer;
            active = true;
            for (int i = 0; i < bookCount; i++)
            {
                books[i].active = true;
                books[i].damage = weaponValues.damage;
                books[i].area = weaponValues.area;
                books[i].exitTime = 20;
            }
        }
        public override void Evolution()
        {
            weaponValues.area += 0.5f;
            FindAnyObjectByType<GameManager>().AddToUpgradeList(newUpgrades);
        }
    }
}