using UnityEngine;

public class LittleFriend : Weapons
{
    private PlayerManager player;
    private int vertical;
    private int horizontal = 1;
    private int bulletCount;
    private int bombCount;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bomb;

    private void Start()
    {
        player = GetComponentInParent<PlayerManager>();
    }

    protected override void Attack()
    {
        bombCount = weaponValues.count;
        bulletCount = weaponValues.durability;

        if (bombCount > 1)
        {
            BombThrow();
        }
        else
        {
            BulletThrow();
        }
    }

    private void BulletThrow()
    {
        bulletCount--;
        var spawnnedBullet = Object.Instantiate<GameObject>(bullet, transform);
        spawnnedBullet.GetComponent<Projectile>().Equalize(weaponValues.damage, 1, weaponValues.stayTime, weaponValues.speed,
            false, damageableLayer);
        ShootAtMoveDirection(spawnnedBullet);
        spawnnedBullet.transform.eulerAngles = new Vector3(0, 0,
            Random.Range(spawnnedBullet.transform.eulerAngles.z - 15, spawnnedBullet.transform.eulerAngles.z + 15));
    
        spawnnedBullet.transform.SetParent(null);
        
        if (bulletCount > 0)
        {
            Invoke(nameof(BulletThrow), 0.05f);
        }
        else if (bombCount > 0)
        {
            Invoke(nameof(BombThrow), 0.15f);
        }
    }

    private void BombThrow()
    {
        bombCount--;
        var _bomb = Object.Instantiate<GameObject>(bomb, transform);
        _bomb.GetComponent<Bomb>().Equalize(weaponValues.damage * 2, 1, weaponValues.stayTime, weaponValues.speed / 2,
            false, damageableLayer);
        ShootAtMoveDirection(_bomb);

        _bomb.transform.SetParent(null);
        
        if (bulletCount > 0)
        {
            Invoke(nameof(BulletThrow), 0.05f);
        }
    }

    private void ShootAtMoveDirection(GameObject projectile)
    {
        if (player.Pos.y > .2)
        {
            vertical = 1;
        }
        else if (player.Pos.y < -.2)
        {
            vertical = -1;
        }
        else
        {
            vertical = 0;
        }

        if (player.Pos.x > .2)
        {
            horizontal = 1;
        }
        else if (player.Pos.x < -.2)
        {
            horizontal = -1;
        }
        else
        {
            horizontal = player.IsLeft() ? -1 : 1;
        }

        if (vertical != 0 && Mathf.Abs(player.Pos.x) < .2)
        {
            projectile.transform.localRotation = Quaternion.Euler(0, 0, vertical == 1 ? 0 : 180);
        }
        else
        {
            if (horizontal == 1)
                projectile.transform.localRotation = Quaternion.Euler(0, 0, 180 + horizontal * 90 + vertical * 45);
            else
                projectile.transform.localRotation = Quaternion.Euler(0, 0, 180 + horizontal * 90 - vertical * 45);
        }

    }

    public override void Evolution()
    {
        throw new System.NotImplementedException();
    }
}