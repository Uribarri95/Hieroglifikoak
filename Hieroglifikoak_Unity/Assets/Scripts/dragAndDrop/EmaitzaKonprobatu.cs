using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmaitzaKonprobatu : MonoBehaviour {

    public EmaitzaIrakurri erabErantzuna;
    public int ekintzaZenbakia;
    public bool hurrengoPuzleaJarri;

    public string GetErantzuna()
    {
        print(erabErantzuna.EmaitzaItzuli());       // ekintzak barruan emaitzak ipintzeko
        return erabErantzuna.EmaitzaItzuli();
    }
}
