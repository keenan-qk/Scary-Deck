using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class Character : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 movement;
    public float speed = 3f;
    public float maxHp;
    public float currentHp;
    public HPBar hp;
    public GameObject boonPanel; //where you're able to choose the boons
    private List<WeaponData> boonWeapons; //list of available boons from the start

    public bool stunned = false;
    private float stunTimer;
    private float slowDown;
    private bool fist;

    void Awake()
    {
        //boonPanel.GetComponent<BoonManager>().SendBoons(boonWeapons);
    }

    void Start()
    {
        fist = false;

        slowDown = 1f;

        rb = GetComponent<Rigidbody2D>();
        movement = new Vector3();
        rb.freezeRotation = true;

        //GetComponent<WeaponManager>().AddWeapon(weaponData);
    }

    void Update()
    {
        if (stunTimer >= 0f)
        {
            stunTimer -= Time.deltaTime;
        }
        else if(!fist)
        {
            if(CardManager.isDuringPlay == false)
            {
                rb.linearVelocity = new Vector3(0, 0, 0);
                return;
            }

            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            //if (movement.x == -1)
            //{
            //    gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            //}
            //else if (movement.x == 1)
            //{
            //    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            //}

            movement *= speed * slowDown;

            rb.linearVelocity = movement;
        }

        if(Input.GetKeyDown(KeyCode.E)) //opens up boon panel (fix later)
        {
            boonPanel.GetComponent<BoonManager>().SendBoons(boonWeapons);
            CardManager.isDuringPlay = false; //because boon panel is now active play has stopped
            boonPanel.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Vector2 direction = (new Vector3(3, 3, 0) - gameObject.transform.position).normalized;

            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

            rb.linearVelocity = direction * 5f;
        }
        
    }

    public void TakeDamage(int damage)
    {
        if(!fist)
        {
            currentHp -= damage;
            hp.setState(currentHp, maxHp);
        }
    }

    public void RemoveBoons(WeaponData boonWeapon)
    {
        boonWeapons.Remove(boonWeapon);
    }

    public void Stun(float time)
    {
        stunTimer = time;
        rb.linearVelocity = new Vector2(0f, 0f);
        stunned = true;
        Debug.Log("Player is Stunned");
    }
    
    public void SendBoons(List<WeaponData> bWeapons) //Connected to CharacterBoons script
    {
        boonWeapons = bWeapons;
    }

    public void ActivateBoonPanel()
    {
        boonPanel.GetComponent<BoonManager>().SendBoons(boonWeapons);
        CardManager.isDuringPlay = false; //because boon panel is now active play has stopped
        boonPanel.SetActive(true);
    }

    public void FistOn()
    {
        fist = true;
    }

    public void Fist(Vector3 endPoint, float proSpeed, float times)
    {
        Vector2 direction = (endPoint - gameObject.transform.position).normalized;

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction*proSpeed;

        StartCoroutine(Wait(times));
    }

    IEnumerator Wait(float times)
    {
        yield return new WaitForSeconds(times);

        fist = false;

        yield return null;
    }
}
