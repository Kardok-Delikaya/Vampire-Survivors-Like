using System.Collections.Generic;
using UnityEngine;

public class PasiveItems : MonoBehaviour
{
    [SerializeField] private List<Items> items;

    public void Equip(Items item)
    {
        items ??= new List<Items>();

        var newItem = ScriptableObject.CreateInstance<Items>();
        newItem.isIn(item.name);
        newItem.values.Add(item.values);
        items.Add(newItem);
        newItem.Equip(GameManager.Instance.player);
        GameManager.Instance.AddToUpgradeList(item.upgrades);
    }

    internal void ItemUpgrade(UpgradeData upgradeData)
    {
        var upgradedItem = items.Find(id => id.name == upgradeData.item.name);
        upgradedItem.values.Add(upgradeData.itemValues);
        upgradedItem.Equip(GameManager.Instance.player);
        GameManager.Instance.AddToUpgradeList(upgradeData.upgrades);
    }
}