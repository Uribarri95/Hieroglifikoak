using System.Collections;
using UnityEngine;

public class PuzleManager : MonoBehaviour {

    #region Singleton
    public static PuzleManager instantzia;

    private void Awake()
    {
        if (instantzia != null)
        {
            Debug.LogWarning("Error Singleton: PuzleManager bat baino gehiago aurkitu da!");
            return;
        }
        instantzia = this;
    }
    #endregion

    public FadeManager fadeManager;
    public ArgibideakAldatu argibideak;
    public int extraKop;
    public bool aktibatuta;     // puzlea jarrita dagoenean jokalaria geldi

    public delegate void Reseta();
    public Reseta resetDeia;

    GameObject currentPanel;
    Ekintzak emaitzak;
    int zenbakia;
    bool konprobatzen = false;
    string emaitzaTxartoMezua = "KODEA EZ DA ZUZENA, DUDARIK BADAUKAZU SAKATU 'P' EDO 'ESC' TEKLA ETA BEGIRATU HIZTEGIA ATALA.";
    string emaitzaZuzenaMezua = "KODEA ZUZENA DA, ZORIONAK!";

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
        print("index berria " + zenbakia);
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

    // !!! 
    IEnumerator HurrengoAriketaJarri()
    {
        print("bi puzzle jarraian");

        fadeManager.Ilundu();
        yield return new WaitForSeconds(1.5f);
        currentPanel.SetActive(false);
        PanelGaitu(zenbakia + 1);
        fadeManager.Argitu();
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
            /*if (zuzena && currentPanel.GetComponent<EmaitzaKonprobatu>().extra) // bi algoritmo zuzendu behar dira
            {
                print("bestea");
                string besteErantzuna = currentPanel.GetComponent<EmaitzaKonprobatu>().GetBesteErantzuna(); // !!! bigarren erantzuna kendu?
                emaitzak.SetIndex(zenbakia + 1);
                zuzena = emaitzak.EmaitzaKonprobatu(besteErantzuna);
                emaitzak.SetIndex(zenbakia);
            }*/
            if (zuzena)
            {
                AudioManager.instantzia.Play("EmaitzaZuzena");

                emaitzak.Eragin();
                StartCoroutine(argibideak.BehinBehinekoTextua(emaitzaZuzenaMezua));
                if (currentPanel.GetComponent<EmaitzaKonprobatu>() != null && currentPanel.GetComponent<EmaitzaKonprobatu>().hurrengoPuzleaJarri)
                {
                    StartCoroutine(HurrengoAriketaJarri());
                }
                else
                {
                    StartCoroutine(PanelEzgaitu());
                }
            }
            else
            {
                AudioManager.instantzia.Play("EmaitzaOkerra");
                
                // mezu kutxa aldatu segundu batzuk, emaitza txarto dagoela esaten duen mezuarekin
                konprobatzen = false;
                StartCoroutine(argibideak.BehinBehinekoTextua(emaitzaTxartoMezua));
            }
        }
    }

    /*public void Reset()
    {
        for (int i = 0; i < currentPanel.transform.childCount; i++)
        {
            Transform ariketaPanelak = currentPanel.transform.GetChild(i);
            for (int j = 0; j < ariketaPanelak.transform.childCount; j++)
            {
                Drag pieza = ariketaPanelak.transform.GetChild(j).GetComponent<Drag>();
                if(pieza != null)
                {
                    pieza.Reset();
                }
            }
        }
    }*/

    public void Reset()
    {
        {
            DenakReset();
        }
    }

    public void DenakReset()
    {
        if(resetDeia != null)
        {
            resetDeia.Invoke();
        }
    }

    public void DialogBerrizJarri()
    {
        print(currentPanel.name);
        if(currentPanel != null)
        {
            if(currentPanel.GetComponent<TutorialDialogTrigger>() != null)
            {
                currentPanel.GetComponent<TutorialDialogTrigger>().ArgibideakJarri();
            }
        }
    }
}
