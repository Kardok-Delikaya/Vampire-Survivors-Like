using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class ThrowingKnife : Weapons
    {
        PlayerManager player;
        int knifeCount;
        int vertical;
        int horizontal=1;

        [SerializeField] GameObject knife;
        [SerializeField] UpgradeData weapon;

        private void Start()
        {
            player = GetComponentInParent<PlayerManager>();
        }

        public override void Attack()
        {
            knifeCount = weaponValues.count;
            ThrowKnivesToMoveDirection();
        }

        void ThrowKnivesToMoveDirection()
        {
            GameObject spawnedKnife = Instantiate(knife, new Vector3(transform.position.x + Random.Range(-.5f, .5f), transform.position.y + Random.Range(-.75f, 1.25f), 0), transform.rotation);
            spawnedKnife.GetComponent<IThrowable>().Equalize(weaponValues.damage, weaponValues.durability, weaponValues.stayTime, weaponValues.speed, false, damageableLayer);

            if (player.pos.y > .2)
            {
                vertical = 1;
            }
            else if (player.pos.y < -.2)
            {
                vertical = -1;
            }
            else
            {
                vertical = 0;
            }

            if (player.pos.x > .2)
            {
                horizontal = 1;
            }
            else if (player.pos.x < -.2)
            {
                horizontal = -1;
            }
            else
            {
                horizontal = player.sprite.flipX ? -1 : 1;
            }

            if (vertical != 0 && Mathf.Abs(player.pos.x) < .2)
            {
                spawnedKnife.transform.localRotation = Quaternion.Euler(0, 0, vertical == 1 ? 0 : 180);
            }
            else
            {
                if (horizontal == 1)
                    spawnedKnife.transform.localRotation = Quaternion.Euler(0, 0, 180 + horizontal * 90 + vertical * 45);
                else
                    spawnedKnife.transform.localRotation = Quaternion.Euler(0, 0, 180 + horizontal * 90 - vertical * 45);
            }

            knifeCount--;

            if (knifeCount > 0)
            {
                Invoke("ThrowKnivesToMoveDirection", .1f);
            }
        }

        public override void Evolution()
        {
            FindAnyObjectByType<GameManager>().AddWeapon(weapon);
            Destroy(this.gameObject);
        }
    }
}