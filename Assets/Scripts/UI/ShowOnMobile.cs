using System;
using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(Application.isMobilePlatform);
    }
}
