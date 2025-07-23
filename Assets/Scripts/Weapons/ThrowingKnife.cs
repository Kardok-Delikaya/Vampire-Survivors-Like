using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class ThrowingKnife : Weapons
    {
        Animator anim;
        [SerializeField] GameObject knife;
        [SerializeField] UpgradeData weapon;
        int knifeCount;
        private void Start()
        {
            anim = GetComponentInParent<Animator>();
        }
        public override void Attack()
        {
            knifeCount = weaponValues.count;
            KnifeThrow();
        }
        void KnifeThrow()
        {
            GameObject spawnedKnife = Instantiate(knife, new Vector3(transform.position.x + Random.Range(-.5f, .5f), transform.position.y + Random.Range(-.75f, 1.25f), 0), transform.rotation);
            spawnedKnife.GetComponent<IThrowable>().Equalize(weaponValues.damage, weaponValues.durability, weaponValues.stayTime,0, false);
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
                    spawnedKnife.transform.eulerAngles = new Vector3(0, 0, 180);
                }
                else
                {
                    spawnedKnife.transform.right = new Vector2(vertical, -horizontal);
                }
            }
            else
            {
                spawnedKnife.transform.eulerAngles = new Vector3(0, 0, -anim.GetFloat("Idle") * 90);
            }
            knifeCount--;
            if (knifeCount > 0)
            {
                Invoke("KnifeThrow", .1f);
            }
        }

        public override void Evolution()
        {
            FindObjectOfType<GameManager>().AddWeapon(weapon);
            Destroy(this.gameObject);
        }
    }
}