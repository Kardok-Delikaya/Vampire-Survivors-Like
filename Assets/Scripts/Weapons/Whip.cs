using System.Collections.Generic;
using UnityEngine;

public class Whip : Weapons
{
    private int attackCount;
    private PlayerManager player;
    [SerializeField] private GameObject[] whips;
    [SerializeField] private List<UpgradeData> upgrades;

    private void Start()
    {
        player = GetComponentInParent<PlayerManager>();
    }

    protected override void Attack()
    {
        if (!player.IsLeft())
        {
            transform.localScale = new Vector3(weaponValues.area, weaponValues.area, 1);
        }
        else
        {
            transform.localScale = new Vector3(-weaponValues.area, weaponValues.area, 1);
        }

        WhipAttack();
        Invoke("CloseWhips", weaponValues.stayTime);
    }

    private void WhipAttack()
    {
        whips[attackCount].SetActive(true);
        var damageableObjects = Physics2D.OverlapBoxAll(whips[attackCount].transform.position,
            new Vector3(3.4f * weaponValues.area, 1.5f * weaponValues.area, 1), 0, damageableLayer);

        for (var i = 0; i < damageableObjects.Length; i++)
        {
            damageableObjects[i].GetComponent<IDamage>().TakeDamage(weaponValues.damage, 2);
        }

        if (attackCount < weaponValues.count - 1)
        {
            Invoke("WhipAttack", .1f);
            attackCount++;
        }
        else
        {
            attackCount = 0;
        }
    }

    private void CloseWhips()
    {
        for (var i = 0; i < weaponValues.count; i++)
        {
            whips[i].SetActive(false);
        }
    }

    public override void Evolution()
    {
        weaponValues.area += 0.6f;
        weaponValues.count += 1;
        Object.FindAnyObjectByType<GameManager>().AddToUpgradeList(upgrades);
    }
}