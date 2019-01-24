using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzleManager : MonoBehaviour {

    public GameObject trantzizioCanvas;
    public int extraKop;
    GameObject currentPanel;
    Ekintzak emaitzak;
    Transition trantzizioa;
    int zenbakia;

    private void Start()
    {
        if (emaitzak == null)
        {
            emaitzak = Ekintzak.instantzia;
        }
        //emaitzak = Ekintzak.instantzia;
        trantzizioa = trantzizioCanvas.GetComponentInChildren<Transition>();
    }

    public void PanelGaitu(int zenb)
    {
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
    }

    IEnumerator PanelEzgaitu()
    {
        yield return new WaitForSeconds(.5f);
        trantzizioCanvas.SetActive(true);
        trantzizioa.FadeOut();
        yield return new WaitForSeconds(1);
        currentPanel.SetActive(false);
        gameObject.SetActive(false);
        trantzizioa.FadeIn();
        yield return new WaitForSeconds(1);
        // mugimendua berreskuratu
    }

    public void Konprobatu()
    {
        string erantzuna = currentPanel.GetComponent<EmaitzaKonprobatu>().GetErantzuna();
        emaitzak.SetIndex(zenbakia);
        bool zuzena = emaitzak.EmaitzaKonprobatu(erantzuna);
        if (zuzena)
        {
            emaitzak.Eragin();
            StartCoroutine(PanelEzgaitu());
        }
    }
}
