using UnityEngine;

public class LootableObject : MonoBehaviour
{
    private Transform player;

    public int id; //0: XP || 1: Health || 2: Gold
    public int count;

    private void Start()
    {
        player = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, player.position);

        if (!(distance > 50)) return;

        gameObject.SetActive(false);
    }
}