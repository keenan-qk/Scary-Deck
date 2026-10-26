using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 movement;
    public float speed = 3f;
    public int maxHp;
    public int currentHp;
    public HPBar hp;
    public GameObject boonPanel; //where you're able to choose the boons
    public List<WeaponData> boonWeapons; //list of available boons from the start
    public WeaponData weaponData;

    void Awake()
    {
        boonPanel.GetComponent<BoonManager>().SendBoons(boonWeapons);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = new Vector3();

        //GetComponent<WeaponManager>().AddWeapon(weaponData);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement *= speed;

        rb.linearVelocity = movement;

        if(Input.GetKeyDown(KeyCode.Q)) //decreases health
        {
            currentHp -= 10;
            hp.setState(currentHp, maxHp);
        }
        if(Input.GetKeyDown(KeyCode.R)) //opens up boon panel (fix later)
        {
            boonPanel.GetComponent<BoonManager>().SendBoons(boonWeapons);
            CardManager.isDuringPlay = false; //because boon panel is now active play has stopped
            boonPanel.SetActive(true);
        }
    }

    public void RemoveBoons(WeaponData boonWeapon)
    {
        boonWeapons.Remove(boonWeapon);
    }
}
