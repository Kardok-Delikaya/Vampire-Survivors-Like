using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, .5f);
    }

    private void FixedUpdate()
    {
        transform.localPosition += new Vector3(0, 0.05f, 0);
        transform.localScale += new Vector3(0.02f, 0.02f, 0);
    }
}