using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddBookPanelController : MonoBehaviour
{
    [SerializeField] private TMP_InputField isbnInput;
    [SerializeField] private TMP_InputField titleInput;
    [SerializeField] private TMP_InputField authorInput;
    [SerializeField] private TMP_InputField copyNumberInput;

    [SerializeField] private BookManager bookManager;

    [SerializeField] private PopupList popupListHolder;

    [SerializeField] private List<GameObject> books;

    public void OnAddButtonClick()
    {
        BookInfo existingBook = FindExistingBook(isbnInput.text);

        if (existingBook != null )
        {
            existingBook.copyNumber += int.Parse(copyNumberInput.text);
            popupListHolder.popupList[6].SetActive(true);
            StartCoroutine(popupListHolder.Timer());
        }

        else
        {
            BookInfo newBook = CreateBook();
            AddBookToBookSO(newBook);
            popupListHolder.popupList[6].SetActive(true);
            StartCoroutine(popupListHolder.Timer());
        }

        isbnInput.text = "";
        titleInput.text = "";
        authorInput.text = "";
        copyNumberInput.text = "";
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

    private BookInfo CreateBook()
    {
        int bookIndex = Random.Range(0, books.Count);
        BookInfo book = ScriptableObject.CreateInstance<BookInfo>();
        book.isbn = isbnInput.text;
        book.title = titleInput.text;
        book.author = authorInput.text;
        book.copyNumber = int.Parse(copyNumberInput.text);
        book.borrowedNumber = 0;

        return book;
    }

    private void AddBookToBookSO(BookInfo book)
    {
        if (bookManager != null && bookManager.books != null)
        {
            bookManager.books.Add(book);
        }
    }
}
