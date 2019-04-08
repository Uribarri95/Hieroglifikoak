using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmaitzaKonprobatu : MonoBehaviour {

    public EmaitzaIrakurri erabErantzuna;
    //public EmaitzaIrakurri bigarrenErantzuna;
    //public bool extra;
    public int ekintzaZenbakia;
    public bool hurrengoPuzleaJarri;

    public string GetErantzuna()
    {
        print(erabErantzuna.EmaitzaItzuli());       // ekintzak barruan emaitzak ipintzeko
        return erabErantzuna.EmaitzaItzuli();
    }

    /*public string GetBesteErantzuna()
    {
        print(bigarrenErantzuna.EmaitzaItzuli());
        return bigarrenErantzuna.EmaitzaItzuli();
    }*/
}
