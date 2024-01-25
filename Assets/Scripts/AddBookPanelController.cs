using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddBookPanelController : MonoBehaviour
{
    [SerializeField] private InputField isbnInput;
    [SerializeField] private InputField titleInput;
    [SerializeField] private InputField authorInput;
    [SerializeField] private InputField copyNumberInput;

    [SerializeField] private BookSO bookSO;

    public void OnAddButtonClick()
    {
        BookInfo existingBook = FindExistingBook(isbnInput.text);

        if (existingBook != null )
        {
            existingBook.copyNumber++;
        }

        else
        {
            BookInfo newBook = CreateBook();

            AddBookToBookSO(newBook);
        }

        // Optionally, close the UI panel or reset input fields
        // panel.SetActive(false);
        // isbnInput.text = "";
        // titleInput.text = "";
        // authorInput.text = ""
        // copyNumberInput.text = ""
    }

    private BookInfo FindExistingBook(string isbn)
    {
        if (bookSO != null && bookSO.books != null)
        {
            foreach (BookInfo book in bookSO.books)
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
        BookInfo book = ScriptableObject.CreateInstance<BookInfo>();
        book.isbn = isbnInput.text;
        book.title = titleInput.text;
        book.author = authorInput.text;
        book.copyNumber = int.Parse(copyNumberInput.text);

        return book;
    }

    private void AddBookToBookSO(BookInfo book)
    {
        if (bookSO != null && bookSO.books != null)
        {
            System.Array.Resize(ref bookSO.books, bookSO.books.Length + 1);
            bookSO.books[bookSO.books.Length - 1] = book;
        }
    }
}
