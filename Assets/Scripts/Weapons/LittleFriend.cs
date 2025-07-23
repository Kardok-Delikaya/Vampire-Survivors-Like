using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class LittleFriend : Weapons
    {
        [SerializeField] GameObject bullet, bomb;
        [SerializeField] int bulletCount, bombCount;
        Animator anim;
        private void Start()
        {
            anim = GetComponentInParent<Animator>();
        }
        public override void Attack()
        {
            bombCount = (int)weaponValues.speed;
            bulletCount = weaponValues.durability;
            if (bombCount == 2)
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
            IBullet.GetComponent<Projectile>().damage = weaponValues.damage;
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
            Ibomba.GetComponent<Bomb>().stayTime = weaponValues.stayTime;
            StraightShoot(Ibomba);
            if (bulletCount > 0)
            {
                Invoke("BulletThrow", 0.05f);
            }
        }
        void StraightShoot(GameObject projectile)
        {
            int vertical;
            int horizontal;
            if (GetComponentInParent<InputHandler>().vertical > .2)
            {
                vertical = 1;
            }
            else if (GetComponentInParent<InputHandler>().vertical < -.2)
            {
                vertical = -1;
            }
            else
            {
                vertical = 0;
            }
            if (GetComponentInParent<InputHandler>().horizontal > .2)
            {
                horizontal = 1;
            }
            else if (GetComponentInParent<InputHandler>().horizontal < -.2)
            {
                horizontal = -1;
            }
            else
            {
                horizontal = 0;
            }
            if (vertical != 0 || horizontal != 0)
            {
                if (vertical == -1 && horizontal == 0)
                {
                    projectile.transform.eulerAngles = new Vector3(0, 0, 180);
                }
                else
                {
                    projectile.transform.right = new Vector2(vertical, -horizontal);
                }
            }
            else
            {
                projectile.transform.eulerAngles = new Vector3(0, 0, -anim.GetFloat("Idle") * 90);
            }
        }

        public override void Evolution()
        {
            throw new System.NotImplementedException();
        }
    }
}