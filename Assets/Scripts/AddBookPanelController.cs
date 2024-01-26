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

    [SerializeField] private List<GameObject> books;

    private float x = 2.2f;
    private float y = 2.2f;
    private float z = 0.8f;

    private void Awake()
    {
        foreach (BookInfo bookSO in bookManager.books)
        {
            for (int i = 0; i < bookSO.copyNumber - bookSO.borrowedNumber; i++)
            {
                Vector3 pos = new Vector3(x, y, z);
                Quaternion rot = Quaternion.Euler(0f, 270f, 0f);
                Instantiate(bookSO.bookPrefab, pos, rot);
                BookPos();
            }
        }
    }

    public void OnAddButtonClick()
    {
        BookInfo existingBook = FindExistingBook(isbnInput.text);

        if (existingBook != null )
        {
            existingBook.copyNumber += int.Parse(copyNumberInput.text);

            for (int i = 0; i < int.Parse(copyNumberInput.text); i++)
            {
                Vector3 pos = new Vector3(x, y, z);
                Quaternion rot = Quaternion.Euler(0f, 270f, 0f);
                Instantiate(existingBook.bookPrefab, pos, rot);
                BookPos();
            }
        }

        else
        {
            BookInfo newBook = CreateBook();
            AddBookToBookSO(newBook);

            for (int i = 0; i < newBook.copyNumber; i++)
            {
                Vector3 pos = new Vector3(x, y, z);
                Quaternion rot = Quaternion.Euler(0f, 270f, 0f);
                Instantiate(newBook.bookPrefab, pos, rot);
                BookPos();
            }
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
        book.bookPrefab = books[bookIndex];
        book.isbn = isbnInput.text;
        book.title = titleInput.text;
        book.author = authorInput.text;
        book.copyNumber = int.Parse(copyNumberInput.text);
        book.borrowedNumber = 0;

        string filePath = $"Assets/Resources/{book.title}.asset";

        UnityEditor.AssetDatabase.CreateAsset(book, filePath);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();

        return book;
    }

    private void AddBookToBookSO(BookInfo book)
    {
        if (bookManager != null && bookManager.books != null)
        {
            bookManager.books.Add(book);
        }
    }

    private void BookPos()
    {
        if (z <= 0.8f && z > -0.8f)
        {
            z -= 0.055f;
        }

        else if (y <= 2.2f && y >= 0.6f)
        {
            y -= 0.4f;
            z = 0.8f;
        }

        else
        {
            Debug.Log("The bookshelf is full!");
        }
    }
}
