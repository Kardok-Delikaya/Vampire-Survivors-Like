using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VSLike
{
    public class Timer : MonoBehaviour
    {
        TextMeshProUGUI text;
        float time;
        int minute;
        int second;

        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        void FixedUpdate()
        {
            time += Time.fixedDeltaTime;
            second = (int)(time % 60);
            minute = (int)(time / 60) % 60;
            text.text = string.Format("{0:0}:{1:00}", minute, second);
        }
    }
}