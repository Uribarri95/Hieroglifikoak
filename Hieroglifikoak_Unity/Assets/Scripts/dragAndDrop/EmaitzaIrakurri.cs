using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmaitzaIrakurri : MonoBehaviour {

    public string pieza;
    public bool hasiBukatu;
    public bool emanEmaintza;       // !!! ekintzak atalean emaitzak jartzeko, bukatu ostean kendu !!!
    string emaintza;

    // Update is called once per frame
    void Update () {                // !!! ekintzak atalean emaitzak jartzeko, bukatu ostean kendu !!!
        if (emanEmaintza)
        {
            print(EmaitzaItzuli());
            emanEmaintza = false;
        }
    }

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
