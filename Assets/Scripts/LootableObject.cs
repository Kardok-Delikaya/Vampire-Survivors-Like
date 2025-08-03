using UnityEngine;

public class LootableObject : MonoBehaviour
{
    private Transform player;

    public int id;
    [HideInInspector] public int count;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerManager>().transform;
    }

    private void FixedUpdate()
    {
        var distance = Vector3.Distance(transform.position, player.position);

        if (distance > 50)
        {
            Destroy(gameObject);

            if (id == 0)
            {
                FindAnyObjectByType<PlayerManager>().xpCount--;
            }
        }
    }
}