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
    public GameObject boonPanel;
    public List<WeaponData> boonWeapons;
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

        if(Input.GetKeyDown(KeyCode.Q))
        {
            currentHp -= 10;
            hp.setState(currentHp, maxHp);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            boonPanel.GetComponent<BoonManager>().SendBoons(boonWeapons);
            boonPanel.SetActive(true);
        }
    }

    public void RemoveBoons(WeaponData boonWeapon)
    {
        boonWeapons.Remove(boonWeapon);
    }
}
