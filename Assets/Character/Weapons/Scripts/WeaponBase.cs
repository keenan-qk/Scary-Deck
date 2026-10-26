using System;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour //foundational base of ALL weapons
{
    public WeaponData weaponData;

    float timer;
    float iniAngle; //the initial angle of the gameObject

    private void Start()
    {
        timer = 0;
        iniAngle = gameObject.transform.eulerAngles.z + ((45-(weaponData.weaponStats.attackBehavior.degreeOfRotation/2)+45)); //offsets the angle a bit
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, iniAngle));
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if(weaponData.weaponStats.attackBehavior.degreeOfRotation != 0) //if object should not be rotated then it will not be rotated
        {
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, iniAngle+
                (timer * (weaponData.weaponStats.attackBehavior.degreeOfRotation/weaponData.weaponStats.attackBehavior.timeToDestroy)))); 
                //changes the rotation based on how long it takes for the attack to finish and the degrees of the attack
        }

        if (timer > weaponData.weaponStats.attackBehavior.timeToDestroy)
        {
            Destroy(gameObject);
        }
    }

    public abstract void Attack(); //child classes will attack with this

    public void SetData(WeaponData wd, Vector3 weaponsTransform, Vector3 mousePos) //sets data of weapon to correct values
    {
        weaponData = wd;
    }
}

