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
            IBullet.GetComponent<Projectile>().Equalize(weaponValues.damage, weaponValues.durability, weaponValues.stayTime, weaponValues.speed, false);
            StraightShoot(IBullet);
            IBullet.transform.eulerAngles = new Vector3(0, 0, Random.Range(IBullet.transform.eulerAngles.z - 15, IBullet.transform.eulerAngles.z + 15));
            if (bulletCount > 0)
            {
                Invoke("BulletThrow", 0.05f);
            }
            else if (bombCount == 1)
            {
                Invoke("BombThrow", 0.15f);
            }
        }

        void BombThrow()
        {
            bombCount--;
            GameObject Ibomba = Instantiate(bomb, transform);
            Ibomba.GetComponent<Bomb>().damage = weaponValues.damage * 2;
            Ibomba.GetComponent<Bomb>().speed = (int)weaponValues.speed * 2;
            Ibomba.GetComponent<Bomb>().stayTime = weaponValues.stayTime;
            StraightShoot(Ibomba);

            if (bulletCount > 0)
            {
                Invoke("BulletThrow", 0.05f);
            }
        }

        void StraightShoot(GameObject projectile)
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