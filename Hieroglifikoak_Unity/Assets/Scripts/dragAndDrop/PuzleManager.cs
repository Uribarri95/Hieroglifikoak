using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzleManager : MonoBehaviour {

    public FadeManager fadeManager;
    public ArgibideakAldatu argibideak;
    public int extraKop;

    GameObject currentPanel;
    Ekintzak emaitzak;
    int zenbakia;

    private void Start()
    {
        if (emaitzak == null)
        {
            emaitzak = Ekintzak.instantzia;
        }
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
        fadeManager.Ilundu();
        yield return new WaitForSeconds(1.5f);
        currentPanel.SetActive(false);
        gameObject.SetActive(false);
        fadeManager.Argitu();
        Pause.jokuaGeldituta = false;
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
        else
        {
            // mezu kutxa aldatu segundu batzuk
            //StopAllCoroutines();
            StartCoroutine(argibideak.BehinBehinekoTextua());
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
