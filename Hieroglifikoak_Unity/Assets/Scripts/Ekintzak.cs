using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ekintzak : MonoBehaviour {

    #region Singleton
    public static Ekintzak instantzia;

    private void Awake()
    {
        if (instantzia != null)
        {
            Debug.LogWarning("Error Singleton: Ekintza multzo bat baino gehiago aurkitu da!");
            return;
        }
        instantzia = this;
    }
    #endregion

    // eskuma = 0, ezkerra = 1, saltoTxikia = 2, korrika = 3, makurtu = 4, 
    [HideInInspector]
    public string[][] emaitzak;

    public int tamaina;
    int index;
    public bool[] ekintzak; //bukatzean private egin !!!
    string[] argibideak;

    // Use this for initialization
    void Start () {
        ekintzak = new bool[tamaina];
        for (int i = 0; i < ekintzak.Length; i++)
        {
            ekintzak[i] = false;
        }

        argibideak = new string[tamaina];
        argibideak[0] = "Eskuma gezia zapaltzean jokalaria eskumara mugitu behar da.\nMugitu eskumako pieza laukizuzen berdearen barnera.";
        argibideak[1] = "Ezkerra gezia zapaltzean jokalaria ezkerrera mugitu behar da.";
        argibideak[2] = "Zuriune-barra zapaltzean jokalaria salto egin behar du.\nLurrera erori arte ezin du berriz jauzi egin.";

        emaitzak = new string[tamaina][];

        emaitzak[0] = new string[] { "hasiifbEskBotpxPos+4ifbukatu" };
        emaitzak[1] = new string[] { "hasiifbEskBotpxPos+4ififbEzkBotpxPos-4ifbukatu", "hasiifbEzkBotpxPos-4ififbEskBotpxPos+4ifbukatu" };
        emaitzak[2] = new string[] { "" };
    }

    public bool EmaitzaKonprobatu(string erabEmaitza)
    {
        for (int i = 0; i < emaitzak[index].Length; i++)
        {
            if (emaitzak[index][i] == erabEmaitza)
            {
                Debug.Log("zuzena");
                return true;
            }
        }
        Debug.Log("okerra");
        return false;
    }

    public string GetArgibidea()
    {
        return argibideak[index];
    }

    public void SetIndex(int indizea) // puzleManager erabiltzen du emaitza konprobatzeko
    {
        index = indizea;
    }

    public int GetIndex() // ?
    {
        return index;
    }

    public void Eragin()
    {
        print("eragin " + index);
        ekintzak[index] = true;
    }

    public bool GetEskuma()
    {
        return ekintzak[0];
    }

    public bool GetEzkerra()
    {
        return ekintzak[1];
    }

    public bool GetSaltoTxikia()
    {
        return ekintzak[2];
    }

    public bool GetKorrika()
    {
        return ekintzak[3];
    }

    public bool GetMakurtu()
    {
        return ekintzak[4];
    }
}
