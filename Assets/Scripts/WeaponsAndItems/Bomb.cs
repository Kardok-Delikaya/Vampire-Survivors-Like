using UnityEngine;

public class Bomb : MonoBehaviour, IThrowable
{
    private int damage;
    private float speed;
    private float stayTime;
    private LayerMask damageableLayer;

    private void FixedUpdate()
    {
        stayTime -= Time.fixedDeltaTime;
        if (stayTime < 0)
            Destroy(gameObject);

        transform.position += transform.up * Time.deltaTime * speed;

        var objs = Physics2D.OverlapCircleAll(transform.position, .3f, damageableLayer);

        if (objs.Length != 0)
        {
            BlowUp();
        }
    }

    private void BlowUp()
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, 3f,damageableLayer);
        foreach (var c in enemies)
        {
            var obj = c.GetComponent<IDamage>();
            obj.TakeDamage(damage, 1);
        }

        Destroy(gameObject);
    }

    public void Equalize(int damage, int health, float stayTime, float speed, bool hasEvolved,
        LayerMask damageableLayer)
    {
        this.damage = damage;
        this.stayTime = stayTime;
        this.speed = speed;
        this.damageableLayer = damageableLayer;
    }
}