using System;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour //foundational base of ALL weapons
{
    public WeaponData weaponData;

    float timer;

    void Start()
    {
        //NOTHING HERE YET
    }

    public void Update() //CHANGE THIS LATER
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            Attack();
            timer = weaponData.weaponStats.timeToAttack;
        }
    }

    public abstract void Attack(); //child classes will attack with this

    public void SetData(WeaponData wd) //sets data of weapon to correct values
    {
        weaponData = wd;
    }
}

