using System;
using UnityEngine;
using System.Collections.Generic;

public class XPSpawner : MonoBehaviour
{
    [Header("XP Objects")] [SerializeField]
    private LootableObject redXP;

    [SerializeField] private List<LootableObject> xpList;

    public void SpawnXP(Transform spawnTransform, int xpRewardCount)
    {
        foreach (var xp in xpList)
        {
            if (xp.gameObject.activeSelf) continue;
            xp.gameObject.SetActive(true);
            xp.transform.position = spawnTransform.position;
            xp.count = xpRewardCount;
            return;
        }

        if (redXP.gameObject.activeSelf)
        {
            redXP.count += xpRewardCount;
        }
        else
        {
            redXP.gameObject.SetActive(true);
            redXP.count = xpRewardCount;
            redXP.transform.position = spawnTransform.position;
        }
    }
}