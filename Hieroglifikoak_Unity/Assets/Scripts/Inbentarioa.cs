using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
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

    [SerializeField]
    public List<Item> items = new List<Item>();
    private int itemKopuruMax = 3;
    string newItem;

    int geziKopurua;
    public int geziKopuruMax = 10;
    int geziakTopera = 20;
    int geziakAreagotu = 5;

    public static int bizitzaPuntuak; // bihotz erdietan kontata
    public int bizitzaPuntuMax = 6;
    int bizitzaTopea = 12;
    int bizitzaPuntuakAreagotu = 2;

    public int txanponKopurua = 0;

    public Sprite suIrudia, ezpataIrudia, arkuIrudia;

    private void Start() // -> kargatu lehenengo aldian !!!
    {
        geziKopurua = geziKopuruMax;
        bizitzaPuntuak = bizitzaPuntuMax;
        UIEguneratu();
    }

    public void Kargatu(Data.PlayerData datuak)
    {
        if (datuak.suArgia)
        {
            Item suArgia = new Item();
            suArgia.izena = "SuArgia";
            suArgia.irudia = suIrudia;
            suArgia.erabileraBakarra = false;
            AddItem(suArgia);
        }
        if (datuak.ezpata)
        {
            Item ezpata = new Item();
            ezpata.izena = "Ezpata";
            ezpata.irudia = ezpataIrudia;
            ezpata.erabileraBakarra = false;
            AddItem(ezpata);
        }
        if (datuak.arkua)
        {
            Item arkua = new Item();
            arkua.izena = "Arkua";
            arkua.irudia = arkuIrudia;
            arkua.erabileraBakarra = false;
            AddItem(arkua);
        }
        geziKopurua = datuak.geziKopurua;
        geziKopuruMax = datuak.geziKopuruMax;
        bizitzaPuntuak = datuak.bizitzaPuntuak;
        bizitzaPuntuMax = datuak.bizitzaPuntuMax;
        txanponKopurua = datuak.txanponKopurua;
    }

    public void AddItem(Item itema)
    {
        items.Add(itema);
        UIEguneratu();
    }

    public Data.PlayerData Gorde()
    {
        Data.PlayerData datuak = new Data.PlayerData();
        datuak.suArgia = false;
        datuak.ezpata = false;
        datuak.arkua = false;
        foreach (var item in items)
        {
            if(item.izena == "SuArgia")
            {
                datuak.suArgia = true;
            }
            if(item.izena == "Ezpata")
            {
                datuak.ezpata = true;
            }
            if (item.izena == "Arkua")
            {
                datuak.arkua = true;
            }
        }
        datuak.geziKopurua = geziKopurua;
        datuak.geziKopuruMax = geziKopuruMax;
        datuak.bizitzaPuntuak = bizitzaPuntuak;
        datuak.bizitzaPuntuMax = bizitzaPuntuMax;
        datuak.txanponKopurua = txanponKopurua;
        return datuak;
    }

    public string GetNewItem()
    {
        return newItem;
    }

    public void NewItemHustu()
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
        if (!item.erabileraBakarra)
        {
            if(items.Count >= itemKopuruMax)
            {
                Debug.Log("Ez dago " + item.izena + " itemarentzat tokirik.");
                return false;
            }
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].izena == item.izena)
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
                case "Geziak":
                    return GeziakGorde();
                    //break;
                case "Bihotz+":
                    // max 3 hartu daitezke
                    return BizitzaPuntuakHanditu();
                    //break;
                case "Gezi+":
                    return GeziKopuruaHanditu();
                    //break;
                case "Txanpona":
                    TxanponaHartu();
                    return true;
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

    bool GeziakGorde()
    {
        Debug.Log("Geziak gordetzen");
        if (geziKopurua == geziKopuruMax)
        {
            Debug.Log("Geziak topera");
            return false;
        }
        geziKopurua += 5;
        if (geziKopurua > geziKopuruMax)
        {
            geziKopurua = geziKopuruMax;
        }
        UIEguneratu();
        return true;
    }

    bool BizitzaPuntuakHanditu()
    {
        if(bizitzaPuntuMax < bizitzaTopea)
        {
            bizitzaPuntuMax += bizitzaPuntuakAreagotu;
        }
        else
        {
            Debug.Log("Gehienez 6 bihotz. Bizitza puntu maximoa duzu");
        }
        bizitzaPuntuak = bizitzaPuntuMax;
        UIEguneratu();
        return true;
    }

    bool GeziKopuruaHanditu()
    {
        if(geziKopuruMax < geziakTopera)
        {
            geziKopuruMax += geziakAreagotu;
        }
        else
        {
            Debug.Log("Gehienez 20 gezi. Gezi kopurua topera");
        }
        geziKopurua = geziKopuruMax;
        UIEguneratu();
        return true;
    }

    void TxanponaHartu()
    {
        txanponKopurua++;
        UIEguneratu();
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
