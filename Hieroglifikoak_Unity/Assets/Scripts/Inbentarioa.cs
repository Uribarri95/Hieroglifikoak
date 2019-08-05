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

    public delegate void ItemJaso();                                // UI eguneratzeko
    public ItemJaso itemJasoDeitu;

    List<Item> items = new List<Item>();                            // item zerrenda
    private int itemKopuruMax = 3;                                  // item kopuru maximoa
    string newItem;                                                 // item berria lortzen denean animazio txiki bat gertatzeko

    int bizitzaPuntuMax = 6;                                        // jokalariaren bizitza puntu maximoa, 3 aldiz handitu daiteke
    int bizitzaPuntuak;                                             // jokalariaren momentuko bizitza puntuak, bihotz erdietan kontatuta
    int bizitzaTopea = 12;                                          // jokalariaren gehiengo bizitza puntuak
    int bizitzaPuntuakAreagotu = 2;                                 // bizitza puntu hobekuntza, 6-tik 8-ra, 8-tik 10-era eta 10-etik 12-ra

    int geziKopuruMax = 10;                                         // jokalariaren gezi kopuru maximoa, 2 aldiz handitu daiteke
    int geziKopurua;                                                // momentuan jokalariak dituen gezi kopurua
    int geziakTopera = 20;                                          // jokalariaren gehiengo geziak
    int geziakAreagotu = 5;                                         // gezi kopuru hobekuntza, 10-etik 15-era eta 15-etik 20-ra

    int txanponKopurua = 0;                                         // jokalariaren txanpon kopurua, hobekuntzak erosteko.
    // !!! behekoa data barruan sartu
    int lortutakoTxaponGuztiak = 0;                                 // jokalariak lortu dituen txanpon denak.
    int jokalariaHilZenbaketa = 0;                                  // jokalaria zenbat aldiz hil den.

    int pasahitzak = 0;                                             // atea zabaltzeko pasahitza.

    public Sprite suIrudia, ezpataIrudia, arkuIrudia;               // item irudiak -> UI-an ikusteko

    private void Start()
    {
        UIEguneratu();
    }

    // jokalariaren datuak kargatzen dira. JokalariKudetzaileak kargatzen ditu
    public void Kargatu(Data.PlayerData datuak)
    {
        if (datuak.suArgia)
        {
            Item suArgia = ScriptableObject.CreateInstance<Item>();
            //Item suArgia = new Item();
            suArgia.izena = "SuArgia";
            suArgia.irudia = suIrudia;
            suArgia.erabileraBakarra = false;
            AddItem(suArgia);
        }
        if (datuak.ezpata)
        {
            Item ezpata = ScriptableObject.CreateInstance<Item>();
            //Item ezpata = new Item();
            ezpata.izena = "Ezpata";
            ezpata.irudia = ezpataIrudia;
            ezpata.erabileraBakarra = false;
            AddItem(ezpata);
        }
        if (datuak.arkua)
        {
            Item arkua = ScriptableObject.CreateInstance<Item>();
            //Item arkua = new Item();
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
        lortutakoTxaponGuztiak = datuak.lortutakoTxanponGuztiak;
        jokalariaHilZenbaketa = datuak.lortutakoTxanponGuztiak;
        pasahitzak = datuak.pasahitzak;
        UIEguneratu();
    }

    // kargatutako itemak zerrendara sartzen dira
    void AddItem(Item itema)
    {
        items.Add(itema);
        UIEguneratu();
    }


    // datuak gordetzen dira. JokalariKudetzailea arduratzen da
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
        // hil ostean jokua gordetzen da -> bizitza topera, ez 0
        if (bizitzaPuntuak <= 0)
        {
            datuak.bizitzaPuntuak = bizitzaPuntuMax;
        }
        else
        {
            datuak.bizitzaPuntuak = bizitzaPuntuak;
        }
        datuak.bizitzaPuntuMax = bizitzaPuntuMax;
        datuak.txanponKopurua = txanponKopurua;
        datuak.lortutakoTxanponGuztiak = lortutakoTxaponGuztiak;
        datuak.jokalariaHilZenbaketa = jokalariaHilZenbaketa;
        datuak.pasahitzak = pasahitzak;
        return datuak;
    }

    // item zerrenda itzultzen du, UI eguneratzeko
    public List<Item> GetItemZerrenda()
    {
        return items;
    }

    // bizitzaPuntuMaximoa itzultze du, UI eguneratzeko
    public int GetBizitzaPuntuMax()
    {
        return bizitzaPuntuMax;
    }

    // bizitzaPuntuak itzultzen du, UI eguneratzeko
    public int GetBizitzaPuntuak()
    {
        return bizitzaPuntuak;
    }

    // newItem balioa itzultzen du, item berria lortzean animazio bat ikusteko
    public string GetNewItem()
    {
        return newItem;
    }

    // balioa erreseteatzen da, beste item bat lortzean beste animazio bat ikusteko
    public void NewItemHustu()
    {
        newItem = null;
    }

    // geziKopurua itzultzen du, UI eguneratzeko
    public int GetGeziKop()
    {
        return geziKopurua;
    }

    // txanponKopurua itzultzen du, UI eguneratzeko
    public int GetTxanponKopurua()
    {
        return txanponKopurua;
    }

    // arkua erabili ondoren gezi bat galtzen da
    public void GeziaJaurti()
    {
        geziKopurua--;
    }

    // item berria lortu (sua, arkua, gezia, bihotzak, geziak, txanponak)
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
                case "Pasahitza":
                    PapiroaHartu();
                    return true;
                    //break;
                default:
                    break;
            }
        }
        return true;
    }

    // galdutako bihotzak berreskuratzen dira. Ezin da maximoa baino gehiago izan (maximoa hirutan hobetzen da)
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

    // geziak berreskuratzen dira. Ezin da maximoa baino gehiago izan (maximoa bitan hobetzen da)
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

    // bizitza kopuru maximoa hobetzen da (3 aldiz egin daiteke)
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

    // gezi kopuru maximoa hobetzen da (2 aldiz egin daiteke) 
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

    // txanpona hartu
    void TxanponaHartu()
    {
        AudioManager.instantzia.Play("Txanpona");

        lortutakoTxaponGuztiak++;
        txanponKopurua++;
        UIEguneratu();
    }

    // hobekuntzak erosi ondoren txanpon kopurua murrizten da
    // !!! 10 edo bukaerako prezioa parametro bihurtu
    public void TxanponakErabili()
    {
        if(txanponKopurua >= 10)
        {
            txanponKopurua -= 10;
        }
    }

    void PapiroaHartu()
    {
        pasahitzak++;
    }

    // zenbat papiro hartu diren itzultzen du
    public int GetPasahitzKop()
    {
        return pasahitzak;
    }

    // pasahitz minijokua berriz hasteko
    public void PasahitzakReset()
    {
        pasahitzak = 0;
    }

    // kolpea jasotzean bihotz errdia galtzen da (BizitzaPuntuak bihotz erdietan kontatzen da)
    public bool KolpeaJaso()
    {
        bizitzaPuntuak--;
        UIEguneratu();
        if (bizitzaPuntuak <= 0)
        {
            return true;
        }
        else
        {
            AudioManager.instantzia.Play("Kolpea");
        }
        return false;
    }

    // jokalaria hiltzen denean bihotz denak galtzen ditu
    public void JokalariaHil()
    {
        jokalariaHilZenbaketa++;
        bizitzaPuntuak = 0;
        UIEguneratu();
    }

    // jokalaria berpiztean bihotz denak berreskurtzen ditu
    public void Berpiztu()
    {
        bizitzaPuntuak = bizitzaPuntuMax;
        UIEguneratu();
    }

    // UI barruan erdian dagoen itema da item erabilgarria. 
    // Eskumako itema erabilgarria den itemarekin aldatzen da.
    public void SwipeRight()
    {
        Item lag = items[0];
        items[0] = items[2];
        items[2] = lag;
        UIEguneratu();
    }

    // UI barruan erdian dagoen itema da item erabilgarria. 
    // Ezkerreko itema erabilgarria den itemarekin aldatzen da.
    public void SwipeLeft()
    {
        Item lag = items[0];
        items[0] = items[1];
        items[1] = lag;
        UIEguneratu();
    }

    // UI-ak delegate-arekin lotuta, aldaketak egin behar direnean funtzioari deitzen zaio
    public void UIEguneratu()
    {
        if (itemJasoDeitu != null)
        {
            itemJasoDeitu.Invoke();
        }
    }
}
