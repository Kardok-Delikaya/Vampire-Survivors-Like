using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Arrow : Weapons
    {
        [SerializeField] bool isArrow;
        [SerializeField] GameObject projectile;
        [SerializeField] Transform[] projectileSpawnPoint;
        [SerializeField] List<UpgradeData> newUpgrades;

        void GetClosestEnemy(Transform poz)
        {
            Collider2D[] düþmanlar = Physics2D.OverlapCircleAll(poz.position, 20f);
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = poz.position;
            foreach (Collider2D t in düþmanlar)
            {
                if (t.GetComponent<EnemyManager>())
                {
                    float dist = Vector3.Distance(t.transform.position, currentPos);
                    if (dist < minDist)
                    {
                        tMin = t.transform;
                        minDist = dist;
                    }
                }
            }

            if (tMin != null)
                poz.up = tMin.position - poz.position;
        }

        public override void Attack()
        {
            for (int i = 0; i < weaponValues.count; i++)
            {
                GameObject throwedArrow = Instantiate(projectile, projectileSpawnPoint[i]);
                GetClosestEnemy(throwedArrow.transform);
                throwedArrow.GetComponent<IThrowable>().Equalize(weaponValues.damage, weaponValues.durability, weaponValues.stayTime, weaponValues.speed, weaponValues.hasEvolved, damageableLayer);

                if (isArrow)
                    throwedArrow.transform.SetParent(null);
            }
        }

        public override void Evolution()
        {
            this.weaponValues.hasEvolved = true;
            FindAnyObjectByType<GameManager>().AddToUpgradeList(newUpgrades);
        }
    }
}