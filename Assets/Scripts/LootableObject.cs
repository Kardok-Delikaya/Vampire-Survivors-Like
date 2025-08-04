using UnityEngine;

public class LootableObject : MonoBehaviour
{
    private Transform player;

    public int id;
    [HideInInspector] public int count;

    private void Start()
    {
        player = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, player.position);

        if (!(distance > 50)) return;
        
        Destroy(gameObject);

        if (id == 0)
        {
            GameManager.Instance.xpObjCount--;
        }
    }
}