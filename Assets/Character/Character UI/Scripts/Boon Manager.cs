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
    private List<WeaponData> newBoonWeapons; //new boons that show up on the boon selector
    private List<WeaponData> oldBoonWeapons; //list that is sent from character script
    bool changeImages; //makes sure update only runs once

    int test;
    void Start()
    {
        newBoonWeapons = new List<WeaponData>();
        changeImages = false;

        for (int i = 0; i < 3; i++) //first time the boon panel is used
        {
            boonPicker = Random.Range(0, oldBoonWeapons.Count);
            newBoonWeapons.Add(oldBoonWeapons[boonPicker]);
            oldBoonWeapons.Remove(oldBoonWeapons[boonPicker]);
            boonImages[i].sprite = newBoonWeapons[i].boonImage;
        }
    }

    void Update()
    {
        if(changeImages) //gets set false later so it only runs once
        {
            if(oldBoonWeapons.Count >= 3) 
            {
                for (int i = 0; i < 3; i++) //same as above method
                {
                    boonPicker = Random.Range(0, oldBoonWeapons.Count);
                    newBoonWeapons.Add(oldBoonWeapons[boonPicker]);
                    oldBoonWeapons.Remove(oldBoonWeapons[boonPicker]);
                    boonImages[i].sprite = newBoonWeapons[i].boonImage;
                }
            }
            else //if there lesss than 3 available boons to choose from
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
        character.GetComponent<CardManager>().AddWeapon(newBoonWeapons[boonButton]); //adds card to available cards
        character.GetComponent<Character>().RemoveBoons(newBoonWeapons[boonButton]); //removes it from boon panel selection so it can't be picked again
        if(newBoonWeapons.Count >= 3) //removes boon panel button images so they can be randomized the next time
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

        gameObject.SetActive(false); //very important, simplest way to make the boon selection process happen
        changeImages = true;
    }

    public void SendBoons(List<WeaponData> boonWeapons) //sent from character and reinitializes the old boons that can be selected
    {
        oldBoonWeapons = new List<WeaponData>();
        for (int i = 0; i < boonWeapons.Count; i++)
        {
            oldBoonWeapons.Add(boonWeapons[i]);
        }
    }
}
