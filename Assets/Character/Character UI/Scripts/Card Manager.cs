using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
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
    public float reloadTime;
    public UnityEngine.UI.Image card1Black;
    public UnityEngine.UI.Image card2Black;
    public UnityEngine.UI.Image card3Black;
    public GameObject reloadPanel;
    public GameObject reloadWipe;
    private RectTransform cardSelectorTransform;
    private Vector2 cardTransform;
    private List<WeaponData> weapons; //cards in your hand
    private List<WeaponData> reserveWeapons; //cards in your reserve (rest of deck)
    private List<WeaponData> discardWeapons;
    private bool reload;
    private string reloadString;
    private bool reloadBlack;
    private float reloadBlackTimer;
    private float reloadBlackTimerTime;

    List<int> newList;
    int index;

    void Start()
    {
        isDuringPlay = true;

        newList = new List<int>();
        weapons = new List<WeaponData>();
        reserveWeapons = new List<WeaponData>();
        discardWeapons = new List<WeaponData>();

        newList.Add(1);
        newList.Add(2);
        newList.Add(3);

        cardSelectorTransform = cardSelector.GetComponent<RectTransform>();
        cardTransform = new Vector2();

        index = 0;

        reloadBlack = false;
        reloadBlackTimer = 0;
        reloadBlackTimerTime = .5f;
    }

    void Update()
    {
        UpdateCardSelections();

        if(Input.mouseScrollDelta.y == -1 && index<2)
        {
            index++; 
        }
        if(Input.mouseScrollDelta.y == 1 && index>0)
        {
            index--;
        }
        if(Input.GetKeyDown(KeyCode.R) && reload == false)
        {
            Reload();
        }

        cardTransform = new Vector2(-550 + (275 * newList[index]), 0);
        cardSelectorTransform.anchoredPosition = cardTransform;

        if(reload)
        {
            isDuringPlay = true;
            reloadPanel.SetActive(true);
            reloadBlackTimer += Time.deltaTime;
            reloadWipe.transform.localScale = new Vector3(1f - (reloadBlackTimer / reloadTime), 1f, 1f);

            if(reloadBlackTimer > reloadTime)
            {
                reloadBlackTimer = 0;
            }
        }

        if(reload == false)
        {
            reloadPanel.SetActive(false);
        }

        if(reloadBlack)
        {
            card1Black.gameObject.SetActive(true);
            card2Black.gameObject.SetActive(true);
            card3Black.gameObject.SetActive(true);

            reloadBlackTimer += Time.deltaTime;

            card1Black.transform.localScale = new Vector3(1f, 1f - (reloadBlackTimer / reloadBlackTimerTime), 1f);
            card2Black.transform.localScale = new Vector3(1f, 1f - (reloadBlackTimer / reloadBlackTimerTime), 1f);
            card3Black.transform.localScale = new Vector3(1f, 1f - (reloadBlackTimer / reloadBlackTimerTime), 1f);

            if (reloadBlackTimer > reloadBlackTimerTime)
            {
                reloadBlackTimer = 0;
                reloadBlack = false;

                card1Black.gameObject.SetActive(false);
                card2Black.gameObject.SetActive(false);
                card3Black.gameObject.SetActive(false);

            }
        }

        if (reload)
        {
            reloadString = "Reloading";
        }
        else
        {
            reloadString = "";
        }

        boxText.text = "Card: " + newList[index].ToString() + " Reserves: " + reserveWeapons.Count + " Discards: " + discardWeapons.Count
                + " " + reloadString;

        if (Input.GetMouseButtonDown(0) && isDuringPlay == true && reloadBlack == false && reload == false)
        {
            Vector3 mouseScreenLoc = Input.mousePosition;
            Vector3 mouseWorldLoc = Camera.main.ScreenToWorldPoint(mouseScreenLoc);
            mouseWorldLoc.z = 0;
            Attack(index, mouseWorldLoc);
            reloadBlack = true;
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
            discardWeapons.Add(weapons[index]);
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

    private void Reload()
    {
        for(int i = 0; i < weapons.Count; i++)
        {
            discardWeapons.Add(weapons[0]);
            weapons.Remove(weapons[0]);
        }

        for(int i = 0; i < reserveWeapons.Count; i++)
        {
            discardWeapons.Add(reserveWeapons[0]);
            reserveWeapons.Remove(reserveWeapons[0]);
        }

        List<WeaponData> randomWeapons = new List<WeaponData>();

        int count = discardWeapons.Count;
        for(int i = 0; i < count; i++)
        {
            int index = Random.Range(0, discardWeapons.Count);
            randomWeapons.Add(discardWeapons[index]);
            discardWeapons.RemoveAt(index);
        }

        Debug.Log("1: " + randomWeapons.Count);

        int loop = randomWeapons.Count;
        if(loop > 3)
        {
            loop = 3;
        }
        for (int i = 0; i < loop; i++)
        {
            weapons.Add(randomWeapons[0]);
            randomWeapons.RemoveAt(0);
        }

        Debug.Log("2: " + randomWeapons.Count);

        count = randomWeapons.Count;

        for(int i = 0; i < count; i++)
        {
            reserveWeapons.Add(randomWeapons[0]);
            randomWeapons.RemoveAt(0);
        }

        StartCoroutine(Wait(reloadTime));

        UpdateCardSelections();
    }

    private IEnumerator Wait(float reloadTime)
    {
        isDuringPlay = false;
        reload = true;

        yield return new WaitForSeconds(reloadTime);

        isDuringPlay = true;
        reload = false;

        yield return null;
    }
}
