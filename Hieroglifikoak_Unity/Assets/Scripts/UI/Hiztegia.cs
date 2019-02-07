using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiztegia : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject menuGurasoa;

    public GameObject datuakInfo;
    public GameObject eragiketakInfo;
    public GameObject funtzioakInfo;
    public GameObject aldagaiakInfo;
    public GameObject baldintzakInfo;
    public GameObject eragigaiakInfo;
    public GameObject ifInfo;
    public GameObject elseInfo;
    public GameObject forInfo;
    public GameObject whileInfo;

    public enum MenuStates { DatuOrokorrak, Eragiketak, Funtzioak, Aldagaiak, Baldintzak, Eragigaiak, If, Else, For, While };
    public MenuStates currentMenu;

    bool aldaketa = false;

    private void Awake()
    {
        currentMenu = MenuStates.DatuOrokorrak;
    }

    private void Update()
    {
        if (aldaketa)
        {
            aldaketa = false;
            switch (currentMenu)
            {
                case MenuStates.DatuOrokorrak:
                    datuakInfo.SetActive(true);
                    UnableActivePanel();
                    break;
                case MenuStates.Eragiketak:
                    eragigaiakInfo.SetActive(true);
                    UnableActivePanel();
                    break;
                case MenuStates.Funtzioak:
                    funtzioakInfo.SetActive(true);
                    UnableActivePanel();
                    break;
                case MenuStates.Aldagaiak:
                    aldagaiakInfo.SetActive(true);
                    UnableActivePanel();
                    break;
                case MenuStates.Baldintzak:
                    baldintzakInfo.SetActive(true);
                    UnableActivePanel();
                    break;
                case MenuStates.Eragigaiak:
                    eragigaiakInfo.SetActive(true);
                    UnableActivePanel();
                    break;
                case MenuStates.If:
                    ifInfo.SetActive(true);
                    UnableActivePanel();
                    break;
                case MenuStates.Else:
                    elseInfo.SetActive(true);
                    break;
                case MenuStates.For:
                    forInfo.SetActive(true);
                    UnableActivePanel();
                    break;
                case MenuStates.While:
                    whileInfo.SetActive(true);
                    UnableActivePanel();
                    break;
                default:
                    break;
            }
        }
    }

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
        currentMenu = MenuStates.DatuOrokorrak;
        aldaketa = true;
        /*
        UnableActivePanel();
        datuakBotoia.SetActive(true);
        */
    }

    public void Eragiketak()
    {
        currentMenu = MenuStates.Eragiketak;
        aldaketa = true;
        /*
        UnableActivePanel();
        eragigaiakInfo.SetActive(true);
        */
    }

    public void Funtzioak()
    {
        currentMenu = MenuStates.Funtzioak;
        aldaketa = true;
    }

    public void Aldagaiak()
    {
        currentMenu = MenuStates.Aldagaiak;
        aldaketa = true;
    }

    public void Baldintzak()
    {
        currentMenu = MenuStates.Baldintzak;
        aldaketa = true;
        /*
        UnableActivePanel();
        baldintzakInfo.SetActive(true);
        */
    }

    public void Eragigaiak()
    {
        currentMenu = MenuStates.Eragigaiak;
        aldaketa = true;
        /*
        UnableActivePanel();
        eragigaiakInfo.SetActive(true);
        */
    }

    public void Ifak()
    {
        currentMenu = MenuStates.If;
        aldaketa = true;
        /*
        UnableActivePanel();
        ifInfo.SetActive(true);
        */
    }

    public void Elseak()
    {
        currentMenu = MenuStates.Else;
        aldaketa = true;
        /*
        UnableActivePanel();
        elseInfo.SetActive(true);
        */
    }

    public void Forak()
    {
        currentMenu = MenuStates.For;
        aldaketa = true;
        /*
        UnableActivePanel();
        forInfo.SetActive(true);
        */
    }

    public void Whileak()
    {
        currentMenu = MenuStates.While;
        aldaketa = true;
        /*
        UnableActivePanel();
        whileInfo.SetActive(true);
        */
    }
}
