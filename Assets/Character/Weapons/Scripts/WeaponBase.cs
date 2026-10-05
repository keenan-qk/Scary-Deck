using System;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponData weaponData;

    float timer;

    void Start()
    {
        //NOTHING HERE YET
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            Attack();
            timer = weaponData.weaponStats.timeToAttack;
        }
    }

    public abstract void Attack();

    public void SetData(WeaponData wd)
    {
        weaponData = wd;
    }
}

