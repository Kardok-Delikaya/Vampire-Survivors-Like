using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapons> weapons=new List<Weapons>();

    public void AddWeapon(WeaponData weaponData)
    {
        var weaponObj = Instantiate(weaponData.weaponPrefab, transform);
        Weapons weapon = weaponObj.GetComponent<Weapons>();

        weapon.SelectData(weaponData);
        weapons.Add(weapon);
        GameManager.Instance.AddToUpgradeList(weaponData.upgrades);
    }

    internal void UpgradeWeapon(UpgradeData upgradeData)
    {
        Weapons weapon = weapons.Find(wd => wd.weaponData == upgradeData.weaponData);
        weapon.Upgrade(upgradeData);
        GameManager.Instance.AddToUpgradeList(upgradeData.upgrades);
    }
}