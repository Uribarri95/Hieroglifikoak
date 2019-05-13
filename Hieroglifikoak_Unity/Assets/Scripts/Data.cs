using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Data : MonoBehaviour {

    string playerDataPath = "_playerData.dat";
    string itemsDataPath = "_itemsData.dat";

    int eszenatokia = 1;
    int checkpointZenbakia = 0;
    int txanponKopurua = 0;
    int lortutakoTxanponGuztiak = 0;
    int jokalariaHilZenbaketa = 0;
    int pasahitzak = 0;
    int geziKopurua = 10;
    int geziKopuruMax = 10;
    int bizitzaPuntuak = 6;
    int bizitzaPuntuMax = 6;

    [HideInInspector]

    #region Singleton
    public static Data instantzia;

    private void Awake()
    {
        if(instantzia != null)
        {
            Debug.LogWarning("Error Singleton: Badago beste PlayerData bat!");
            Debug.LogWarning("Objektu hau suntzituko da.");
            Destroy(gameObject);
            return;
        }
        instantzia = this;

        //DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton

    private void Start()        // !!! try catch jarri kargatu eta gorde koedan !!!
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            DatuakEzabatu();
        }
    }

    public void DatuakEzabatu()
    {
        try
        {
            File.Delete(Application.persistentDataPath + playerDataPath);
            File.Delete(Application.persistentDataPath + itemsDataPath);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void JokalariDatuakGorde(PlayerData datuak)
    {
        Ekintzak jokalariEkintzak = Ekintzak.instantzia;
        datuak.ekintzak = jokalariEkintzak.GetEkintzak();
        datuak.datuakGordeta = true;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fitxategia = File.Open(Application.persistentDataPath + playerDataPath, FileMode.OpenOrCreate);
        bf.Serialize(fitxategia, datuak);
        fitxategia.Close();
    }

    public PlayerData JokalariDatuakKargatu()
    {
        PlayerData datuak = new PlayerData();
        if (File.Exists(Application.persistentDataPath + playerDataPath))         // !!! errorea gertatzen bada partida berria !!! 
        {
            //print("gorde ftxategia bat dago  -> " + Application.persistentDataPath+playerDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + playerDataPath, FileMode.Open);
            datuak = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            datuak = JokalariDatuBerriak();
        }
        return datuak;
    }

    public void MapaGorde(Mapa mapaDatuak)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fitxategia = File.Open(Application.persistentDataPath + itemsDataPath, FileMode.OpenOrCreate);
        bf.Serialize(fitxategia, mapaDatuak);
        fitxategia.Close();
    }

    public Mapa MapaDatuakKargatu()
    {
        Mapa datuak = new Mapa();
        if (File.Exists(Application.persistentDataPath + playerDataPath))         // !!! errorea gertatzen bada partida berria !!! 
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + itemsDataPath, FileMode.Open);
            datuak = (Mapa)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            datuak = MapaBerria();
        }
        return datuak;
    }

    public PlayerData JokalariDatuBerriak()     // zenbakiak parametro bihurtu eta inbentariotik kendu -> hasieraketa kargatu() izatea !!! ekintzak hasieraketa ere !!!
    {
        PlayerData datuBerriak = new PlayerData();
        datuBerriak.datuakGordeta = false;
        datuBerriak.Eszenatokia = eszenatokia;
        datuBerriak.checkPointZenbakia = checkpointZenbakia;
        datuBerriak.txanponKopurua = txanponKopurua;
        datuBerriak.lortutakoTxanponGuztiak = lortutakoTxanponGuztiak;
        datuBerriak.jokalariaHilZenbaketa = jokalariaHilZenbaketa;
        datuBerriak.pasahitzak = pasahitzak;
        datuBerriak.suArgia = false;
        datuBerriak.ezpata = false;
        datuBerriak.arkua = false;
        datuBerriak.geziKopurua = geziKopurua;
        datuBerriak.geziKopuruMax = geziKopuruMax;
        datuBerriak.bizitzaPuntuak = bizitzaPuntuak;
        datuBerriak.bizitzaPuntuMax = bizitzaPuntuMax;
        return datuBerriak;
    }

    public Mapa MapaBerria()
    {
        Mapa mapaBerria = new Mapa();
        mapaBerria.itemak = null;
        mapaBerria.dialogak = null;
        return mapaBerria;
    }

    [Serializable]
    public class PlayerData
    {
        public bool datuakGordeta;
        public int Eszenatokia;
        public int checkPointZenbakia;
        public bool[] ekintzak;
        public int txanponKopurua;
        public int lortutakoTxanponGuztiak;
        public int jokalariaHilZenbaketa;
        public int pasahitzak;
        public bool suArgia;
        public bool ezpata;
        public bool arkua;
        public int geziKopurua, geziKopuruMax;
        public int bizitzaPuntuak, bizitzaPuntuMax;
    }

    [Serializable]
    public class Mapa
    {
        public bool[] itemak;
        public bool[] dialogak;
    }
}
