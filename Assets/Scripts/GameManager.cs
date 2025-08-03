using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Spawner spawner;
    private PlayerManager player;
    private int killCount;
    private int targetKillCount;
    private int goldCount;
    private float maxXp;
    private float xp;

    [Header("Level")]
    public int level;

    [Header("HUD")]
    [SerializeField]
    private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI goldCountText;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelMenu;

    [Header("Rewards")]
    [SerializeField]
    private GameObject rewardMenu;
    [SerializeField] private TextMeshProUGUI rewardMessage;

    [Header("Upgrades")]
    [SerializeField]
    private List<Image> upgradePictures;
    [SerializeField] private List<UpgradeData> upgrades;
    [SerializeField] private List<UpgradeData> pasiveUpgrades;
    private List<UpgradeData> chosedUpgrades;
    [SerializeField] private List<UpgradeData> receivedUpgrades;
    [SerializeField] private List<UpgradeButton> upgradeButtons;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private PasiveItems pasiveItems;
    [SerializeField] private GameObject playerSpecPanel;
    public List<TextMeshProUGUI> playerSpecTexts;

    private void Awake()
    {
        spawner = GetComponent<Spawner>();
        player = FindAnyObjectByType<PlayerManager>();
        goldCount = PlayerPrefs.GetInt("Gold_Count");
        goldCountText.text = goldCount.ToString();
        maxXp = 5f;
        targetKillCount = 50;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ClearButtons();
        AddStartWeapon(0);
        AddToUpgradeList(pasiveUpgrades);
    }

    public void Menu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pauseMenu.activeSelf || rewardMenu.activeSelf)
            {
                ClosePauseMenu();
            }
            else if (!levelMenu.activeSelf)
            {
                OpenPauseMenu();
            }
        }
    }

    public void HandleKill()
    {
        killCount++;
        killText.text = killCount + "";

        if (killCount == targetKillCount)
        {
            GetReward();
        }
    }

    private void GetReward()
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
        goldCountText.text = goldCount.ToString();
        PlayerPrefs.SetInt("Gold_Count", goldCount);
        Pause(0);
    }

    public void GetXP(int xpCount)
    {
        xp += xpCount;

        if (xp >= maxXp)
        {
            LevelUp();
        }

        xpSlider.value = xp / maxXp;
    }

    private void LevelUp()
    {
        if (upgrades.Count != 0)
        {
            ClearButtons();
            OpenLevelMenu();
        }

        xp -= maxXp;
        level++;
        levelText.text = "LV " + level;

        if (level < 20)
        {
            maxXp += 10;
        }
        else if (level < 41)
        {
            maxXp += 13;
        }
        else
        {
            maxXp += 16;
        }

        spawner.TryToSpawnBoss();
    }

    public void GetGold()
    {
        var rewardGoldCount = Random.Range(level, level * 3);
        goldCount += rewardGoldCount;
        goldCountText.text = goldCount + "";
        PlayerPrefs.SetInt("Gold_Count", goldCount);
        player.Message($"+{rewardGoldCount}", Color.yellow);
    }

    public void Death()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        levelMenu.SetActive(false);
        rewardMenu.SetActive(false);
        Pause(1);
    }

    private void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        Pause(0);
    }

    public void Upgrade(int id)
    {
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
            xpSlider.value = xp / maxXp;
            LevelUp();
        }
        else
        {
            ClosePauseMenu();
        }
    }

    public List<UpgradeData> GetUpgrades(int count)
    {
        var UpgradeList = new List<UpgradeData>();
        var chosedNumbers = new List<int>();
        if (count > upgrades.Count)
        {
            count = upgrades.Count;
        }
        for (var i = 0; i < count;)
        {
            var id = Random.Range(0, upgrades.Count);

            if (!chosedNumbers.Contains(id))
            {
                UpgradeList.Add(upgrades[id]);
                chosedNumbers.Add(id);
                i++;
            }
        }

        return UpgradeList;
    }

    private void ClearButtons()
    {
        for (var i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].Clear();
            upgradeButtons[i].gameObject.SetActive(false);
        }
    }

    private void OpenLevelMenu()
    {
        if (chosedUpgrades == null) chosedUpgrades = new List<UpgradeData>();
        chosedUpgrades.Clear();
        chosedUpgrades.AddRange(GetUpgrades(3));

        for (var i = 0; i < chosedUpgrades.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(true);
            upgradeButtons[i].Upgrade(chosedUpgrades[i]);
        }

        levelMenu.SetActive(true);
        Pause(0);
    }

    internal void AddToUpgradeList(List<UpgradeData> newUpgrades)
    {
        this.upgrades.AddRange(newUpgrades);
    }

    private void Pause(int i)
    {
        Time.timeScale = i;

        if (i == 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CharacterSpecs();
        }
        else
        {
            playerSpecPanel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void AddSpriteToUpgradeSprites(UpgradeData upgradeData)
    {
        for (var i = 0; i < upgradePictures.Count; i++)
        {
            if (!upgradePictures[i].enabled)
            {
                upgradePictures[i].enabled = !upgradePictures[i].enabled;
                upgradePictures[i].sprite = upgradeData.icon;
                break;
            }
        }
    }

    private void AddStartWeapon(int i)
    {
        weaponManager.AddWeapon(upgrades[i].weaponData);
        AddSpriteToUpgradeSprites(upgrades[i]);
        receivedUpgrades.Add(upgrades[i]);
        upgrades.Remove(upgrades[i]);
    }

    public void AddWeapon(UpgradeData upgradeData)
    {
        weaponManager.AddWeapon(upgradeData.weaponData);
        AddSpriteToUpgradeSprites(upgradeData);
        receivedUpgrades.Add(upgradeData);
    }

    private void CharacterSpecs()
    {
        playerSpecTexts[0].text = player.maxHealth + "";
        playerSpecTexts[1].text = player.armor + "";
        playerSpecTexts[2].text = player.speed + "";
        playerSpecTexts[3].text = player.regenerate + "";
        playerSpecTexts[4].text = player.magnet + "";
        playerSpecPanel.SetActive(true);
    }
}