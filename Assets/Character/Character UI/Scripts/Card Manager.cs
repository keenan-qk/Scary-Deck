using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour
{
    public static bool isDuringPlay; //checks if you are currently in play
    public TMPro.TextMeshProUGUI boxText;
    public UnityEngine.UI.Image card1;
    public UnityEngine.UI.Image card2;
    public UnityEngine.UI.Image card3;
    public UnityEngine.UI.Image cardSelector;
    private RectTransform cardSelectorTransform;
    private Vector2 cardTransform;
    private List<WeaponData> weapons; //cards in your hand
    private List<WeaponData> reserveWeapons; //cards in your reserve (rest of deck)

    List<int> newList;
    int index;

    void Start()
    {
        isDuringPlay = false;

        newList = new List<int>();
        weapons = new List<WeaponData>();
        reserveWeapons = new List<WeaponData>();

        newList.Add(1);
        newList.Add(2);
        newList.Add(3);

        cardSelectorTransform = cardSelector.GetComponent<RectTransform>();
        cardTransform = new Vector2();

        index = 0;
    }

    void Update()
    {
        UpdateCardSelections();

        if(Input.GetKeyDown(KeyCode.G) && index<2) //moves card selector to right
        {
            index++; 
        }
        if(Input.GetKeyDown(KeyCode.F) && index > 0) //moves card selector to left
        {
            index--;
        }

        cardTransform = new Vector2(-550 + (275 * newList[index]), 0);
        cardSelectorTransform.anchoredPosition = cardTransform;

        boxText.text = "Card: " + newList[index].ToString() + " | Reserves: " + reserveWeapons.Count;

        if(Input.GetMouseButtonDown(0) && isDuringPlay == true)
        {
            Vector3 mouseScreenLoc = Input.mousePosition;
            Vector3 mouseWorldLoc = Camera.main.ScreenToWorldPoint(mouseScreenLoc);
            mouseWorldLoc.z = 0;
            Attack(index, mouseWorldLoc);
        }
        while ((weapons.Count < 3) && reserveWeapons.Count > 0) //draws cards from reserve deck into active hand
        {   
            weapons.Add(reserveWeapons[0]);
            reserveWeapons.Remove(reserveWeapons[0]); 
        }
    }

    private void Attack(int index, Vector3 mousePos)
    {
        if (index < weapons.Count)
        {
            gameObject.GetComponent<WeaponManager>().CardAttack(weapons[index], mousePos);
            weapons.Remove(weapons[index]);
        }
        else
        {
            Debug.Log("no card there.");
        }
    }

    private void UpdateCardSelections() //sets all the cards in your hands to correct cards and images
    {
        if(weapons.Count > 0)
        {
            card1.sprite = weapons[0].boonImage;
        }
        else
        {
            card1.sprite = null;
        }
        if (weapons.Count > 1)
        {
            card2.sprite = weapons[1].boonImage;
        }
        else
        {
            card2.sprite = null;
        }
        if (weapons.Count > 2)
        {
            card3.sprite = weapons[2].boonImage;
        }
        else
        {
            card3.sprite = null;
        }
    }

    public void AddWeapon(WeaponData weaponData)
    {
        if(weapons.Count != 3)
        {
            weapons.Add(weaponData);
        }
        else
        {
            reserveWeapons.Add(weaponData);
        }
    }
}
