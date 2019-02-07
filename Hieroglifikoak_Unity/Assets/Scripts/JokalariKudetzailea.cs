using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class JokalariKudetzailea : MonoBehaviour {

    #region Singleton
    public static JokalariKudetzailea instantzia;

    private void Awake()
    {
        if (instantzia != null)
        {
            Debug.LogWarning("Error Singleton: JokalariKudeatzaile bat baino gehiago aurkitu da!");
            return;
        }
        instantzia = this;
    }
    #endregion

    public GameObject checkpoint;
    public GameObject cam;
    public FadeManager fadeManager;
    
    private JokalariMug jokalaria;
    Inbentarioa inbentarioa;

    public float hilAnimazioa = 1.2f;
    public float berpiztuAnimazioa = 1.2f;

    public bool aktibatu;
    public int checkpointZenbakia;

    string dataPath = "playerData.dat";

    // Use this for initialization
    void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
        inbentarioa = Inbentarioa.instantzia;
        // kargatu() !!!
        fadeManager.Argitu(2f); // !!!dialog triggerraren aktibazio denbora luzatu baita ere!
    }

    private void Update()
    {
        if (aktibatu)
        {
            aktibatu = false;
            CheckpointeraMugitu(checkpointZenbakia);
        }
    }

    public void CheckpointZenbakiaLortu()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i) == checkpoint.transform)
            {
                checkpointZenbakia = i;
            }
        }
    }

    public void CheckpointeraMugitu(int zenbakia)
    {
        checkpointZenbakia = zenbakia;
        checkpoint = transform.GetChild(zenbakia).gameObject;
        StartCoroutine(CheckPointeraMugitu());
    }

    public void JokalariaHil()
    {
        if (!jokalaria.hiltzen)
        {
            inbentarioa.JokalariaHil();
            StartCoroutine(JokalariaBerpiztu());
        }
    }

    IEnumerator JokalariaBerpiztu()
    {
        // jokalariaren mugimendua ezgaitu
        jokalaria.hiltzen = true;
        yield return new WaitForSeconds(.4f);
        fadeManager.Ilundu();
        // jokalaria azken checkpointera mugitu eta jokoaren aurreko egoera berrezarri
        yield return new WaitForSeconds(hilAnimazioa);
        StartCoroutine(CheckPointeraMugitu());
    }

    IEnumerator CheckPointeraMugitu()
    {
        jokalaria.transform.position = checkpoint.transform.position;
        checkpoint.GetComponent<Checkpoint>().EtsaiakAgerrarazi();
        // !!! tranpak erreseteatu
        // camera bound aldatu
        cam.GetComponent<VCam>().CameraConfinerKudeatu(checkpoint.transform.position);
        jokalaria.GetComponent<Renderer>().enabled = false;
        fadeManager.Argitu();
        // mapa erreseteatu

        // animazioa kargatzeko behar duen denbora
        yield return new WaitForSeconds(.4f);
        jokalaria.berpizten = true;
        inbentarioa.Berpiztu();
        yield return new WaitForSeconds(.04f);
        jokalaria.GetComponent<Renderer>().enabled = true;
        // jokalariari txanpon batzuk kendu

        yield return new WaitForSeconds(berpiztuAnimazioa);
        jokalaria.hiltzen = false;
        jokalaria.berpizten = false;
    }

    public void Gorde()
    {
        PlayerData datuak = inbentarioa.Gorde();
        datuak.Eszenatokia = fadeManager.GetCurrentScene(); // ! fademanager derrigorrez egongo da eszenatoki guztietan ? !
        CheckpointZenbakiaLortu();
        datuak.checkPointZenbakia = checkpointZenbakia;

        // datuak gorde
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + dataPath);
        bf.Serialize(file, datuak);
        file.Close();
    }

    // eszenatokia -> main menu baruan? beste fitxategi bat? !!!
    // gela argitu arte jokalariaren mugimendu geldituta -> pause.jokuageldituta = true !!!
    public void Kargatu() // fadeManagerko start-etik fade in kendu? --> checkpoint ezberdin batera joateko ilun egon behar du !!!
    {
        if(File.Exists(Application.persistentDataPath + dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + dataPath, FileMode.Open);
            PlayerData datuak = (PlayerData)bf.Deserialize(file);
            file.Close();

            inbentarioa.Kargatu(datuak);
            CheckpointeraMugitu(datuak.checkPointZenbakia);
        }
    }

    [Serializable]
    public class PlayerData // eszenatokia hemen ? beste fitxategi bat main menuk kargatzea? 
    {
        public int Eszenatokia;
        public List<Item> itemak;
        public int geziKopurua, geziKopuruMax;
        public int bizitzaPuntuak, bizitzaPuntuMax;
        public int txanponKopurua;
        public int checkPointZenbakia;
    }

}
