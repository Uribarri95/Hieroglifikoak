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
    public SceneLoader sceneLoader;

    public float hilAnimazioa = 1.2f;
    public float berpiztuAnimazioa = 1.2f;

    public bool aktibatu;           // temporal
    public int checkpointZenbakia;

    private JokalariMug jokalaria;
    Inbentarioa inbentarioa;
    Data jokalariDatuak;

    // Use this for initialization
    void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
        inbentarioa = Inbentarioa.instantzia;

        jokalariDatuak = Data.instantzia;
        DatuakKargatu();
        //fadeManager.Argitu(2f); // !!!dialog triggerraren aktibazio denbora luzatu baita ere! 
    }

    private void Update()
    {
        if (aktibatu)
        {
            aktibatu = false;
            CheckPointAldatu(checkpointZenbakia);
        }
    }

    void DatuakKargatu()
    {
        Data.PlayerData datuak = jokalariDatuak.Kargatu();
        CheckPointAldatu(datuak.checkPointZenbakia);
        inbentarioa.Kargatu(datuak);
    }

    public void DatuakGorde()
    {
        Data.PlayerData datuak = inbentarioa.Gorde();
        datuak.Eszenatokia = sceneLoader.GetCurrentScene();
        CheckpointZenbakiaLortu();
        datuak.checkPointZenbakia = checkpointZenbakia;
        datuak.ekintzak = Ekintzak.instantzia.ekintzak;

        jokalariDatuak.Gorde(datuak);
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

    public void CheckPointAldatu(int zenbakia)
    {
        checkpointZenbakia = zenbakia;
        checkpoint = transform.GetChild(zenbakia).gameObject;
        StartCoroutine(CheckPointeraMugitu());
    }

    public void JokalariaHil()
    {
        if (!jokalaria.hiltzen)
        {
            // jokalariaren mugimendua ezgaitu
            jokalaria.hiltzen = true;
            inbentarioa.JokalariaHil();
            StartCoroutine(JokalariaBerpiztu());
        }
    }

    IEnumerator JokalariaBerpiztu()
    {
        yield return new WaitForSeconds(.4f);
        fadeManager.Ilundu();
        // jokalaria azken checkpointera mugitu eta jokoaren aurreko egoera berrezarri
        yield return new WaitForSeconds(hilAnimazioa);
        StartCoroutine(CheckPointeraMugitu());
    }

    IEnumerator CheckPointeraMugitu()       // 2 zatitan banatu !!!
    {
        jokalaria.transform.position = checkpoint.transform.position;
        checkpoint.GetComponent<Checkpoint>().EtsaiakAgerrarazi();

        // camera bound aldatu
        yield return new WaitForSeconds(1f);
        cam.GetComponent<VCam>().CameraConfinerKudeatu(checkpoint.transform.position);
        jokalaria.GetComponent<Renderer>().enabled = false;
        fadeManager.Argitu(2);      // !!!dialog triggerraren aktibazio denbora luzatu baita ere! 
        // mapa erreseteatu

        // animazioa kargatzeko behar duen denbora
        yield return new WaitForSeconds(2.4f);
        jokalaria.berpizten = true;
        inbentarioa.Berpiztu();
        yield return new WaitForSeconds(.04f);
        jokalaria.GetComponent<Renderer>().enabled = true;
        // jokalariari txanpon batzuk kendu

        yield return new WaitForSeconds(berpiztuAnimazioa);
        jokalaria.hiltzen = false;
        jokalaria.berpizten = false;
    }
}
