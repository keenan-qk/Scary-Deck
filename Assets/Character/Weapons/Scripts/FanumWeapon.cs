using UnityEngine;

public class FanumWeapon : WeaponBase
{
    public GameObject test;

    public override void Attack() //will change soon
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
