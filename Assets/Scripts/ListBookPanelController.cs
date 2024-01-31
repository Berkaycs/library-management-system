using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ListBookPanelController : MonoBehaviour
{
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private BookManager bookManager;
    [SerializeField] private ExpiredBooksPanelController expiredController;

    [SerializeField] private PopupList popupListHolder;

    private HashSet<string> borrowedISBNs = new HashSet<string>();

    public void OnListButton()
    {
        foreach (BookInfo bookSO in bookManager.books)
        {
            if (bookSO != null)
            {
                GameObject existingBook = FindBookAwakeByISBN(bookSO.isbn);

                if (existingBook != null)
                {
                    TextMeshProUGUI copyText = existingBook.transform.Find("CopyText").GetComponent<TextMeshProUGUI>();
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

                popupListHolder.popupList[0].SetActive(true);
                StartCoroutine(popupListHolder.Timer());

                AddExpiredBookToList(isbn);
            }

           else if (bookManager.borrowedBooks.Contains(selectedBook) && !borrowedISBNs.Contains(isbn))
            {
                selectedBook.borrowedNumber += 1;
                selectedBook.isBorrowed = true;
                borrowedISBNs.Add(isbn);

                popupListHolder.popupList[0].SetActive(true);
                StartCoroutine(popupListHolder.Timer());

                AddExpiredBookToList(isbn);
            }

            else if (borrowedISBNs.Contains(isbn)) 
            {
                selectedBook.isBorrowed = true;

                popupListHolder.popupList[2].SetActive(true);
                StartCoroutine(popupListHolder.Timer());

            }

            else if (selectedBook.copyNumber == selectedBook.borrowedNumber)
            {

                popupListHolder.popupList[4].SetActive(true);
                StartCoroutine(popupListHolder.Timer());          
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

                RemoveExpiredBookFromList(isbn);

                popupListHolder.popupList[1].SetActive(true);
                StartCoroutine(popupListHolder.Timer());

                if (selectedBook.copyNumber - selectedBook.borrowedNumber == 0 || selectedBook.borrowedNumber == 0)
                {
                    bookManager.borrowedBooks.Remove(selectedBook);
                }
            }

            else
            {

                popupListHolder.popupList[3].SetActive(true);
                StartCoroutine(popupListHolder.Timer());        
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
        GameObject selectedBook = FindBookAwakeByISBN(book.isbn);

        if (selectedBook != null)
        {
            TextMeshProUGUI borrowedText = selectedBook.transform.Find("BorrowedText").GetComponent<TextMeshProUGUI>();
            if (borrowedText != null)
            {
                borrowedText.text = book.borrowedNumber.ToString();
            }
        }
    }

    public void AddExpiredBookToList(string isbn)
    {
        StartCoroutine(DueDate(isbn));
    }

    public void RemoveExpiredBookFromList(string isbn)
    {
        GameObject containerInstance = FindBookContainerByISBN(isbn);
        BookInfo selectedBook = FindExistingBook(isbn);

        if (containerInstance != null)
        {
            Destroy(containerInstance.gameObject);
            popupListHolder.popupList[1].SetActive(true);
            StartCoroutine(popupListHolder.Timer());
            expiredController.expiredBooks.Remove(selectedBook);
        }
    }

    public IEnumerator DueDate(string isbn)
    {
        yield return new WaitForSecondsRealtime(30f);

        GameObject containerInstance = Instantiate(expiredController.containerPrefab, expiredController.parentObject.transform);

        BookInfo selectedBook = FindExistingBook(isbn);

        TextMeshProUGUI isbnText = containerInstance.transform.Find("ISBNText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI titleText = containerInstance.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI authorText = containerInstance.transform.Find("AuthorText").GetComponent<TextMeshProUGUI>();

        isbnText.text = selectedBook.isbn;
        authorText.text = selectedBook.author;
        titleText.text = selectedBook.title;

        TextMeshProUGUI expiredText = popupListHolder.popupList[7].GetComponentInChildren<TextMeshProUGUI>();
        expiredText.text = $"{selectedBook.title} has expired. Please return the book.";
        popupListHolder.popupList[7].SetActive(true);
        StartCoroutine(popupListHolder.Timer());
        expiredController.expiredBooks.Add(selectedBook);
    }



    private GameObject FindBookAwakeByISBN(string isbn)
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

    private GameObject FindBookContainerByISBN(string isbn)
    {
        foreach (Transform child in expiredController.parentObject.transform)
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
