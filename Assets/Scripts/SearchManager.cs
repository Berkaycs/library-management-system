using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchManager : MonoBehaviour
{
    public GameObject contentHolder;
    public GameObject[] element;
    public GameObject searchBar;
    public TextMeshProUGUI notFoundText;

    public int totalElements;

    public void OnListButtonClicked()
    {
        totalElements = contentHolder.transform.childCount;
        
        element = new GameObject[totalElements];

        for (int i = 0; i < totalElements; i++)
        {
            element[i] = contentHolder.transform.GetChild(i).gameObject;
        }
    }
    
    public void Search()
    {
        string searchText = searchBar.GetComponent<TMP_InputField>().text.ToLower();

        int searchTxtLength = searchText.Length;

        bool foundMatch = false;

        foreach (GameObject elem in element)
        {
            TextMeshProUGUI titleText = elem.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI authorText = elem.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

            if (titleText.text.Length >= searchTxtLength || authorText.text.Length >= searchTxtLength)
            {
                if ((titleText.text.Length >= searchTxtLength && searchText == titleText.text.Substring(0, searchTxtLength).ToLower()) || (authorText.text.Length >= searchTxtLength && searchText == authorText.text.Substring(0, searchTxtLength).ToLower()))
                {
                    elem.SetActive(true);
                    foundMatch = true;
                }
                else
                {
                    elem.SetActive(false);
                }
            }
        }

        notFoundText.gameObject.SetActive(!foundMatch);
    }
}
