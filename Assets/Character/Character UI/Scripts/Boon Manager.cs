using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BoonManager : MonoBehaviour
{
    public List<UnityEngine.UI.Image> boonImages;
    public int size;
    public Sprite blankImage;
    private int boonPicker;
    public GameObject character;
    private List<Sprite> boonSprites;
    private List<WeaponData> newBoonWeapons;
    private List<WeaponData> oldBoonWeapons;
    bool changeImages;

    int test;
    void Start()
    {
        newBoonWeapons = new List<WeaponData>();
        changeImages = false;

        for (int i = 0; i < 3; i++)
        {
            boonPicker = Random.Range(0, oldBoonWeapons.Count);
            newBoonWeapons.Add(oldBoonWeapons[boonPicker]);
            oldBoonWeapons.Remove(oldBoonWeapons[boonPicker]);
            boonImages[i].sprite = newBoonWeapons[i].boonImage;
        }
    }

    void Update()
    {
        if(changeImages)
        {
            if(oldBoonWeapons.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    boonPicker = Random.Range(0, oldBoonWeapons.Count);
                    newBoonWeapons.Add(oldBoonWeapons[boonPicker]);
                    oldBoonWeapons.Remove(oldBoonWeapons[boonPicker]);
                    boonImages[i].sprite = newBoonWeapons[i].boonImage;
                }
            }
            else
            {
                int blanks = oldBoonWeapons.Count;
                for (int i = 0; i < blanks; i++)
                {
                    boonPicker = Random.Range(0, oldBoonWeapons.Count);
                    newBoonWeapons.Add(oldBoonWeapons[boonPicker]);
                    oldBoonWeapons.Remove(oldBoonWeapons[boonPicker]);
                    boonImages[i].sprite = newBoonWeapons[i].boonImage;
                }
                for(int i = 0; i < 3-blanks; i++)
                {
                    boonImages[blanks + i].sprite = blankImage;
                }
            }
            changeImages = false;
        }
    }

    public void AddBoonWeapon(int boonButton)
    {
        character.GetComponent<CardManager>().AddWeapon(newBoonWeapons[boonButton]);
        character.GetComponent<Character>().RemoveBoons(newBoonWeapons[boonButton]);
        if(newBoonWeapons.Count >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                newBoonWeapons.Remove(newBoonWeapons[newBoonWeapons.Count - 1]);
            }
        }
        else
        {
            for (int i = 0; i < newBoonWeapons.Count; i++)
            {
                newBoonWeapons.Remove(newBoonWeapons[i]);
            }
        }

        gameObject.SetActive(false);
        changeImages = true;
    }

    public void SendBoons(List<WeaponData> boonWeapons)
    {
        oldBoonWeapons = new List<WeaponData>();
        for (int i = 0; i < boonWeapons.Count; i++)
        {
            oldBoonWeapons.Add(boonWeapons[i]);
        }
    }
}
