using UnityEngine;

public class ThrowingKnife : Weapons
{
    private PlayerManager player;
    private int knifeCount;
    private int vertical;
    private int horizontal = 1;

    [SerializeField] private GameObject knife;
    [SerializeField] private UpgradeData weapon;

    private void Start()
    {
        player = GetComponentInParent<PlayerManager>();
    }

    protected override void Attack()
    {
        knifeCount = weaponValues.count;
        ThrowKnivesToMoveDirection();
    }

    private void ThrowKnivesToMoveDirection()
    {
        var spawnedKnife = Object.Instantiate(knife,
            new Vector3(transform.position.x + Random.Range(-.5f, .5f),
                transform.position.y + Random.Range(-.75f, 1.25f), 0), transform.rotation);
        spawnedKnife.GetComponent<IThrowable>().Equalize(weaponValues.damage, weaponValues.durability,
            weaponValues.stayTime, weaponValues.speed, false, damageableLayer);

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
            spawnedKnife.transform.localRotation = Quaternion.Euler(0, 0, vertical == 1 ? 0 : 180);
        }
        else
        {
            if (horizontal == 1)
                spawnedKnife.transform.localRotation = Quaternion.Euler(0, 0, 180 + horizontal * 90 + vertical * 45);
            else
                spawnedKnife.transform.localRotation = Quaternion.Euler(0, 0, 180 + horizontal * 90 - vertical * 45);
        }

        knifeCount--;

        if (knifeCount > 0)
        {
            Invoke(nameof(ThrowKnivesToMoveDirection), .1f);
        }
    }

    public override void Evolution()
    {
        Object.FindAnyObjectByType<GameManager>().AddWeapon(weapon);
        Object.Destroy(this.gameObject);
    }
}