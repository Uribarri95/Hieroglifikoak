using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defuser : MonoBehaviour {

    TranpaManager tranpak;
    public HormaMugimendua mugTranpa;
    public Zapaldu zapalTranpa;

    // Use this for initialization
    void Start()
    {
        tranpak = GetComponentInParent<TranpaManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tranpak != null)
        {
            tranpak.TranpakKudeatu(false);
        }
        else if (mugTranpa != null)
        {
            mugTranpa.TranpakKudeatu(false);
        }
        else if (zapalTranpa != null)
        {
            zapalTranpa.TranpakKudeatu(false);
        }
    }
}
