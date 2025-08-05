using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamage
{
    private Transform player;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject gold;

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
        }
    }

    public void TakeDamage(int damage, int power)
    {
        var randomNumber = Random.Range(0, 100);

        if (randomNumber < 30)
        {
            Instantiate(health, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(gold, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}