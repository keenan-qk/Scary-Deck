using UnityEngine;

public class FanumWeapon : WeaponBase
{
    public GameObject test;

    public override void Attack()
    {
        if (test.activeSelf)
        {
            test.SetActive(false);
        }
        else
        {
            test.SetActive(true);
        }
    }
}
