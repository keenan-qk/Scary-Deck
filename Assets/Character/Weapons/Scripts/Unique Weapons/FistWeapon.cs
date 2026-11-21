using UnityEngine;

public class FistWeapon : WeaponBase
{
    public override void Attack()
    {
        
    }

    public override void OnDeath()
    {
        Fist(gameObject.transform.position.normalized);
    }
}

