using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform weaponsPos;
    [SerializeField] private GameManager gameManager;
    public List<Weapons> weapons=new List<Weapons>();

    public void AddWeapon(WeaponData weaponData)
    {
        var weaponObj = Instantiate(weaponData.weaponPrefab, weaponsPos);
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