using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutxaPuzlea : MonoBehaviour {

    public Atea atea;
    KutxaJarri[] kutxak;

    // Use this for initialization
    void Start()
    {
        kutxak = GetComponentsInChildren<KutxaJarri>();
    }

    // Update is called once per frame
    void Update()
    {
        bool zabaldu = true;
        for (int i = 0; i < kutxak.Length; i++)
        {
            zabaldu = zabaldu && kutxak[i].GetKutxaEgokia();
        }
        if (zabaldu)
        {
            atea.AteaZabaldu(zabaldu);
        }
    }
}
