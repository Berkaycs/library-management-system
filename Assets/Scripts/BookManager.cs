using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public List<BookInfo> books;

    public float x = 2.2f;
    public float y = 2.2f;
    public float z = 0.8f;

    private void Awake()
    {
        foreach (BookInfo bookSO in books)
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
