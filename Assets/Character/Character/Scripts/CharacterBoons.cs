using System.Collections.Generic;
using UnityEngine;

public class CharacterBoons : MonoBehaviour
{
    public GameObject character;
    public GameObject boonPanel; //where you're able to choose the boons
    public List<WeaponData> boonWeapons; //list of available boons from the start

    void Awake()
    {
        boonPanel.GetComponent<BoonManager>().SendBoons(boonWeapons);
        character.GetComponent<Character>().SendBoons(boonWeapons);
    }
}
