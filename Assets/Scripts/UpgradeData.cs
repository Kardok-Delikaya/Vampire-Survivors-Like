using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    WeaponValue,
    ItemValue,
    OpenWeapon,
    OpenItem
}

[CreateAssetMenu]
public class UpgradeData : ScriptableObject
{
    public UpgradeType upgradeType;
    public string Name;
    public string Description;
    public Sprite icon;
    public WeaponData weaponData;
    public WeaponValues weaponValues;
    public Items item;
    public ItemValues itemValues;
    public List<UpgradeData> upgrades;
    public bool lastLevel;
    public UpgradeData common;
}