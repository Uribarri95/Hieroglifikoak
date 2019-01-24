using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmaitzaKonprobatu : MonoBehaviour {

    public EmaitzaIrakurri erabErantzuna;
    public int ekintzaZenbakia;

    public string GetErantzuna()
    {
        return erabErantzuna.EmaitzaItzuli();
    }
}
