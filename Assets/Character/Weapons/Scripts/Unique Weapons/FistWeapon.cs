using UnityEngine;

public class FistWeapon : WeaponBase
{
    public WeaponData fist;

    private void Start()
    {
        StartFist();

        GameObject weaponGameObject = Instantiate(fist.weaponPrefab, gameObject.transform);

        WeaponBase weaponBase = weaponGameObject.GetComponent<WeaponBase>();

        weaponBase.SetData(fist);

        weaponBase.SetInitVel(gameObject.GetComponent<Rigidbody2D>().linearVelocity);
    }

    private void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        Fist();
    }

    public override void OnDeath()
    {
        
    }
}

