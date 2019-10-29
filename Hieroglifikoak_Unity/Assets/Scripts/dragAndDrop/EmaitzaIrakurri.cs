using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmaitzaIrakurri : MonoBehaviour {

    public string pieza;
    public bool hasiBukatu;
    string emaintza;

    public string EmaitzaItzuli()
    {
        emaintza = pieza;
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                emaintza += transform.GetChild(i).GetComponent<EmaitzaIrakurri>().EmaitzaItzuli();
            }
        }
        if (hasiBukatu)
        {
            emaintza += pieza;
        }
        return emaintza;
    }
}
