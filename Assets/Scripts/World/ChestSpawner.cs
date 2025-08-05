using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] private GameObject chest;
    private float possibility = .3f;

    public void Spawn()
    {
        if (Random.value < possibility)
        {
            Instantiate(chest, transform.position, Quaternion.identity);
        }
    }
}