using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] Transform weaponsPos;
        [SerializeField] GameManager gameManager;
        public List<Weapons> weapons=new List<Weapons>();

        public void AddWeapon(WeaponData weaponData)
        {
            GameObject weaponObj = Instantiate(weaponData.weaponPrefab, weaponsPos);
            Weapons weapon = weaponObj.GetComponent<Weapons>();

            weapon.SelectData(weaponData);
            weapons.Add(weapon);
            gameManager.AddToUpgradeList(weaponData.upgrades);
        }

        internal void UpgradeWeapon(UpgradeData upgradeData)
        {
            Weapons weapon = weapons.Find(wd => wd.weaponData == upgradeData.weaponData);
            weapon.Upgrade(upgradeData);
            gameManager.AddToUpgradeList(upgradeData.upgrades);
        }
    }
}