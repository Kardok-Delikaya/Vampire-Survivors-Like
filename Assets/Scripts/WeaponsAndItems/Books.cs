using System.Collections.Generic;
using UnityEngine;

public class Books : Weapons
{
    private float attackCoolDown;
    private float stayTime;
    private float rotationTimer;
    private int bookCount = 1;
    private bool active;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Book[] books;
    [SerializeField] private List<UpgradeData> newUpgrades;

    private void Awake()
    {
        foreach (var t in books)
            t.damageableLayer = damageableLayer;
    }

    private new void Update()
    {
        transform.Rotate(Vector3.forward * (Time.deltaTime * -180 * weaponValues.speed));
        attackCoolDown -= Time.deltaTime;

        if (attackCoolDown <= 0f)
        {
            Attack();
        }

        if (stayTime >= 0f)
        {
            stayTime -= Time.deltaTime;
        }
        else if (active)
        {
            active = false;
            for (var i = 0; i < bookCount; i++)
            {
                books[i].active = false;
                books[i].backTime = 20;
            }
        }

        if (rotationTimer > 0)
        {
            rotationTimer -= Time.deltaTime;
        }
        else
        {
            for (var i = 0; i < weaponValues.count; i++)
            {
                books[i].enemiesHasBeenHitted.Clear();
            }

            rotationTimer = 1f / weaponValues.speed;
        }
    }

    protected override void Attack()
    {
        transform.localScale = new Vector3(weaponValues.area, weaponValues.area, 1);
        bookCount = weaponValues.count;
        stayTime = weaponValues.stayTime;
        attackCoolDown = weaponValues.timer;
        active = true;
        for (var i = 0; i < bookCount; i++)
        {
            books[i].active = true;
            books[i].damage = weaponValues.damage;
            books[i].area = weaponValues.area;
            books[i].exitTime = 20;
        }
    }

    public override void Evolution()
    {
        weaponValues.area += 0.5f;
        Object.FindAnyObjectByType<GameManager>().AddToUpgradeList(newUpgrades);
    }
}