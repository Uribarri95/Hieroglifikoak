using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour {

    TranpaManager tranpak;
    HormaMugimendua mugTranpa;
    Zapaldu zapalTranpa;

	// Use this for initialization
	void Start () {
        tranpak = GetComponentInParent<TranpaManager>();
        mugTranpa = GetComponentInParent<HormaMugimendua>();
        zapalTranpa = GetComponentInParent<Zapaldu>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (tranpak != null)
            {
                tranpak.TranpakKudeatu(true);
            }
            else if (mugTranpa != null)
            {
                mugTranpa.TranpakKudeatu(true);
            }
            else if (zapalTranpa != null)
            {
                zapalTranpa.TranpakKudeatu(true);
            }
        }
    }

}
