using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListBookPanelController : MonoBehaviour
{
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private BookManager bookManager;

    public void OnListButton()
    {
        foreach (BookInfo bookSO in bookManager.books)
        {
            if (bookSO != null)
            {
                GameObject existingBookEntry = FindBookEntryByISBN(bookSO.isbn);

                if (existingBookEntry != null)
                {
                    TextMeshProUGUI copyText = existingBookEntry.transform.Find("CopyText").GetComponent<TextMeshProUGUI>();
                    int currentCopyNumber = bookSO.copyNumber;
                    copyText.text = (currentCopyNumber).ToString();
                }
                else
                {
                    GameObject containerInstance = Instantiate(containerPrefab, parentObject.transform);

                    TextMeshProUGUI titleText = containerInstance.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI authorText = containerInstance.transform.Find("AuthorText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI copyText = containerInstance.transform.Find("CopyText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI borrowedText = containerInstance.transform.Find("BorrowedText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI isbnText = containerInstance.transform.Find("ISBNText").GetComponent<TextMeshProUGUI>();

                    titleText.text = "" + bookSO.title;
                    authorText.text = "" + bookSO.author;
                    copyText.text = "" + bookSO.copyNumber.ToString();
                    borrowedText.text = "" + bookSO.borrowedNumber.ToString();
                    isbnText.text = "" + bookSO.isbn;
                }              
            }
        }
    }

    private GameObject FindBookEntryByISBN(string isbn)
    {
        foreach (Transform child in parentObject.transform)
        {
            TextMeshProUGUI isbnText = child.Find("ISBNText").GetComponent<TextMeshProUGUI>();
            if (isbnText != null && isbnText.text == isbn)
            {
                return child.gameObject;
            }
        }

        return null;
    }
}
