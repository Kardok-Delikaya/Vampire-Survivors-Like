using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponValues
{
    public int damage;
    public float timer;
    public int count;
    public int durability;
    public float stayTime;
    public float area;
    public float speed;
    public bool hasEvolved;

    public WeaponValues(int damage, float timer, int count, int durability, float stayTime, float area, float speed, bool hasEvolved)
    {
        this.damage = damage;
        this.timer = timer;
        this.count = count;
        this.durability = durability;
        this.stayTime = stayTime;
        this.area = area;
        this.speed = speed;
        this.hasEvolved = hasEvolved;
    }

    internal void Upgrade(WeaponValues weaponValues)
    {
        this.damage += weaponValues.damage;
        this.timer += weaponValues.timer;
        this.count += weaponValues.count;
        this.durability += weaponValues.durability;
        this.stayTime += weaponValues.stayTime;
        this.area += weaponValues.area;
        this.speed += weaponValues.speed;
        this.hasEvolved = weaponValues.hasEvolved;
    }
}

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public string Name;
    public WeaponValues values;
    public GameObject weaponPrefab;
    public List<UpgradeData> upgrades;
}