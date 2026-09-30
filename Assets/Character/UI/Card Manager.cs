using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI boxText;
    List<int> newList = new List<int>();
    int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newList.Add(1);
        newList.Add(2);
        newList.Add(3);
        newList.Add(4);
        newList.Add(5);

        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && index<4)
        {
            index++; 
        }
        if (Input.GetKeyDown(KeyCode.G) && index > 0)
        {
            index--;
        }

        boxText.text = newList[index].ToString();
    }
}
