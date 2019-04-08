using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour {

    PintxoTranpaManager pintxoTranpak;
    HormaMugimendua mugTranpa;
    Zapaldu zapalTranpa;
    Zapaldu[] zapaltranpaMultzoa;
    PlataformaKudeatzailea plataforma;

	// Use this for initialization
	void Start () {
        pintxoTranpak = GetComponentInParent<PintxoTranpaManager>();
        mugTranpa = GetComponentInParent<HormaMugimendua>();
        zapalTranpa = GetComponentInParent<Zapaldu>();
        zapaltranpaMultzoa = GetComponentsInChildren<Zapaldu>();
        plataforma = GetComponentInParent<PlataformaKudeatzailea>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Aktibatu();
        }
    }

    private void Aktibatu()
    {
        if (pintxoTranpak != null)
        {
            pintxoTranpak.Aktibatu();
        }
        else if (mugTranpa != null)
        {
            mugTranpa.TranpakKudeatu(true);
        }
        else if (zapalTranpa != null)
        {
            zapalTranpa.TranpakKudeatu(true);
        }
        else if (zapaltranpaMultzoa != null)
        {
            for (int i = 0; i < zapaltranpaMultzoa.Length; i++)
            {
                zapaltranpaMultzoa[i].TranpakKudeatu(false);
                zapaltranpaMultzoa[i].TranpakKudeatu(true);
            }
        }
        else if (plataforma != null)
        {
            plataforma.reset = true;
        }
    }

    public void ZapalduDesaktibatu()
    {
        for (int i = 0; i < zapaltranpaMultzoa.Length; i++)
        {
            zapaltranpaMultzoa[i].TranpakKudeatu(false);
        }
    }

}
