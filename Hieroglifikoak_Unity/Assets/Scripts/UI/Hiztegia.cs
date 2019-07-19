using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiztegia : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject menuGurasoa;

    public GameObject datuakInfo;
    public GameObject eragiketakInfo;
    public GameObject aldagaiakInfo;
    public GameObject baldintzakInfo;
    public GameObject eragigaiakInfo;
    public GameObject ifInfo;
    public GameObject elseInfo;
    public GameObject forInfo;
    public GameObject whileInfo;
    public GameObject listInfo;

    public void AtzeraBotoia()
    {
        gameObject.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void UnableActivePanel()
    {
        for (int i = 0; i < menuGurasoa.transform.childCount; i++)
        {
            if (menuGurasoa.transform.GetChild(i).gameObject.activeSelf)
            {
                menuGurasoa.transform.GetChild(i).gameObject.SetActive(false);
                return;
            }
        }
    }

    public void DatuakIkusi()
    {
        //currentMenu = MenuStates.DatuOrokorrak;
        UnableActivePanel();
        datuakInfo.SetActive(true);
    }

    public void Eragiketak()
    {
        //currentMenu = MenuStates.Eragiketak;
        UnableActivePanel();
        eragiketakInfo.SetActive(true);
    }

    public void Aldagaiak()
    {
        //currentMenu = MenuStates.Aldagaiak;
        UnableActivePanel();
        aldagaiakInfo.SetActive(true);
    }

    public void Baldintzak()
    {
        //currentMenu = MenuStates.Baldintzak;
        UnableActivePanel();
        baldintzakInfo.SetActive(true);
    }

    public void Eragigaiak()
    {
        //currentMenu = MenuStates.Eragigaiak;
        UnableActivePanel();
        eragigaiakInfo.SetActive(true);
    }

    public void Ifak()
    {
        //currentMenu = MenuStates.If;
        UnableActivePanel();
        ifInfo.SetActive(true);
    }

    public void Elseak()
    {
        //currentMenu = MenuStates.Else;
        UnableActivePanel();
        elseInfo.SetActive(true);
    }

    public void Forak()
    {
        //currentMenu = MenuStates.For;
        UnableActivePanel();
        forInfo.SetActive(true);
    }

    public void Whileak()
    {
        //currentMenu = MenuStates.While;
        UnableActivePanel();
        whileInfo.SetActive(true);
    }

    public void Listak()
    {
        //currentMenu = MenuStates.List;
        UnableActivePanel();
        listInfo.SetActive(true);
    }
}