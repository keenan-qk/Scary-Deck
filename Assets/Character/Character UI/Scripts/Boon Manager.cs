using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BoonManager : MonoBehaviour
{
    public List<WeaponData> boonWeapons;
    public List<UnityEngine.UI.Image> boonImages;
    public int size;
    private int boonPicker;
    public GameObject character;
    private List<Sprite> boonSprites;
    private List<WeaponData> newBoonWeapons;

    int test;
    void Start()
    { 
        boonSprites = new List<Sprite>();
        newBoonWeapons = new List<WeaponData>();

        for(int i = 0; i < size; i++)
        {
            boonSprites.Add(boonWeapons[i].boonImage);
        }

        for(int i = 0; i < 3; i++)
        {
            boonPicker = Random.Range(0, size-i);
            boonImages[i].sprite = boonSprites[boonPicker];
            boonSprites.Remove(boonSprites[boonPicker]);
            newBoonWeapons.Add(boonWeapons[boonPicker]);
        }
    }

    public void AddBoonWeapon(int boonButton)
    {
        character.GetComponent<WeaponManager>().AddWeapon(newBoonWeapons[boonButton]);
        gameObject.SetActive(false);
    }

    //CREATE SETACTIVE FUNCTION
}
