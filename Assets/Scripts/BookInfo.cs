using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BookInfo : ScriptableObject
{
    public BookType bookType;

    public GameObject bookPrefab;
    public string isbn;
    public string title;
    public string author;
    public int copyNumber;
    public int borrowedNumber;

    public enum BookType
    {
        Adventure,
        Mystery,
        Horror,
        Science
    }
}
