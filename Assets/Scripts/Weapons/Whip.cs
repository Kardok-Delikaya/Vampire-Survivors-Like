using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Whip : Weapons
    {
        int attackCount;
        Animator anim;
        [SerializeField] GameObject[] whips;
        [SerializeField] List<UpgradeData> upgrades;

        void Start()
        {
            anim = GetComponentInParent<Animator>();
        }

        public override void Attack()
        {
            if (anim.GetFloat("Idle") == 1)
            {
                transform.localScale = new Vector3(weaponValues.area, weaponValues.area, 1);
            }
            else if (anim.GetFloat("Idle") == -1)
            {
                transform.localScale = new Vector3(-weaponValues.area, weaponValues.area, 1);
            }
            WhipAttack();
            Invoke("CloseWhips", weaponValues.stayTime);
        }
        void WhipAttack()
        {
            whips[attackCount].SetActive(true);
            Collider2D[] objs;
            objs = Physics2D.OverlapBoxAll(whips[attackCount].transform.position, new Vector2(3.4f * weaponValues.area, 1.5f * weaponValues.area), 0f);
            for (int i = 0; i < objs.Length; i++)
            {
                IDamage obj = objs[i].GetComponent<IDamage>();
                if (obj != null)
                    obj.TakeDamage(weaponValues.damage, 2);
            }
            if (attackCount < weaponValues.count - 1)
            {
                Invoke("WhipAttack", .1f);
                attackCount++;
            }
            else
            {
                attackCount = 0;
            }
        }

        void CloseWhips()
        {
            for (int i = 0; i < weaponValues.count; i++)
            {
                whips[i].SetActive(false);
            }
        }

        public override void Evolution()
        {
            weaponValues.area += 0.6f;
            weaponValues.count += 1;
            FindObjectOfType<GameManager>().AddToUpgradeList(upgrades);
        }
    }
}