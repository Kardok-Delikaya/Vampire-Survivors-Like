using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class LittleFriend : Weapons
    {
        PlayerManager player;
        int vertical;
        int horizontal = 1;
        int bulletCount;
        int bombCount;
        [SerializeField] GameObject bullet;
        [SerializeField] GameObject bomb;

        private void Start()
        {
            player = GetComponentInParent<PlayerManager>();
        }

        public override void Attack()
        {
            bombCount = weaponValues.count;
            bulletCount = weaponValues.durability;

            if (bombCount > 1)
            {
                BombThrow();
            }
            else
            {
                BulletThrow();
            }
        }

        void BulletThrow()
        {
            bulletCount--;
            GameObject IBullet = Instantiate(bullet, transform);
            IBullet.GetComponent<Projectile>().Equalize(weaponValues.damage, 1, weaponValues.stayTime, weaponValues.speed, false, damageableLayer);
            ShootAtMoveDirection(IBullet);
            IBullet.transform.eulerAngles = new Vector3(0, 0, Random.Range(IBullet.transform.eulerAngles.z - 15, IBullet.transform.eulerAngles.z + 15));
            
            if (bulletCount > 0)
            {
                Invoke("BulletThrow", 0.05f);
            }
            else if (bombCount > 0)
            {
                Invoke("BombThrow", 0.15f);
            }
        }

        void BombThrow()
        {
            bombCount--;
            GameObject _bomb = Instantiate(bomb, transform);
            _bomb.GetComponent<Bomb>().Equalize(weaponValues.damage*2, 1, weaponValues.stayTime, weaponValues.speed/2, false, damageableLayer);
            ShootAtMoveDirection(_bomb);

            if (bulletCount > 0)
            {
                Invoke("BulletThrow", 0.05f);
            }
        }

        void ShootAtMoveDirection(GameObject projectile)
        {
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
                projectile.transform.localRotation = Quaternion.Euler(0, 0, vertical == 1 ? 0 : 180);
            }
            else
            {
                if (horizontal == 1)
                    projectile.transform.localRotation = Quaternion.Euler(0, 0, 180 + horizontal * 90 + vertical * 45);
                else
                    projectile.transform.localRotation = Quaternion.Euler(0, 0, 180 + horizontal * 90 - vertical * 45);
            }

        }

        public override void Evolution()
        {
            throw new System.NotImplementedException();
        }
    }
}