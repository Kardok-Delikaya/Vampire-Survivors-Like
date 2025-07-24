using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VSLike
{
    public class PersistantUpgrades : MonoBehaviour
    {
        public Items persistantUpgrade;

        [Header("Upgrade")]
        public int goldCount;
        public TMPro.TMP_Text goldText;

        [Header("Buttons")]
        public PersistantUpgradeInfoPanel panel;
        public PersistantUpgradeButton[] buttons;

        void Start()
        {
            if (PlayerPrefs.GetInt("Gold_Count") == 0)
            {
                goldCount = 500;
            }
            else
            {
                goldCount = PlayerPrefs.GetInt("Gold_Count");
            }
            
            goldText.text = goldCount + "";
            GetUpgradesToPersistantItem();
            PersistantUpgradeInfoPanel(0);
        }

        void GetUpgradesToPersistantItem()
        {
            persistantUpgrade.values.maxHealth = PlayerPrefs.GetInt("Max Health Value");
            persistantUpgrade.values.armor = PlayerPrefs.GetInt("Armor Value");
            persistantUpgrade.values.speed = PlayerPrefs.GetInt("Speed Value");
            persistantUpgrade.values.regenerate = PlayerPrefs.GetInt("Regenerate Value");
            persistantUpgrade.values.magnet = PlayerPrefs.GetFloat("Magnet Value");
        }

        public void UpgradeMaxHealth()
        {
            if (buttons[0].currentLevel < buttons[0].maxLevel && goldCount >= buttons[0].upgradeCost)
            {
                goldCount -= buttons[0].upgradeCost;
                persistantUpgrade.values.maxHealth += 10;
                PlayerPrefs.SetInt("Max Health Value", persistantUpgrade.values.maxHealth);
                goldText.text = goldCount + "";
                buttons[0].Upgrade();
                PlayerPrefs.SetInt("Gold_Count", goldCount);
            }
        }

        public void UpgradeArmor()
        {
            if (buttons[1].currentLevel < buttons[1].maxLevel && goldCount >= buttons[1].upgradeCost)
            {
                goldCount -= buttons[1].upgradeCost;
                persistantUpgrade.values.armor += 2;
                PlayerPrefs.SetInt("Armor Value", persistantUpgrade.values.armor);
                goldText.text = goldCount + "";
                buttons[1].Upgrade();
                PlayerPrefs.SetInt("Gold_Count", goldCount);
            }
        }

        public void UpgradeSpeed()
        {
            if (buttons[2].currentLevel < buttons[2].maxLevel && goldCount >= buttons[2].upgradeCost)
            {
                goldCount -= buttons[2].upgradeCost;
                persistantUpgrade.values.speed += 1;
                PlayerPrefs.SetInt("Speed Value", persistantUpgrade.values.speed);
                goldText.text = goldCount + "";
                buttons[2].Upgrade();
                PlayerPrefs.SetInt("Gold_Count", goldCount);
            }
        }

        public void UpgradeRegenerate()
        {
            if (buttons[3].currentLevel < buttons[3].maxLevel && goldCount >= buttons[3].upgradeCost)
            {
                goldCount -= buttons[3].upgradeCost;
                persistantUpgrade.values.regenerate += 1;
                PlayerPrefs.SetInt("Regenerate Value", persistantUpgrade.values.regenerate);
                goldText.text = goldCount + "";
                buttons[3].Upgrade();
                PlayerPrefs.SetInt("Gold_Count", goldCount);
            }
        }

        public void UpgradeMagnet()
        {
            if (buttons[4].currentLevel < buttons[4].maxLevel && goldCount >= buttons[4].upgradeCost)
            {
                goldCount -= buttons[4].upgradeCost;
                persistantUpgrade.values.magnet += 0.5f;
                PlayerPrefs.SetFloat("Magnet Value", persistantUpgrade.values.magnet);
                goldText.text = goldCount + "";
                buttons[4].Upgrade();
                PlayerPrefs.SetInt("Gold_Count", goldCount);
            }
        }

        public void PersistantUpgradeInfoPanel(int id)
        {
            panel.upgradeName.text = buttons[id].upgradeName;
            panel.upgradeIcon.sprite = buttons[id].upgradeIcon;
            panel.upgradeInfo.text = buttons[id].upgradeInfo;
            panel.upgradeCost.text = buttons[id].upgradeCost + "";

            for (int i = 0; i < buttons.Length; i++)
            {
                panel.upgradeButton[i].gameObject.SetActive(false);
            }

            panel.upgradeButton[id].gameObject.SetActive(true);
        }

        public void Reset()
        {
            persistantUpgrade.values.maxHealth = 0;
            persistantUpgrade.values.armor = 0;
            persistantUpgrade.values.speed = 0;
            persistantUpgrade.values.regenerate = 0;
            persistantUpgrade.values.magnet = 0;

            PlayerPrefs.SetInt("Max Health Value", 0);
            PlayerPrefs.SetInt("Armor Value", 0);
            PlayerPrefs.SetInt("Speed Value", 0);
            PlayerPrefs.SetInt("Regenerate Value", 0);
            PlayerPrefs.SetFloat("Magnet Value", 0);

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Reset();
            }
        }
    }
}