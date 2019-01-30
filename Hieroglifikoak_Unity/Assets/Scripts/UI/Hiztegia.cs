using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiztegia : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject layoutTokia;

    public GameObject hasiBukatu;
    public GameObject eragiketak;
    public GameObject baldintza;
    public GameObject eragigaia;
    public GameObject if_a;
    public GameObject else_a;
    public GameObject for_a;
    public GameObject while_a;

    public void AtzeraBotoia()
    {
        gameObject.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void UnableActivePanel()
    {
        for (int i = 0; i < layoutTokia.transform.childCount; i++)
        {
            if (layoutTokia.transform.GetChild(i).gameObject.activeSelf)
            {
                layoutTokia.transform.GetChild(i).gameObject.SetActive(false);
                return;
            }
        }
    }

    public void HasiBukatu()
    {
        UnableActivePanel();
        hasiBukatu.SetActive(true);
    }

    public void Eragiketak()
    {
        UnableActivePanel();
        eragigaia.SetActive(true);
    }

    public void Baldintzak()
    {
        UnableActivePanel();
        baldintza.SetActive(true);
    }

    public void Eragigaiak()
    {
        UnableActivePanel();
        eragigaia.SetActive(true);
    }

    public void Ifak()
    {
        UnableActivePanel();
        if_a.SetActive(true);
    }

    public void Elseak()
    {
        UnableActivePanel();
        else_a.SetActive(true);
    }

    public void Forak()
    {
        UnableActivePanel();
        for_a.SetActive(true);
    }

    public void Whileak()
    {
        UnableActivePanel();
        while_a.SetActive(true);
    }
}
