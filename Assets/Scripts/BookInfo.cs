using System;
using UnityEngine;

[Serializable] [CreateAssetMenu()]
public class BookInfo : ScriptableObject
{
    public string isbn;
    public string title;
    public string author;
    public int copyNumber;
    public int borrowedNumber;
    public bool isBorrowed = false;
}
