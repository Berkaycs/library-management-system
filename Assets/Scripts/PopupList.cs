using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupList : MonoBehaviour
{
    public GameObject[] popupList;
    
    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(1.5f);
        popupList[0].SetActive(false);
        popupList[1].SetActive(false);
        popupList[2].SetActive(false);
        popupList[3].SetActive(false);
        popupList[4].SetActive(false);
        popupList[5].SetActive(false);
        popupList[6].SetActive(false);
        popupList[7].SetActive(false);
    }
}
