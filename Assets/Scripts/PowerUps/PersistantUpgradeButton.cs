using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PowerUps
{
    public class PersistantUpgradeButton : MonoBehaviour
    {
        [Header("Button")]
        public int maxLevel;
        public int currentLevel;
        public List<Toggle> toggles;

        [Header("Upgrade Info")]
        public string upgradeName;
        public Sprite upgradeIcon;
        public string upgradeInfo;
        public int upgradeCost;

        private void Start()
        {
            currentLevel = PlayerPrefs.GetInt(upgradeName+"level");

            for (var i = 0; i < currentLevel; i++)
            {
                toggles[i].isOn = true;
            }
        }

        public void Upgrade()
        {
            currentLevel++;

            for(var i=0;i<currentLevel; i++)
            {
                toggles[i].isOn = true;
            }

            PlayerPrefs.SetInt(upgradeName + "level", currentLevel);
        }

        public void Reset()
        {
            currentLevel = 0;

            for (var i = 0; i < maxLevel; i++)
            {
                toggles[i].isOn = false;
            }

            PlayerPrefs.SetInt(upgradeName + "level", 0);
        }
    }
}
