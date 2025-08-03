using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrower : Weapons
{
    [SerializeField] private bool isArrow;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform[] projectileSpawnPoint;
    [SerializeField] private List<UpgradeData> newUpgrades;

    private void GetClosestEnemy(Transform poz)
    {
        var damageableObjects = Physics2D.OverlapCircleAll(poz.position, 20f, damageableLayer);
        Transform closestDamageableObject = null;
        var minDist = Mathf.Infinity;

        foreach (var damageable in damageableObjects)
        {
            var dist = Vector3.Distance(damageable.transform.position, poz.position);

            if (dist < minDist)
            {
                closestDamageableObject = damageable.transform;
                minDist = dist;
            }
        }

        if (closestDamageableObject != null)
            poz.up = closestDamageableObject.position - poz.position;
    }

    protected override void Attack()
    {
        for (var i = 0; i < weaponValues.count; i++)
        {
            var throwedObject = Object.Instantiate(projectile, projectileSpawnPoint[i]);
            GetClosestEnemy(throwedObject.transform);
            throwedObject.GetComponent<IThrowable>().Equalize(weaponValues.damage, weaponValues.durability,
                weaponValues.stayTime, weaponValues.speed, weaponValues.hasEvolved, damageableLayer);

            if (isArrow)
                throwedObject.transform.SetParent(null);
        }
    }

    public override void Evolution()
    {
        this.weaponValues.hasEvolved = true;
        Object.FindAnyObjectByType<GameManager>().AddToUpgradeList(newUpgrades);
    }
}