using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Books : Weapons
    {
        float time;
        float stayingTime;
        float destroyTime;
        int bookCount = 1;
        bool active;
        [SerializeField] Sprite sprite;
        [SerializeField] GameObject[] books;
        [SerializeField] List<UpgradeData> newUpgrades;
        private new void FixedUpdate()
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * -180 * weaponValues.speed);
            time -= Time.fixedDeltaTime;

            if (time <= 0f)
            {
                Attack();
            }

            if (stayingTime >= 0f)
            {
                stayingTime -= Time.fixedDeltaTime;
            }
            else if (active)
            {
                active = false;
                for (int i = 0; i < bookCount; i++)
                {
                    books[i].GetComponent<Book>().active = false;
                    books[i].GetComponent<Book>().backTime = 20;
                }
            }

            if (destroyTime > 0)
            {
                destroyTime -= Time.fixedDeltaTime;
            }
            else
            {
                for (int i = 0; i < weaponValues.count; i++)
                {
                    books[i].GetComponent<Book>().enemiesHasBeenShooted.Clear();
                }
                destroyTime = 1f / weaponValues.speed;
            }
        }
        public override void Attack()
        {
            transform.localScale = new Vector3(weaponValues.area, weaponValues.area, 1);
            bookCount = weaponValues.count;
            stayingTime = weaponValues.stayTime;
            time = weaponValues.timer;
            active = true;
            for (int i = 0; i < bookCount; i++)
            {
                books[i].GetComponent<Book>().active = true;
                books[i].GetComponent<Book>().damage = weaponValues.damage;
                books[i].GetComponent<Book>().area = weaponValues.area;
                books[i].GetComponent<Book>().exitTime = 20;
            }
        }
        public override void Evolution()
        {
            weaponValues.area += 0.5f;
            FindObjectOfType<GameManager>().AddToUpgradeList(newUpgrades);
        }
    }
}