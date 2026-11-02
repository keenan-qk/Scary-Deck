using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour //foundational base of ALL weapons
{
    public WeaponData weaponData;
    private HashSet<GameObject> triggeredObjects;

    float timer;
    float iniAngle; //the initial angle of the gameObject
    private bool goAgain;

    private void Start()
    {
        Debug.Log(weaponData.name + ", " + weaponData.weaponStats.damage);
        goAgain = true;
        triggeredObjects = new HashSet<GameObject>();

        if(weaponData.weaponStats.attackBehavior.isItem)
        {
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        timer = 0;
        if (weaponData.weaponStats.attackBehavior.isRotate) //change to is less than 90 degrees
        {
            iniAngle = gameObject.transform.eulerAngles.z + (45 - (weaponData.weaponStats.attackBehavior.degreeOfRotation / 2) + 45); //offsets the angle a bit
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, iniAngle));
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (weaponData.weaponStats.attackBehavior.degreeOfRotation != 0) //if object should not be rotated then it will not be rotated
        {
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, iniAngle +
                (timer * (weaponData.weaponStats.attackBehavior.degreeOfRotation / weaponData.weaponStats.attackBehavior.timeToDestroy))));
            //changes the rotation based on how long it takes for the attack to finish and the degrees of the attack
        }
        
        if (timer > weaponData.weaponStats.attackBehavior.timeToDestroy && goAgain)
        {
            if(weaponData.weaponStats.attackBehavior.isSpawnObject)
            {
                Attack();
                goAgain = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (weaponData.weaponStats.attackBehavior.isItem)
        {
            Character c = trigger.gameObject.GetComponent<Character>();

            if (c != null)
            {
                c.TakeDamage(-(weaponData.weaponStats.damage));
                StartCoroutine(Wait());
            }
        }

        if (weaponData.weaponStats.attackBehavior.isOneTimeDestroy)
        {
            Enemy e = trigger.gameObject.GetComponent<Enemy>();

            if (e != null)
            {
                e.TakeDamage(weaponData.weaponStats.damage);
                Destroy(gameObject);
            }
        }
        else if (!triggeredObjects.Contains(trigger.gameObject))
        {
            triggeredObjects.Add(trigger.gameObject);

            Enemy e = trigger.gameObject.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(weaponData.weaponStats.damage);
            }
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(weaponData.weaponStats.attackBehavior.timeToDestroy);

        Destroy(gameObject);

        yield return null;
    }

    public abstract void Attack(); //child classes will attack with this

    public void SetData(WeaponData wd) //sets data of weapon to correct values
    {
        weaponData = wd;
    }
}

