using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VSLike
{
    public class PersistantUpgradeInfoPanel : MonoBehaviour
    {
        public TMPro.TMP_Text upgradeName;
        public Image upgradeIcon;
        public TMPro.TMP_Text upgradeInfo;
        public TMPro.TMP_Text upgradeCost;
        public List<Button> upgradeButton;
    }
}
