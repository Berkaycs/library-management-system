using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public List<BookInfo> books;
    public List<BookInfo> borrowedBooks;

    private void Awake()
    {
        foreach (BookInfo bookSO in books)
        {
            if (bookSO.borrowedNumber > 0)
            {
                borrowedBooks.Add(bookSO);
            }
        }
    }
}
