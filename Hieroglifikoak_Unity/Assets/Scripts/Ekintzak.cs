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

    Data data;

    // Use this for initialization
    void Start () {
        data = Data.instantzia;
        ekintzak = new bool[tamaina];
        Data.PlayerData jokalariDatuak = data.JokalariDatuakKargatu();

        if (jokalariDatuak.datuakGordeta)
        {
            ekintzak = jokalariDatuak.ekintzak;
        }
        else
        {
            for (int i = 0; i < ekintzak.Length; i++)
            {
                ekintzak[i] = false;
            }
        }

        argibideak = new string[tamaina];

        argibideak[0] = "Eskuma gezia zapaltzen bada, jokalaria eskumara mugitu behar da.\nMugitu eskumako pieza laukizuzen berdearen barnera.";
        argibideak[1] = "Ezkerra gezia zapaltzen bada, jokalaria ezkerrera mugitu behar da.\nPieza bat soberan egon daiteke.";
        argibideak[2] = "Zuriune-barra zapaltzean jokalaria salto egin behar du. Jokalaria ezin du airean dagoenean berriro salto egin." + 
                        " Salto egin ondoren eguneratu 'SaltoEginDezaket' aldagaia gezurra balioarekin";
        argibideak[3] = "Jokalariaren abiadura 6 izan behar da korrika dagoenean eta 4 oinez dagoenean. Eskumara zein ezkerrera joateko funtzionatu behar du." +
                        "\nAldatu pieza zaharrak aldagaiak dituzten piezengatik.";
        argibideak[4] = "Jokalariaren bizitza puntuak unitate baten areagotu behar dira. Jokalariak ezin du bizitza maximoa baino bihotz gehiago izan.";
        argibideak[5] = "Plataformak jokalaria helmugara garraiatu behar du, baina jokalaria plataforma gainean mugitzeko gai izan behar du." + 
                        "Horretarako, jokalariaren abiadurari plataformaren abiadura batu edo kendu.";
        argibideak[6] = "Plataforma gainean badago eta behera gezia zapaltzen badu, plataforma zeharkatzen du eta jauzi egiten da." +
                        "Jokalaria plataforma azpian badago eta salto egiten badu, plataforma zeharkatu behar du, zapaia kolpatu gabe.";
        argibideak[7] = "Aldatu kodea aldagaiak erabiltzeko. Salto botoia sakatzean eta salto egin badezake abiadura bertikala 6 izan behar du. Salto egin ondoren ezin du berriro salto egin";
        argibideak[8] = "Demagun jokalaria salto egin duela eta airean dagoela." + 
                        "\nJokalaria hormara itsasten bada, erortzen den abiadura erdian murrizten da. Gainera, berriro salto egin dezake naiz eta lurrean ez egon.";

        emaitzak = new string[tamaina][];

        /*emaitzak[0] = new string[] { "ifabif" };
        emaitzak[1] = new string[] { "ifabif", "ifcdififabif" };
        emaitzak[2] = new string[] { "aifbkdANDebkcbif", "aifbkdANDebkbcif", "aifbkeANDdbkcbif", "aifbkeANDdbkbcif" };
        emaitzak[3] = new string[] { "labifcdifelsemelseifejififgkif", "albifcdifelsemelseifejififgkif", "ablifcdifelsemelseifejififgkif", "abifcldifelsemelseifejififgkif" };
        emaitzak[4] = new string[] { "bifafif" };
        emaitzak[5] = new string[] { "zifbifebififhaififfgififdcifif" };
        emaitzak[6] = new string[] { "aifbkdANDbbkfhififbkeANDgbkchif", "aifbkdANDbbkfhififbkeANDgbkhcif", "aifbkdANDbbkfhififbkgANDebkhcif", "aifbkdANDbbkfhififbkgANDebkchif",
                                     "aifbkdANDbbkhfififbkgANDebkchif", "aifbkdANDbbkhfififbkgANDebkhcif", "aifbkdANDbbkhfififbkeANDgbkhcif", "aifbkdANDbbkhfififbkeANDgbkchif",
                                     "aifbkbANDdbkhfififbkeANDgbkchif", "aifbkbANDdbkhfififbkeANDgbkhcif", "aifbkbANDdbkhfififbkgANDebkhcif", "aifbkbANDdbkhfififbkgANDebkchif",
                                     "aifbkbANDdbkfhififbkgANDebkchif", "aifbkbANDdbkfhififbkgANDebkhcif", "aifbkbANDdbkfhififbkeANDgbkhcif", "aifbkbANDdbkfhififbkeANDgbkchif",
                                     "aifbkeANDgbkchififbkbANDdbkfhif", "aifbkeANDgbkchififbkbANDdbkhfif", "aifbkeANDgbkchififbkdANDbbkhfif", "aifbkeANDgbkchififbkdANDbbkfhif",
                                     "aifbkeANDgbkhcififbkdANDbbkfhif", "aifbkeANDgbkhcififbkdANDbbkhfif", "aifbkeANDgbkhcififbkbANDdbkhfif", "aifbkeANDgbkhcififbkbANDdbkfhif",
                                     "aifbkgANDebkhcififbkbANDdbkfhif", "aifbkgANDebkhcififbkbANDdbkhfif", "aifbkgANDebkhcififbkdANDbbkhfif", "aifbkgANDebkhcififbkdANDbbkfhif",
                                     "aifbkgANDebkchififbkdANDbbkfhif", "aifbkgANDebkchififbkdANDbbkhfif", "aifbkgANDebkchififbkbANDdbkhfif", "aifbkgANDebkchififbkbANDdbkfhif"};
        emaitzak[7] = new string[] { "cifbkfANDebkdgif", "cifbkfANDebkgdif", "cifbkeANDfbkgdif", "cifbkeANDfbkdgif" };
        emaitzak[8] = new string[] { "abcifdliifbkfANDgbkehifif", "abcifdliifbkfANDgbkheifif", "abcifdliifbkgANDfbkheifif", "abcifdliifbkgANDfbkehifif",
                                     "abcifdilifbkgANDfbkehifif", "abcifdilifbkgANDfbkheifif", "abcifdilifbkfANDgbkheifif", "abcifdilifbkfANDgbkehifif"};*/

        emaitzak[0] = new string[] { "ifabif" };
        emaitzak[1] = new string[] { "ifabif" };
        emaitzak[2] = new string[] { "ifabif" };
        emaitzak[3] = new string[] { "ifaifbcifif", "ifbifacifif" };
        emaitzak[4] = new string[] { "ifbkaandbcif", "ifbkbandacif" };
        emaitzak[4] = new string[] { "ifabififcdififbkeandfgif", "ifabififcdififbkfandegif" };
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
        ekintzak[index] = true;
    }

    public bool[] GetEkintzak()
    {
        return ekintzak;
    }

    public bool GetEkintza(int zenbakia)
    {
        return ekintzak[zenbakia];
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

    public bool GetSaltoHandia()
    {
        return ekintzak[7];
    }

    public bool GetHormaSaltoa()
    {
        return ekintzak[8];
    }

    public bool GetAteaZabaldu()
    {
        return ekintzak[9];
    }

    public bool GetEskilerakIgo()
    {
        return ekintzak[10];
    }

    public bool GetMakurtu()
    {
        return ekintzak[11];
    }

    public bool GetIrristatu()
    {
        return ekintzak[12];
    }

    public bool GetAldapaIrristatu()
    {
        return ekintzak[13];
    }
}
