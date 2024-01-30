using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListBookPanelController : MonoBehaviour
{
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private BookManager bookManager;

    public GameObject[] popupList;

    private HashSet<string> borrowedISBNs = new HashSet<string>();

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

                    Button borrowButton = containerInstance.transform.Find("BorrowButton").GetComponent<Button>();
                    Button returnButton = containerInstance.transform.Find("ReturnButton").GetComponent<Button>();


                    titleText.text = "" + bookSO.title;
                    authorText.text = "" + bookSO.author;
                    copyText.text = "" + bookSO.copyNumber.ToString();
                    borrowedText.text = "" + bookSO.borrowedNumber.ToString();
                    isbnText.text = "" + bookSO.isbn;

                    borrowButton.onClick.AddListener(() => BorrowButtonClicked(isbnText.text));
                    returnButton.onClick.AddListener(() => ReturnButtonClicked(isbnText.text));
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

    public void BorrowBook(string isbn)
    {
        BookInfo selectedBook = FindExistingBook(isbn);

        if (selectedBook != null && selectedBook.copyNumber > selectedBook.borrowedNumber)
        {
            if (!bookManager.borrowedBooks.Contains(selectedBook))
            {
                bookManager.borrowedBooks.Add(selectedBook);
                selectedBook.borrowedNumber += 1;
                selectedBook.isBorrowed = true;
                borrowedISBNs.Add(isbn);

                popupList[0].SetActive(true);
                StartCoroutine(Timer());            
            }

           else if (bookManager.borrowedBooks.Contains(selectedBook) && !borrowedISBNs.Contains(isbn))
            {
                selectedBook.borrowedNumber += 1;
                selectedBook.isBorrowed = true;
                borrowedISBNs.Add(isbn);

                popupList[0].SetActive(true);
                StartCoroutine(Timer());
            }

            else if (borrowedISBNs.Contains(isbn)) 
            {
                selectedBook.isBorrowed = true;

                popupList[2].SetActive(true);
                StartCoroutine(Timer());
            }

            else if (selectedBook.copyNumber == selectedBook.borrowedNumber)
            {

                popupList[4].SetActive(true);
                StartCoroutine(Timer());          
            }
        }
        UpdateUIText(selectedBook);
    }

    public void ReturnBook(string isbn)
    {
        BookInfo selectedBook = FindExistingBook(isbn);

        if (selectedBook != null)
        {
            if (bookManager.borrowedBooks.Contains(selectedBook) && selectedBook.borrowedNumber > 0 && selectedBook.isBorrowed == true)
            {
                selectedBook.borrowedNumber -= 1;
                borrowedISBNs.Remove(isbn);
                selectedBook.isBorrowed = false;

                popupList[1].SetActive(true);
                StartCoroutine(Timer());

                if (selectedBook.copyNumber - selectedBook.borrowedNumber == 0 || selectedBook.borrowedNumber == 0)
                {
                    bookManager.borrowedBooks.Remove(selectedBook);
                }
            }

            else
            {

                popupList[3].SetActive(true);
                StartCoroutine(Timer());        
            }
        }

        UpdateUIText(selectedBook);
    }

    private void BorrowButtonClicked(string text)
    {
        BorrowBook(text);
    }

    private void ReturnButtonClicked(string text)
    {
        ReturnBook(text);
    }

    private void UpdateUIText(BookInfo book)
    {
        GameObject selectedBook = FindBookEntryByISBN(book.isbn);

        if (selectedBook != null)
        {
            TextMeshProUGUI borrowedText = selectedBook.transform.Find("BorrowedText").GetComponent<TextMeshProUGUI>();
            if (borrowedText != null)
            {
                borrowedText.text = book.borrowedNumber.ToString();
            }
        }
    }

    private BookInfo FindExistingBook(string isbn)
    {
        if (bookManager != null && bookManager.books != null)
        {
            foreach (BookInfo book in bookManager.books)
            {
                if (book.isbn == isbn)
                {
                    return book;
                }
            }
        }

        return null;
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);
        popupList[0].SetActive(false);
        popupList[2].SetActive(false);
        popupList[4].SetActive(false);
        popupList[1].SetActive(false);
        popupList[3].SetActive(false);
        popupList[5].SetActive(false);
        popupList[6].SetActive(false);
    }
}
