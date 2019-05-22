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
        argibideak[1] = "Ezkerra gezia zapaltzen bada, jokalaria ezkerrera mugitu behar da.";
        argibideak[2] = "Zuriune-barra zapaltzen bada, jokalariak salto egin behar du.";
        argibideak[3] = "Zuriune-barra zapaltzen bada eta jokalaria lurrean badago, jokalariak salto egin behar du. \nErabili SaltoEginDaiteke() funtzioa jokalaria lurrean dagoen jakiteko.";
        argibideak[4] = "Zuriune-barra zapaltzen bada eta jokalaria lurrean badago, jokalariak salto egin behar du.";
        argibideak[5] = "Eskuma eta ezkerra geziak aldi berean zapaltzen badira jokalaria geldi mantendu behar da.";
        argibideak[6] = "Jokalariak bihotz bat berreskuratu behar du.";
        argibideak[7] = "Jokalariak bihotz erdia galdu behar du.";
        argibideak[8] = "Jokalariak bihotz bat berreskuratu behar du. Ziurtatu bihotzak bizitza maximoa baino handiagoa ez izatea.";
        argibideak[9] = "Jokalariak bihotz erdia galdu behar du. Bihotzak 0 edo txikiago badira jokalaria hil egiten da.";
        argibideak[10] = "Bihotz bat hartu aurretik ziurtatu bihotzak topera ez egotea.";
        argibideak[11] = "Shift tekla zapaltzen bada, jokalariaren abiadura 6 izango da eta bestela 4.";
        argibideak[12] = "Jokalaria lurrean dagoenean saltoEgin aldagaia egia izan behar da, bestela saltoEgin gezurra izan behar da.";
        argibideak[13] = "KorrikaEgin() exekutatzeko eskuma edo ezkerra geziak sakatuta egon behar dute, bestela jokalaria geldi mantendu behar da.";
        argibideak[14] = "Norabide geziak sakatu ostean, aurrean hormarik badago jokalaria gelditu egin behar da, bestela ikututako noranzkoan mugitu behar da.";
        argibideak[15] = "Funtzio honek bool motako informazio erantzun behar du. Jokalaria plataformaren gainean badago egia izan behar du eta bestela gezurra.";
        argibideak[16] = "Jokalaria plataformarekin batera mugitu behar da naiz eta geziak ez ikutu. Plataforma geldi badago jokalaria ere geldi mantenduko da.";
        argibideak[17] = "Aldatu baldintza plataforma hegalariez gain plataforma zeharkagarrien gainean gaudenean ere 'gainean' aldagaia egia izateko.";
        argibideak[18] = "Jokalaria behera jauzi behar da plataforma gainean badago eta sabaia zeharkatu salto egiten badu. " + 
                         "Erabili ! eragigaia PlataformaGaineanDago() funtzioarekin jokalaria plataforma azpian dagoen jakiteko";
        argibideak[19] = "Jokalaria geldi mantendu behar da botoirik sakatzen ez denean.";
        argibideak[20] = "Salto botoia sakatzen denean, botoia sakatuta mantentzen bada indarra 8 izan behar da eta altuera maximoa lortu baino lehen askatzen bada 4 izan behar da.";
        argibideak[21] = "Funtzioak saltoa aldagaia erabiliko du salto egin dezakken jakiteko. Gero, hormaren kontrako zentzuan salto egingo du.\nSaltoEgin() funtzio pieza bakarra dago, " + 
                         "jarri toki egokian bi noranzkoetan funtzionatzeko.";
        argibideak[22] = "HormaSaltoa() funtzioak bool motako datua behar du funtzionatzeko. Datu hori SaltoEginDaiteke() funtzioak eskeiniko du. Jarri funtzioak itzultzen duen datua HormaSaltoa() duen hutsunean.";
        argibideak[23] = "Jokalariak salto egin ahal izango du baldin eta lurrean badago edo hormari itsatsita badago. Funtzioak egia itzuli behar du salto egin badezake eta gezurra kontrako kasuan.";
        argibideak[24] = "Jokalaria lurrean badago salto normala egingo du. Bestela, airean badago eta pareta baten aurka itsatsita badago horma saltoa egingo du.";
        argibideak[25] = "Jokalariak bihotzak berreskuratu behar ditu. Kopurua bihotzKopurua aldagaiak adieraziko du. Ziurtatu bihotzak bizitza maximoa baino hadiagoa ez izatea.";
        argibideak[26] = "Jokalariak atea zeharkatu dezake behera gezia zapaltzen baldin eta atearen aurrean badago.";
        argibideak[27] = "Jokalaria eskilera gainean badago behera mugitu daiteke behera geziarekin. Eskilera azpian badago gora mugitu daiteke gora geziarekin. " +
                         "Eskilera erdian badago gora edo behera mugitu daiteke norabide gezi egokiak erabilita. Goian ez badago eta behean ere ez badago jokalaria erdian dago.";
        argibideak[28] = "Jokalariak noiz makurtu daitekeen erabaki behar da. Hasieran makurtu aldagaia egia izango da, eta airean, plataforma zeharkagarri baten gainean, eskileran edo ate baten aurrean badago " + 
                         "makurtu aldagaia gezurra izango da.";
        argibideak[29] = "Behera gezia zapaltzen denean eta makurtu badaiteke, jokalariaren tamaina 2 aldiz txikiagoa izan behar da eta abiadura 2 izan behar da.";
        argibideak[30] = "Erabili EskumaraMugitu() eta EzkerreraMugitu() pieza berriak abiadura ezberdinekin funtzionatzeko. Deitu KorrikaEgin() eta makurtu() funtzioei abiadura aldatzeko.";
        argibideak[31] = "Joalaria makurtuta dagoen bitartean abiadura 2 izan behar du. Makurtzeko botoia askatzean (edo zapalduta ez dagoenean) eta gainean oztoporik ez dagoenean makurtu gezurra izango da " +
                         "eta abiadura 4 izan behar du berriro";
        argibideak[32] = "Jokalaria korrika badago eta makurtu egiten bada makurtuta mugitu beharrean irristatu egingo da.";
        argibideak[33] = "Erabili begizta jokalariaren abiadura pixkanaka murrizteko. Jarri IF pieza tamaina aldaketa behin bakarrik gertatzeko. Irristatzen bukatu ondoren jokalaria makurtu egin behar da.";
        argibideak[34] = "Jokalaria aldapan badago eta behera gezia zapaltzen badu, aldapa behera irristatuko du oso abiadura handian. Aldapa bukatzean abiadura azkarra galduko du.";
        argibideak[35] = "Lortutako pasahitza atePasahitzaren berdina izan behar du atea zabaltzeko. Erabili 'Equals' baldintza pieza pasahitzak berdinak diren zihurtatzeko.";
        argibideak[36] = "Batu lortutako bi hitzak '+' eragigaia erabilita atea zabaltzeko.";
        argibideak[37] = "";
        argibideak[38] = "";
        argibideak[39] = "";
        argibideak[40] = "";
        argibideak[41] = "ArgiDistantzia 5 da. Ctrl tekla zapaldu ondoren argiDistantzia 30 bihurtzen da eta 3 segundu pasatu ondoren argiDistantzia pixkanaka murrizten da berriro 5 izan arte.";
        argibideak[42] = "Kontrol tekla zapaltzean ingurunea argitu behar da eta etsaia gertu badago zerrendan gordeko da. Erabili pieza moreak zerrendekin lan egiteko. Erabili 'Add' eragigaia kolpatutako etsaiak zerrendan sartzeko.";
        argibideak[43] = "Erabaki zelan zeharkatuko dira zerrendako elementuak etsai guztiei bizitza kentzeko. 1.- Zenbatgarren elementuarekin hasiko da. 2.- Zein baldintzarekin geldituko da. 3.- Zenbatero ikusiko dira etsaiak eta gorako edo beherako ordenean";
        argibideak[44] = "Etsaiari minPuntuak kendu behar zaizkio bere bizitza puntuetatik. Bizitza dena galtzen badu etsai hiltzen da.";
        argibideak[45] = "";
        argibideak[46] = "";
        argibideak[47] = "";
        argibideak[48] = "";
        argibideak[49] = "";
        argibideak[50] = "";
        argibideak[51] = "";
        argibideak[52] = "";

        emaitzak = new string[tamaina][];

        emaitzak[0] = new string[] { "ifabif" };
        emaitzak[1] = new string[] { "ifabif" };
        emaitzak[2] = new string[] { "ifabif" };
        emaitzak[3] = new string[] { "ifaifacifif" };
        emaitzak[4] = new string[] { "ifbkaandabif" };
        emaitzak[5] = new string[] { "ifabififcdififbkeandefif" };
        emaitzak[6] = new string[] { "a" };
        emaitzak[7] = new string[] { "a" };
        emaitzak[8] = new string[] { "aifb>cdif" };
        emaitzak[9] = new string[] { "aifb<=cdif" };
        emaitzak[10] = new string[] { "ifb>acif", "ifa<bcif" };
        emaitzak[11] = new string[] { "aifbcifelsedelsee" };
        emaitzak[12] = new string[] { "aifbcifelsedelsee" };
        emaitzak[13] = new string[] { "ifbkaorbcifelsedelse", "ifbkboracifelsedelse" };
        emaitzak[14] = new string[] { "ifaifbcifelsedelseififeiffcifelsegelseif", "ifeiffcifelsegelseififaifbcifelsedelseif" };
        emaitzak[15] = new string[] { "abifcdifelseefelsegb" };
        emaitzak[16] = new string[] { "ifbkaandbifcgififdjififehififfiififelsekelse" };
        emaitzak[17] = new string[] { "aifbkcandbdifelseeelsef", "aifbkbandcdifelseeelsef" };
        emaitzak[18] = new string[] { "ifaifcdifififbifbk!ceifif" };
        emaitzak[19] = new string[] { "ifbk!aand!bcif", "ifbk!band!acif" };
        emaitzak[20] = new string[] { "aifbkbandcifefifelseeelsegif" };
        emaitzak[21] = new string[] { "bifbkaandbifdgififceiffif", "bifbkaandbifdgiffifceifif", "bifbkaandbfifdgififceifif" };
        emaitzak[22] = new string[] { "ifehdif", "abcdifehbif" };
        emaitzak[23] = new string[] { "aifbkborceifelsefelsegd", "aifbkcorbeifelsefelsegd" };
        emaitzak[24] = new string[] { "ifaifbcifelseifbkcordefifelseif", "ifaifbcifelseifbkdorcefifelseif" };
        emaitzak[25] = new string[] { "ifbka<bcdifelseefelse", "ifbka>befifelsecdelse" };
        emaitzak[26] = new string[] { "ifbkaandbcif", "ifbkbandacif" };
        emaitzak[27] = new string[] { "ifcifdifbfifififeifagifififbk!dand!eifafififbgififif", "ifcifdifbfifififeifagifififbk!eand!difafififbgififif" };
        emaitzak[28] = new string[] { "caif!dbififebififfbififgbifh" };
        emaitzak[29] = new string[] { "abifbkeandfgbghchzifi", "abifbkfandegbghchzifi" };
        emaitzak[30] = new string[] { "abifcdififefififghififijif" };
        emaitzak[31] = new string[] { "awhilebfifbk!cand!ceeeifwhilei", "awhilebifbk!cand!ceeeiffwhilei" };
        emaitzak[32] = new string[] { "ifbcifadifif" };
        emaitzak[33] = new string[] { "whilebka>bdif!fccifwhileg", "whilebka>bif!fccifdwhileg" };
        emaitzak[34] = new string[] { "aifbkbandheifwhilecdififgifwhile", "aifbkbandheifwhilecdifigfifwhile", "aifbkbandheifwhilecifigfifdwhile", "aifbkbandheifwhilecififgifdwhile" };
        emaitzak[35] = new string[] { "ifeqaeeqbif", "cpkdepkifeqadeqbif" };
        emaitzak[36] = new string[] { "aaapkbfepkifeqgeqdif" };
        emaitzak[37] = new string[] { "" };
        emaitzak[38] = new string[] { "" };
        emaitzak[39] = new string[] { "" };
        emaitzak[40] = new string[] { "" };
        emaitzak[41] = new string[] { "abcaifdefgehihifwhilebkf>cjwhile" };
        emaitzak[42] = new string[] { "abaifchifdegefifif", "abaifcifdegefifhif" };
        emaitzak[43] = new string[] { "aforbdfhbcjfor", "aforbegibcjfor" };
        emaitzak[44] = new string[] { "aifbk<=bkdif" };
        emaitzak[45] = new string[] { "" };
        emaitzak[46] = new string[] { "" };
        emaitzak[47] = new string[] { "" };
        emaitzak[48] = new string[] { "" };
        emaitzak[49] = new string[] { "" };
        emaitzak[50] = new string[] { "" };
        emaitzak[51] = new string[] { "" };
        emaitzak[52] = new string[] { "" };
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
        return ekintzak[4];
    }

    public bool GetKorrika()
    {
        return ekintzak[14];
    }

    public bool GetZoruaZeharkatu()
    {
        return ekintzak[19];
    }

    public bool GetSaltoHandia()
    {
        return ekintzak[20];
    }

    public bool GetHormaSaltoa()
    {
        return ekintzak[25];
    }

    public bool GetAteaZabaldu()
    {
        return ekintzak[26];
    }

    public bool GetEskilerakIgo()
    {
        return ekintzak[27];
    }

    public bool GetMakurtu()
    {
        return ekintzak[31];
    }

    public bool GetIrristatu()
    {
        return ekintzak[33];
    }

    public bool GetAldapaIrristatu()
    {
        return ekintzak[34];
    }
}
