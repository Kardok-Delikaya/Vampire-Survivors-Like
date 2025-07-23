using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class PasiveItems : MonoBehaviour
    {
        [SerializeField] List<Items> items;
        Player character;
        [SerializeField] GameManager gameManager;

        void Awake()
        {
            character = GetComponent<Player>();
        }
        public void Equip(Items item)
        {
            if (items == null)
            {
                items = new List<Items>();
            }
            Items newItem = new Items();
            newItem.isIn(item.name);
            newItem.values.Add(item.values);
            items.Add(newItem);
            newItem.Equip(character);
            gameManager.AddToUpgradeList(item.upgrades);
        }
        internal void ItemUpgrade(UpgradeData upgradeData)
        {
            Items upgradedItem = items.Find(id => id.name == upgradeData.item.name);
            upgradedItem.values.Add(upgradeData.itemValues);
            upgradedItem.Equip(character);
            gameManager.AddToUpgradeList(upgradeData.upgrades);
        }
    }
}