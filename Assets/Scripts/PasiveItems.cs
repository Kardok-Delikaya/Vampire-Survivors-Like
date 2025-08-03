using System.Collections.Generic;
using UnityEngine;

public class PasiveItems : MonoBehaviour
{
    [SerializeField] private List<Items> items;
    private PlayerManager character;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        character = GetComponent<PlayerManager>();
    }

    public void Equip(Items item)
    {
        if (items == null)
        {
            items = new List<Items>();
        }

        var newItem = ScriptableObject.CreateInstance<Items>();
        newItem.isIn(item.name);
        newItem.values.Add(item.values);
        items.Add(newItem);
        newItem.Equip(character);
        gameManager.AddToUpgradeList(item.upgrades);
    }

    internal void ItemUpgrade(UpgradeData upgradeData)
    {
        var upgradedItem = items.Find(id => id.name == upgradeData.item.name);
        upgradedItem.values.Add(upgradeData.itemValues);
        upgradedItem.Equip(character);
        gameManager.AddToUpgradeList(upgradeData.upgrades);
    }
}