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

    public WeaponData wd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = new Vector3();
        Debug.Log(wd.stats.damage);
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement *= speed;

        rb.linearVelocity = movement;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentHp -= 10;
            hp.setState(currentHp, maxHp);
        }
    }
}
