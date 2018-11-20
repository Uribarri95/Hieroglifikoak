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
    string newItem;

    int geziKopurua;
    public int gezikopuruMax = 10;

    // hide in inspector
    public Item edabea; // enum -> handia, txikia, ezer

    // hide in inspector
    public int bizitzaPuntuak = 6; // bihotz erdietan kontata
    public int bizitzaPuntuMax = 6;

    private void Start()
    {
        geziKopurua = 10;
        UIEguneratu();
    }

    /*public bool ItemaDaukat(string izena)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].izena == izena)
            {
                return true;
            }
        }
        return false;
    }*/

    public string GetNewItem()
    {
        return newItem;
    }

    public void SetNewItem()
    {
        newItem = null;
    }

    public int GetGeziKop()
    {
        return geziKopurua;
    }

    public void GeziaJaurti()
    {
        geziKopurua--;
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
            newItem = item.izena;
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
                case "Bihotz+":
                    return BizitzaPuntuakHanditu();
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

    bool BizitzaPuntuakHanditu()
    {
        if(bizitzaPuntuMax < 12)
        {
            bizitzaPuntuMax += 2;
        }
        else
        {
            Debug.Log("Gehienez 6 bihotz. Bizitza puntu maximoa duzu");
        }
        bizitzaPuntuak = bizitzaPuntuMax;
        UIEguneratu();
        return true;
    }

    public bool KolpeaJaso()
    {
        bizitzaPuntuak--;
        UIEguneratu();
        if (bizitzaPuntuak <= 0)
        {
            return true;
        }
        return false;
    }

    public void JokalariaHil()
    {
        bizitzaPuntuak = 0;
        UIEguneratu();
    }

    public void Berpiztu()
    {
        bizitzaPuntuak = bizitzaPuntuMax;
        //bizitzaPuntuak = 6; // !!!
        UIEguneratu();
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
