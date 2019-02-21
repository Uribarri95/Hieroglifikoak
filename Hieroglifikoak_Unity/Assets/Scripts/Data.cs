using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Data : MonoBehaviour {

    string dataPath = "playerData.dat";

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

        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton

    private void Start()
    {
        //Kargatu();      // beste klase batek hasieratu dezala, ez denez suntzitzen atzera aurrera joatean ez dira datuak kargatzen
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            try
            {
                File.Delete(Application.persistentDataPath + dataPath);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    public void Gorde(PlayerData datuak)
    {
        print("gordetzen");
        datuak.datuakGordeta = true;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fitxategia = File.Open(Application.persistentDataPath + dataPath, FileMode.OpenOrCreate);
        bf.Serialize(fitxategia, datuak);
        fitxategia.Close();
    }

    public PlayerData Kargatu()
    {
        print("kargatzen");
        PlayerData datuak = new PlayerData();
        if (File.Exists(Application.persistentDataPath + dataPath))         // !!! errorea gertatzen bada partida berria !!! 
        {
            //print("gorde ftxategia bat dago  -> " + Application.persistentDataPath+dataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + dataPath, FileMode.Open);
            //PlayerData datuak = (PlayerData)bf.Deserialize(file);
            datuak = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            datuak = DatuBerriak();
        }
        return datuak;
    }

    public PlayerData DatuBerriak()     // zenbakiak parametro bihurtu eta inbentariotik kendu -> hasieraketa kargatu() izatea !!! ekintzak hasieraketa ere !!!
    {
        PlayerData datuBerriak = new PlayerData();
        datuBerriak.datuakGordeta = false;
        datuBerriak.Eszenatokia = 1;
        datuBerriak.checkPointZenbakia = 0;
        datuBerriak.txanponKopurua = 0;
        datuBerriak.itemak = new List<Item>();
        datuBerriak.geziKopurua = 10;
        datuBerriak.geziKopuruMax = 10;
        datuBerriak.bizitzaPuntuak = 6;
        datuBerriak.bizitzaPuntuMax = 6;
        return datuBerriak;
    }

    [Serializable]                      // fitxategi berri bat -> mapa -> hartutako txanpon eta itemak -> 
    public class PlayerData             // fitxategi berri bat -> partidadatuak -> datuak daude eta eszenatokia, mainmenu behar duen bakarra
    {
        public bool datuakGordeta;      // beste fitxategi baten partidadatuak
        public int Eszenatokia;         // beste fitxategi baten partidadatuak
        public int checkPointZenbakia;
        public bool[] ekintzak;
        public int txanponKopurua;
        public List<Item> itemak;
        public int geziKopurua, geziKopuruMax;
        public int bizitzaPuntuak, bizitzaPuntuMax;
    }
}
