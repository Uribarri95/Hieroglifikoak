using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranpaKudeaketa : MonoBehaviour {

    PintxoTranpaManager pintxoTranpak;                      // seme ugari ditu -> pintxoak agertzen eta desagertzen dira
    Zapaldu[] zapalduTranpak;                               // zorutik jauzten diren zoru zatiak, gora eta behera doaz, jokalaria korrika zeharkatu behar du
    HormaMugimendua hormaMugituTranpa;                      // horma ezkerretik eskumara doa, jokalaria korrika egin behar du hormak ez harrapatzeko
    PlataformaKudeatzailea[] plataformaMultzoa;             // behera jauzten diren plataformak, orden jakin batean mugitzen diren horma multzoa...

    // Use this for initialization
    void Start () {
        pintxoTranpak = GetComponent<PintxoTranpaManager>();
        zapalduTranpak = GetComponentsInChildren<Zapaldu>();
        hormaMugituTranpa = GetComponentInChildren<HormaMugimendua>();
        plataformaMultzoa = GetComponentsInChildren<PlataformaKudeatzailea>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            TranpakAktibatu();
        }
    }

    void TranpakAktibatu()
    {
        if(pintxoTranpak != null)
        {
            pintxoTranpak.Aktibatu();
        }
        else if (zapalduTranpak.Length != 0)
        {
            for (int i = 0; i < zapalduTranpak.Length; i++)
            {
                //zapalduTranpak[i].TranpakKudeatu(false);
                zapalduTranpak[i].TranpakKudeatu(true);
            }
        }
        else if (hormaMugituTranpa != null)
        {
            hormaMugituTranpa.TranpakKudeatu(true);
        }
        else if (plataformaMultzoa.Length != 0)
        {
            for (int i = 0; i < plataformaMultzoa.Length; i++)
            {
                plataformaMultzoa[i].Berrabiarazi();
            }
        }
    }

    public void TranpakDesaktibatu()
    {
        if (pintxoTranpak != null)
        {
            pintxoTranpak.Desaktibatu();
        }
        else if (zapalduTranpak.Length != 0)
        {
            for (int i = 0; i < zapalduTranpak.Length; i++)
            {
                zapalduTranpak[i].TranpakKudeatu(false);
            }
        }
        else if (hormaMugituTranpa != null)
        {
            hormaMugituTranpa.TranpakKudeatu(false);
        }
        else if (plataformaMultzoa.Length != 0)
        {
            for (int i = 0; i < plataformaMultzoa.Length; i++)
            {
                plataformaMultzoa[i].Gelditu();
            }
        }
    }
}
