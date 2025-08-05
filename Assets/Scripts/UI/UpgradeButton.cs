using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void HandleUpgradeButtons(UpgradeData upgradeData)
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