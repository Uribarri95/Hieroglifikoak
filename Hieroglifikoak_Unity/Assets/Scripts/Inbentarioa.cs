using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inbentarioa : MonoBehaviour {

    #region Singleton
    public static Inbentarioa instantzia;

    private void Awake()
    {
        if(instantzia != null)
        {
            Debug.LogWarning("Error Singleton: Inbentario bat baino gehiago aurkitu da!");
            return;
        }
        instantzia = this;
    }
    #endregion

    public delegate void ItemJaso();
    public ItemJaso itemJasoDeitu;

    public List<Item> items = new List<Item>();
    private int itemKopuruMax = 3;

    int geziKopurua;
    public int gezikopuruMax = 10;

    // hide in inspector
    public Item edabea; // enum -> handia, txikia, ezer

    // hide in inspector
    public int bizitzaPuntuak = 6; // bihotz erdietan kontata
    public int bizitzaPuntuMax = 6;

    private void Start()
    {
        UIEguneratu();
    }

    public bool Add(Item item)
    {
        if (!item.erbileraBakarra)
        {
            if(items.Count >= itemKopuruMax)
            {
                Debug.Log("Ez dago " + item.izena + " itemarentzat tokirik.");
                return false;
            }
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i] == item)
                {
                    Debug.Log("Badaukazu " + item.izena + " inbentarioan.");
                    return false;
                }
            }
            items.Add(item);
            UIEguneratu();
        }
        else
        {
            Debug.Log("erabilera bakarreko itema");
            switch (item.izena)
            {
                case "Bihotza":
                    return BizitzaPuntuakGehitu();
                    //break;
                case "EdariHandia":
                    return EdariHandiaGorde(item);
                    //break;
                case "EdariTxikia":
                    return EdariTxikiaGorde(item);
                    //break;
                case "Geziak":
                    return GeziakGorde();
                    //break;
                default:
                    break;
            }
        }
        return true;
    }

    bool BizitzaPuntuakGehitu()
    {
        Debug.Log("bizitza puntuak berreskuratzen");
        if(bizitzaPuntuak == bizitzaPuntuMax)
        {
            Debug.Log("Bizitza topera");
            return false;
        }
        bizitzaPuntuak += 2;
        if (bizitzaPuntuak > bizitzaPuntuMax)
        {
            bizitzaPuntuak = bizitzaPuntuMax;
        }
        UIEguneratu();
        return true;
    }

    //edozein momentuan edan bizitza gehitzeko
    bool EdariHandiaGorde(Item item)
    {
        Debug.Log("Edabe handia gordetzen");
        Debug.Log(edabea.izena);
        if (edabea == null)
        {
            edabea = item;
        }
        else if(edabea.izena == "EdariTxikia")
        {
            edabea = item;
        }
        else
        {
            Debug.Log("Badaukazu edabe handia");
            return false;
        }
        UIEguneratu();
        return true;
    }

    bool EdariTxikiaGorde(Item item)
    {
        Debug.Log("Edabe txikia gordetzen");
        if (edabea == null)
        {
            edabea = item;
        }
        else
        {
            Debug.Log("Badaukazu edabe bat");
            return false;
        }
        UIEguneratu();
        return true;
    }

    bool GeziakGorde()
    {
        Debug.Log("Geziak gordetzen");
        if (geziKopurua == gezikopuruMax)
        {
            Debug.Log("Geziak topera");
            return false;
        }
        geziKopurua += 5;
        if (geziKopurua > gezikopuruMax)
        {
            geziKopurua = gezikopuruMax;
        }
        // UI barruan adierazi
        UIEguneratu();
        return true;
    }

    public void SwipeRight()
    {
        Item lag = items[0];
        items[0] = items[2];
        items[2] = lag;
        UIEguneratu();
    }

    public void SwipeLeft()
    {
        Item lag = items[0];
        items[0] = items[1];
        items[1] = lag;
        UIEguneratu();
    }

    public void UIEguneratu()
    {
        if (itemJasoDeitu != null)
        {
            itemJasoDeitu.Invoke();
        }
    }
}
