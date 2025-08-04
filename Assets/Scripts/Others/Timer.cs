using TMPro;
using UnityEngine;

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

    private void Update()
    {
        time += Time.deltaTime;
        second = (int)(time % 60);
        minute = (int)(time / 60) % 60;
        text.text = $"{minute:0}:{second:00}";
    }
}