using TMPro;
using UnityEngine;

namespace Others
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private float time;
        private int minute;
        private int second;

        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void FixedUpdate()
        {
            time += Time.fixedDeltaTime;
            second = (int)(time % 60);
            minute = (int)(time / 60) % 60;
            text.text = string.Format("{0:0}:{1:00}", minute, second);
        }
    }
}