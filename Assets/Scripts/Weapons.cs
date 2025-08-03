using UnityEngine;

public abstract class Weapons : MonoBehaviour
{
    public WeaponData weaponData;
    public WeaponValues weaponValues;
    [SerializeField] protected LayerMask damageableLayer;
    private float timer;

    protected void Update()
    {
        timer -= Time.deltaTime;
        if (!(timer <= 0f)) return;
        Attack();
        timer = weaponValues.timer;
    }

    public virtual void SelectData(WeaponData sd)
    {
        weaponData = sd;

        weaponValues = new WeaponValues(sd.values.damage, sd.values.timer, sd.values.count, sd.values.durability,
            sd.values.stayTime, sd.values.area, sd.values.speed, sd.values.hasEvolved);
    }

    protected abstract void Attack();

    public abstract void Evolution();

    internal void Upgrade(UpgradeData upgradeData)
    {
        weaponValues.Upgrade(upgradeData.weaponValues);
    }
}