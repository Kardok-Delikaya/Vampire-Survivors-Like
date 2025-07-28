using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VSLike
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI descriptionText;

        public void Upgrade(UpgradeData upgradeData)
        {
            icon.sprite = upgradeData.icon;
            nameText.text = upgradeData.Name;
            descriptionText.text = upgradeData.Description;
        }

        internal void Clear()
        {
            icon.sprite = null;
            nameText.text = null;
            descriptionText.text = null;
        }
    }
}