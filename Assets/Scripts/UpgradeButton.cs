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
        [SerializeField] TextMeshProUGUI ad, açıklama;
        public void Upgrade(UpgradeData yükseltmeData)
        {
            icon.sprite = yükseltmeData.icon;
            ad.text = yükseltmeData.Name;
            açıklama.text = yükseltmeData.Description;
        }

        internal void Clear()
        {
            icon.sprite = null;
            ad.text = null;
            açıklama.text = null;
        }
    }
}