using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

namespace VSLike
{
    public class GameManager : MonoBehaviour
    {
        Spawner spawner;
        int killCount;
        int targetKillCount;
        int goldCount;
        float maxXp;
        float xp;

        [Header("Level")]
        public int level;

        [Header("HUD")]
        [SerializeField] Slider xpSlider;
        [SerializeField] TextMeshProUGUI levelText;
        [SerializeField] TextMeshProUGUI killText;
        [SerializeField] TextMeshProUGUI goldCountText;
        [SerializeField] GameObject deadScreen;
        [SerializeField] GameObject pauseMenu;
        [SerializeField] GameObject levelMenu;

        [Header("Rewards")]
        [SerializeField] GameObject rewardMenu;
        [SerializeField] TextMeshProUGUI rewardMessage;

        [Header("Upgrades")]
        [SerializeField] List<Image> upgradePictures;
        [SerializeField] List<UpgradeData> upgrades;
        [SerializeField] List<UpgradeData> pasiveUpgrades;
        List<UpgradeData> chosedUpgrades;
        [SerializeField] List<UpgradeData> receivedUpgrades;
        [SerializeField] List<UpgradeButton> upgradeButtons;
        [SerializeField] WeaponManager weaponManager;
        [SerializeField] PasiveItems pasiveItems;
        [SerializeField] Player character;
        [SerializeField] GameObject characterTable;
        public List<TextMeshProUGUI> characterSpecs;
       
        void Start()
        {
            goldCount=PlayerPrefs.GetInt("Gold_Count");
            goldCountText.text = goldCount + "";
            maxXp = 5f;
            targetKillCount = 50;
            Time.timeScale = 1;
            spawner = GetComponent<Spawner>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CleanButtons();
            AddStartWeapon(1);
            AddStartWeapon(1);
            AddStartWeapon(1);
            AddStartWeapon(1);
            AddStartWeapon(0);
            AddToUpgradeList(pasiveUpgrades);
        }

        public void Menu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (pauseMenu.activeSelf || rewardMenu.activeSelf)
                {
                    CloseMenu();
                }
                else if (!levelMenu.activeSelf)
                {
                    OpenMenu();
                }
            }
        }

        public void Kill()
        {
            killCount++;
            killText.text = killCount + "";

            if (killCount == targetKillCount)
            {
                GetReward();
            }
        }

        public void GetReward()
        {
            int i = Random.Range(level * 1, level * 3);
            rewardMenu.SetActive(true);
            switch (targetKillCount)
            {
                case 50:
                    rewardMessage.text = "+10 Shield +" + i + " gold.";
                    character.shield += 10;
                    character.Message("+10", Color.blue);
                    targetKillCount += 100;
                    break;
                case 150:
                    rewardMessage.text = "+15 Shield +" + i + " gold.";
                    character.shield += 15;
                    character.Message("+15", Color.blue);
                    targetKillCount += 150;
                    break;
                case 300:
                    rewardMessage.text = "+20 Shield +" + i + " gold.";
                    character.shield += 20;
                    character.Message("+20", Color.blue);
                    targetKillCount += 200;
                    break;
                case 500:
                    rewardMessage.text = "+25 Shield +" + i + " gold.";
                    character.shield += 25;
                    character.Message("+25", Color.blue);
                    targetKillCount += 250;
                    break;
                default:
                    rewardMessage.text = "+25 Shield +" + i + " gold.";
                    character.shield += 25;
                    character.Message("+25", Color.blue);
                    targetKillCount += 500;
                    break;
            }

            goldCount += i;
            character.ShieldValue();
            goldCountText.text = goldCount + "";
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

        void LevelUp()
        {
            if (upgrades.Count != 0)
            {
                CleanButtons();
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

            spawner.BossSpawn();
            spawner.level++;
        }

        public void GetGold()
        {
            int rewardGoldCount = Random.Range(level, level * 3);
            goldCount += rewardGoldCount;
            goldCountText.text = goldCount + "";
            PlayerPrefs.SetInt("Gold_Count", goldCount);
            character.Message("+" + rewardGoldCount, Color.yellow);
        }

        public void Death()
        {
            deadScreen.SetActive(true);
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

        public void CloseMenu()
        {
            pauseMenu.SetActive(false);
            levelMenu.SetActive(false);
            rewardMenu.SetActive(false);
            Pause(1);
        }

        void OpenMenu()
        {
            pauseMenu.SetActive(true);
            Pause(0);
        }

        public void Upgrade(int id)
        {
            UpgradeData upgradeData = chosedUpgrades[id];
            if (receivedUpgrades == null) { receivedUpgrades = new List<UpgradeData>(); }
            switch (upgradeData.upgradeType)
            {
                case UpgradeType.OpenItem:
                    pasiveItems.Equip(upgradeData.item);
                    ShowSprite(upgradeData);
                    break;
                case UpgradeType.ItemValue:
                    pasiveItems.ItemUpgrade(upgradeData);
                    break;
                case UpgradeType.OpenWeapon:
                    weaponManager.AddWeapon(upgradeData.weaponData);
                    ShowSprite(upgradeData);
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
            CloseMenu();
        }

        public List<UpgradeData> GetUpgrades(int count)
        {
            List<UpgradeData> UpgradeList = new List<UpgradeData>();
            List<int> chosedNumbers = new List<int>();
            if (count > upgrades.Count)
            {
                count = upgrades.Count;
            }
            for (int i = 0; i < count;)
            {
                int id = Random.Range(0, upgrades.Count);

                if (!chosedNumbers.Contains(id))
                {
                    UpgradeList.Add(upgrades[id]);
                    chosedNumbers.Add(id);
                    i++;
                }
            }

            return UpgradeList;
        }

        void CleanButtons()
        {
            for (int i = 0; i < upgradeButtons.Count; i++)
            {
                upgradeButtons[i].Clear();
                upgradeButtons[i].gameObject.SetActive(false);
            }
        }

        void OpenLevelMenu()
        {
            if (chosedUpgrades == null) { chosedUpgrades = new List<UpgradeData>(); }
            chosedUpgrades.Clear();
            chosedUpgrades.AddRange(GetUpgrades(3));

            for (int i = 0; i < chosedUpgrades.Count; i++)
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

        void Pause(int i)
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
                characterTable.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        void ShowSprite(UpgradeData upgradeData)
        {
            for (int i = 0; i < upgradePictures.Count; i++)
            {
                if (!upgradePictures[i].enabled)
                {
                    upgradePictures[i].enabled = !upgradePictures[i].enabled;
                    upgradePictures[i].sprite = upgradeData.icon;
                    break;
                }
            }
        }

        void AddStartWeapon(int i)
        {
            weaponManager.AddWeapon(upgrades[i].weaponData);
            ShowSprite(upgrades[i]);
            receivedUpgrades.Add(upgrades[i]);
            upgrades.Remove(upgrades[i]);
        }

        public void AddWeapon(UpgradeData upgradeData)
        {
            weaponManager.AddWeapon(upgradeData.weaponData);
            ShowSprite(upgradeData);
            receivedUpgrades.Add(upgradeData);
        }

        void CharacterSpecs()
        {
            characterSpecs[0].text = character.maxHealth + "";
            characterSpecs[1].text = character.health + "";
            characterSpecs[2].text = character.shield + "";
            characterSpecs[3].text = character.armor + "";
            characterSpecs[4].text = character.speed + "";
            characterSpecs[5].text = character.regenerate + "";
            characterSpecs[6].text = character.magnet + "";
            characterTable.SetActive(true);
        }
    }
}