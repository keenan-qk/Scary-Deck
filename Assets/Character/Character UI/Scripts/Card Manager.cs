using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI boxText;
    public UnityEngine.UI.Image cardSelector;
    private RectTransform cardSelectorTransform;
    private Vector2 cardTransform;

    List<int> newList;
    int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newList = new List<int>();

        newList.Add(1);
        newList.Add(2);
        newList.Add(3);

        cardSelectorTransform = cardSelector.GetComponent<RectTransform>();
        cardTransform = new Vector2();

        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && index<2)
        {
            index++; 
        }
        if (Input.GetKeyDown(KeyCode.G) && index > 0)
        {
            index--;
        }

        cardTransform = new Vector2(-550 + (275 * newList[index]), 0);
        cardSelectorTransform.anchoredPosition = cardTransform;

        Debug.Log(cardSelectorTransform.anchoredPosition);

        boxText.text = newList[index].ToString();
    }
}
