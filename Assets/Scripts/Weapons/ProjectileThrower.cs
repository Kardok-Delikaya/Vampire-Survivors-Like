using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class ProjectileThrower : Weapons
    {
        [SerializeField] bool isArrow;
        [SerializeField] GameObject projectile;
        [SerializeField] Transform[] projectileSpawnPoint;
        [SerializeField] List<UpgradeData> newUpgrades;

        void GetClosestEnemy(Transform poz)
        {
            Collider2D[] damageableObjects = Physics2D.OverlapCircleAll(poz.position, 20f, damageableLayer);
            Transform closestDamageableObject = null;
            float minDist = Mathf.Infinity;

            foreach (Collider2D damageable in damageableObjects)
            {
                float dist = Vector3.Distance(damageable.transform.position, poz.position);

                if (dist < minDist)
                {
                    closestDamageableObject = damageable.transform;
                    minDist = dist;
                }
            }

            if (closestDamageableObject != null)
                poz.up = closestDamageableObject.position - poz.position;
        }

        public override void Attack()
        {
            for (int i = 0; i < weaponValues.count; i++)
            {
                GameObject throwedObject = Instantiate(projectile, projectileSpawnPoint[i]);
                GetClosestEnemy(throwedObject.transform);
                throwedObject.GetComponent<IThrowable>().Equalize(weaponValues.damage, weaponValues.durability, weaponValues.stayTime, weaponValues.speed, weaponValues.hasEvolved, damageableLayer);

                if (isArrow)
                    throwedObject.transform.SetParent(null);
            }
        }

        public override void Evolution()
        {
            this.weaponValues.hasEvolved = true;
            FindAnyObjectByType<GameManager>().AddToUpgradeList(newUpgrades);
        }
    }
}