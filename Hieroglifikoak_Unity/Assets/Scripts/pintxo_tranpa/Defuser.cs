using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defuser : MonoBehaviour {

    TranpaManager tranpak;
    public HormaMugimendua mugTranpa;
    public Zapaldu zapalTranpa;
    public PlataformaKudeatzailea plataforma;
    PlataformaKudeatzailea[] plataformaTranpak;

    // Use this for initialization
    void Start()
    {
        tranpak = GetComponentInParent<TranpaManager>();
        plataformaTranpak = GetComponentsInChildren<PlataformaKudeatzailea>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
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
            else if (plataforma != null)
            {
                plataforma.TranpakKudeatu(true);
            }
            else if(plataformaTranpak != null)
            {
                for (int i = 0; i < plataformaTranpak.Length; i++)
                {
                    plataformaTranpak[i].TranpakKudeatu(true);
                }
            }
        }
    }
}
