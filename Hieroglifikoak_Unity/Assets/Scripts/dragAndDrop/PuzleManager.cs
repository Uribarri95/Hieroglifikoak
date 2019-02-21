using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzleManager : MonoBehaviour {

    public FadeManager fadeManager;
    public ArgibideakAldatu argibideak;
    public int extraKop;
    public bool aktibatuta;     // puzlea jarrita dagoenean jokalaria geldi

    GameObject currentPanel;
    Ekintzak emaitzak;
    int zenbakia;
    bool konprobatzen = false;
    string emaitzaTxartoMezua = "KODEA EZ DA ZUZENA, DUDARIK BADAUKAZU SAKATU 'P' EDO 'ESC' TEKLA ETA BEGIRATU HIZTEGIA ATALA.";

    private void Start()
    {
        if (emaitzak == null)
        {
            emaitzak = Ekintzak.instantzia;
        }
    }

    public void PanelGaitu(int zenb)
    {
        konprobatzen = false;
        aktibatuta = true;
        zenbakia = zenb;
        if(emaitzak == null)
        {
            emaitzak = Ekintzak.instantzia;
        }
        emaitzak.SetIndex(zenbakia);
        for (int i = 1; i < transform.childCount - extraKop; i++)
        {
            EmaitzaKonprobatu puzlea = transform.GetChild(i).GetComponent<EmaitzaKonprobatu>();
            if(puzlea.ekintzaZenbakia == zenb)
            {
                print(transform.GetChild(i).name);
                currentPanel = transform.GetChild(i).gameObject;
                currentPanel.SetActive(true);
            }
        }
        argibideak.gameObject.SetActive(true);
    }

    IEnumerator PanelEzgaitu()
    {
        aktibatuta = false;
        yield return new WaitForSeconds(.5f);
        fadeManager.Ilundu();
        yield return new WaitForSeconds(1.5f);
        currentPanel.SetActive(false);
        gameObject.SetActive(false);
        fadeManager.Argitu();
        Pause.jokuaGeldituta = false;
    }

    public void Konprobatu()
    {
        if (!konprobatzen)
        {
            konprobatzen = true;
            string erantzuna = currentPanel.GetComponent<EmaitzaKonprobatu>().GetErantzuna();
            emaitzak.SetIndex(zenbakia);
            bool zuzena = emaitzak.EmaitzaKonprobatu(erantzuna);
            if (zuzena && currentPanel.GetComponent<EmaitzaKonprobatu>().extra) // bi algoritmo zuzendu behar dira
            {
                print("bestea");
                string besteErantzuna = currentPanel.GetComponent<EmaitzaKonprobatu>().GetBesteErantzuna();
                emaitzak.SetIndex(zenbakia + 1);
                zuzena = emaitzak.EmaitzaKonprobatu(besteErantzuna);
                emaitzak.SetIndex(zenbakia);
            }
            if (zuzena)
            {
                emaitzak.Eragin();
                StartCoroutine(PanelEzgaitu());
            }
            else
            {
                // mezu kutxa aldatu segundu batzuk
                //StopAllCoroutines();
                konprobatzen = false;
                StartCoroutine(argibideak.BehinBehinekoTextua(emaitzaTxartoMezua));
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < currentPanel.transform.childCount; i++)
        {
            Transform child = currentPanel.transform.GetChild(i);
            for (int j = 0; j < child.transform.childCount; j++)
            {
                Drag pieza = child.transform.GetChild(j).GetComponent<Drag>();
                if(pieza != null)
                {
                    pieza.Reset();
                }
            }
        }
    }
}
