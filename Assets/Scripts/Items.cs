using System;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    [Serializable]
    public class ItemValues
    {
        public int maxHealth;
        public int armor;
        public int speed;
        public int regenerate;
        public float magnet;

        internal void Add(ItemValues values)
        {
            maxHealth = values.maxHealth;
            armor = values.armor;
            speed = values.speed;
            regenerate = values.regenerate;
            magnet = values.magnet;
        }
    }
    [CreateAssetMenu]
    public class Items : ScriptableObject
    {
        public string Name;
        public ItemValues values;
        public List<UpgradeData> upgrades;
        public void isIn(string name)
        {
            this.name = name;
            values = new ItemValues();
            upgrades = new List<UpgradeData>();
        }
        public void Equip(PlayerManager character)
        {
            character.maxHealth += values.maxHealth;
            character.armor += values.armor;
            character.speed += values.speed;
            character.regenerate += values.regenerate;
            character.magnet += values.magnet;
        }
    }
}