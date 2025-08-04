using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Spawner))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerManager player;
    public Spawner spawner;
    public UIManager uiManager { get; private set; }

    [Header("Level")]
    public int level;
    [SerializeField] private float maxXp;
    [SerializeField] private int targetKillCount;
    private int killCount;
    private int goldCount;
    private float xp;

    [Header("Rewards")]
    [SerializeField]
    private GameObject rewardMenu;
    [SerializeField] private TMP_Text rewardMessage;

    [Header("Upgrades")]
    [SerializeField] private List<UpgradeData> upgrades;
    [SerializeField] private List<UpgradeData> pasiveUpgrades;
    private List<UpgradeData> chosedUpgrades;
    [SerializeField] private List<UpgradeData> receivedUpgrades;
    [SerializeField] private List<UpgradeButton> upgradeButtons;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private PasiveItems pasiveItems;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        uiManager=GetComponent<UIManager>();
        
        goldCount = PlayerPrefs.GetInt("Gold_Count");
        uiManager.goldCountText.text = goldCount.ToString();
        AddStartWeapon();
        AddToUpgradeList(pasiveUpgrades);
    }

    public void HandleKill()
    {
        killCount++;
        uiManager.killText.text = killCount.ToString();

        if (killCount == targetKillCount)
        {
            StartCoroutine(GetReward());
        }
    }

    private IEnumerator GetReward()
    {
        var randomGoldReward = Random.Range(level * 1, level * 3);
        rewardMenu.SetActive(true);

        switch (targetKillCount)
        {
            case 50:
                rewardMessage.text = "+10 Shield +" + randomGoldReward + " gold.";
                player.GainShield(10);
                targetKillCount += 100;
                break;
            case 150:
                rewardMessage.text = "+15 Shield +" + randomGoldReward + " gold.";
                player.GainShield(15);
                targetKillCount += 150;
                break;
            case 300:
                rewardMessage.text = "+20 Shield +" + randomGoldReward + " gold.";
                player.GainShield(20);
                break;
            case 500:
                rewardMessage.text = "+25 Shield +" + randomGoldReward + " gold.";
                player.GainShield(25);
                targetKillCount += 250;
                break;
            default:
                rewardMessage.text = "+25 Shield +" + randomGoldReward + " gold.";
                player.GainShield(25);
                targetKillCount += 500;
                break;
        }

        goldCount += randomGoldReward;
        player.ShieldValue();
        uiManager.goldCountText.text = goldCount.ToString();
        PlayerPrefs.SetInt("Gold_Count", goldCount);
        
        yield return new WaitForSeconds(5f);
        rewardMenu.SetActive(false);
    }

    public void GetXP(int xpCount)
    {
        xp += xpCount;

        if (xp >= maxXp)
        {
            LevelUp();
        }

        uiManager.xpSlider.value = xp / maxXp;
    }

    private void LevelUp()
    {
        if (upgrades.Count != 0)
        {
            OpenLevelMenu();
        }

        xp -= maxXp;
        level++;
        uiManager.levelText.text = $"LV {level}";

        maxXp += level switch
        {
            < 20 => 10,
            < 41 => 13,
            _ => 16
        };

        spawner.TryToSpawnBoss();
    }

    public void GetGold()
    {
        var rewardGoldCount = Random.Range(level, level * 3);
        goldCount += rewardGoldCount;
        uiManager.goldCountText.text = goldCount.ToString();
        PlayerPrefs.SetInt("Gold_Count", goldCount);
        player.Message($"+{rewardGoldCount}", Color.yellow);
    }

    public void Upgrade(int id)
    {
        Debug.Log($"Upgrade {id}");
        var upgradeData = chosedUpgrades[id];
        if (receivedUpgrades == null) receivedUpgrades = new List<UpgradeData>();
        switch (upgradeData.upgradeType)
        {
            case UpgradeType.OpenItem:
                pasiveItems.Equip(upgradeData.item);
                AddSpriteToUpgradeSprites(upgradeData);
                break;
            case UpgradeType.ItemValue:
                pasiveItems.ItemUpgrade(upgradeData);
                break;
            case UpgradeType.OpenWeapon:
                weaponManager.AddWeapon(upgradeData.weaponData);
                AddSpriteToUpgradeSprites(upgradeData);
                break;
            case UpgradeType.WeaponValue:
                weaponManager.UpgradeWeapon(upgradeData);
                break;
        }
        if (upgradeData.lastLevel)
        {
            if (receivedUpgrades.Contains(upgradeData.common))
            {
                if (upgradeData.upgradeType == UpgradeType.WeaponValue)
                {
                    weaponManager.weapons.Find(wd => wd.weaponData == upgradeData.weaponData).Evolution();
                }
                else
                {
                    weaponManager.weapons.Find(wd => wd.weaponData == upgradeData.common.weaponData).Evolution();
                }
            }
        }
        receivedUpgrades.Add(upgradeData);
        upgrades.Remove(upgradeData);

        if (xp >= maxXp&&upgrades.Count>0)
        {
            uiManager.xpSlider.value = xp / maxXp;
            LevelUp();
        }
        else
        {
            uiManager.ClosePauseMenu();
        }
    }

    private List<UpgradeData> GetUpgrades()
    {
        var upgradeList = new List<UpgradeData>();
        var chosedNumbers = new List<int>();
        
        var upgradeCount=3;

        if (upgrades.Count < 3)
        {
            upgradeCount = upgrades.Count;
            
            for (int i = 2; i > 0; i--)
            {
                upgradeButtons[i].gameObject.SetActive(false);
            }
        }
        

        for (var i = 0; i < upgradeCount;)
        {
            var upgradeId = Random.Range(0, upgrades.Count);

            if (!chosedNumbers.Contains(upgradeId))
            {
                upgradeList.Add(upgrades[upgradeId]);
                chosedNumbers.Add(upgradeId);
                i++;
            }
        }
        
        

        return upgradeList;
    }

    private void OpenLevelMenu()
    {
        chosedUpgrades ??= new List<UpgradeData>();
        chosedUpgrades.Clear();
        chosedUpgrades.AddRange(GetUpgrades());

        for (var i = 0; i < chosedUpgrades.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(true);
            upgradeButtons[i].HandleUpgradeButtons(chosedUpgrades[i]);
        }

        uiManager.levelMenu.SetActive(true);
        uiManager.PauseGame();
    }

    internal void AddToUpgradeList(List<UpgradeData> newUpgrades)
    {
        upgrades.AddRange(newUpgrades);
    }

    private void AddSpriteToUpgradeSprites(UpgradeData upgradeData)
    {
        foreach (var upgradePicture in uiManager.upgradePictures.Where(upgradePicture => !upgradePicture.enabled))
        {
            upgradePicture.enabled = !upgradePicture.enabled;
            upgradePicture.sprite = upgradeData.icon;
            break;
        }
    }

    private void AddStartWeapon()
    {
        weaponManager.AddWeapon(upgrades[0].weaponData);
        AddSpriteToUpgradeSprites(upgrades[0]);
        receivedUpgrades.Add(upgrades[0]);
        upgrades.Remove(upgrades[0]);
    }

    public void AddWeapon(UpgradeData upgradeData)
    {
        weaponManager.AddWeapon(upgradeData.weaponData);
        AddSpriteToUpgradeSprites(upgradeData);
        receivedUpgrades.Add(upgradeData);
    }
}