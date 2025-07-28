using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public abstract class Weapons : MonoBehaviour
    {
        public WeaponData weaponData;
        public WeaponValues weaponValues;
        [SerializeField] protected LayerMask damageableLayer;
        float timer;

        public void FixedUpdate()
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0f)
            {
                Attack();
                timer = weaponValues.timer;
            }
        }

        public virtual void SelectData(WeaponData sd)
        {
            weaponData = sd;

            weaponValues = new WeaponValues(sd.values.damage, sd.values.timer, sd.values.count, sd.values.durability, sd.values.stayTime, sd.values.area, sd.values.speed, sd.values.hasEvolved);
        }

        public abstract void Attack();

        public abstract void Evolution();

        internal void Upgrade(UpgradeData upgradeData)
        {
            weaponValues.Upgrade(upgradeData.weaponValues);
        }
    }
}