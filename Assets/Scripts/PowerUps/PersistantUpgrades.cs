using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PersistantUpgrades : MonoBehaviour
{
    [SerializeField] private Items persistantUpgrade;

    [Header("Upgrade")] 
    [SerializeField] private int goldCount;
    [SerializeField] private TMP_Text goldText;

    [Header("Buttons")] 
    [SerializeField] private PersistantUpgradeButton[] buttons;

    [Header("Info Panel")]
    [SerializeField] private TMP_Text upgradeName;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TMP_Text upgradeInfo;
    [SerializeField] private TMP_Text upgradeCost;
    [SerializeField] private Button upgradeBuyButton;
    private int currentUpgradeButton = 0;
    
    private void Start()
    {
        upgradeBuyButton.onClick.AddListener(UpgradeStats);
        
        for (var i=0; i<buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().onClick.AddListener(PersistantUpgradeInfoPanel(i));
        }
        
        goldCount = PlayerPrefs.GetInt("Gold_Count");
        goldText.text = goldCount + "";
        GetUpgradesToPersistantItem();
        PersistantUpgradeInfoPanel(currentUpgradeButton);
    }

    private void GetUpgradesToPersistantItem()
    {
        persistantUpgrade.values.maxHealth = PlayerPrefs.GetInt("Max Health Value");
        persistantUpgrade.values.armor = PlayerPrefs.GetInt("Armor Value");
        persistantUpgrade.values.speed = PlayerPrefs.GetInt("Speed Value");
        persistantUpgrade.values.regenerate = PlayerPrefs.GetInt("Regenerate Value");
        persistantUpgrade.values.magnet = PlayerPrefs.GetFloat("Magnet Value");
    }

    private void UpgradeStats()
    {
        if (buttons[currentUpgradeButton].currentLevel >= buttons[currentUpgradeButton].maxLevel ||
            goldCount < buttons[currentUpgradeButton].upgradeCost) 
            return;
        
        goldCount -= buttons[currentUpgradeButton].upgradeCost;
        persistantUpgrade.values.maxHealth += 10;
            
        switch (currentUpgradeButton)
        {
            case 0:
                persistantUpgrade.values.maxHealth += 10;
                break;
            case 1:
                persistantUpgrade.values.armor += 2;
                break;
            case 2:
                persistantUpgrade.values.speed += 1;
                break;
            case 3:
                persistantUpgrade.values.regenerate += 1;
                break;
            case 4:
                persistantUpgrade.values.magnet += 0.5f;
                break;
        }
            
        PlayerPrefs.SetInt("Max Health Value", persistantUpgrade.values.maxHealth);
        goldText.text = goldCount + "";
        buttons[currentUpgradeButton].Upgrade();
        PlayerPrefs.SetInt("Gold_Count", goldCount);
    }

    private UnityAction PersistantUpgradeInfoPanel(int upgradeID)
    {
        upgradeName.text = buttons[upgradeID].upgradeName;
        upgradeIcon.sprite = buttons[upgradeID].upgradeIcon;
        upgradeInfo.text = buttons[upgradeID].upgradeInfo;
        upgradeCost.text = buttons[upgradeID].upgradeCost + "";
        currentUpgradeButton = upgradeID;
        return null;
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

        foreach (var button in buttons)
        {
            button.Reset();
        }
    }
}