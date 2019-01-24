using System.Collections;
using System.Collections.Generic;
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

    // true denean input-ak ezgaitu
    public static bool jokuaGeldituta = false;

    public GameObject checkpoint;
    public GameObject cam;
    public Transition trantzizioa;
    
    private JokalariMug jokalaria;
    Inbentarioa inbentarioa;

    public float hilAnimazioa = 1.2f;
    public float berpiztuAnimazioa = 1.2f;

    public bool gelditu = false;
    public bool abiarazi = false;

    // Use this for initialization
    void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
        inbentarioa = Inbentarioa.instantzia;
    }

    private void Update()
    {
        if (gelditu)
        {
            gelditu = false;
            jokuaGeldituta = true;
            Time.timeScale = 0f;
        }
        if (abiarazi)
        {
            abiarazi = false;
            jokuaGeldituta = false;
            Time.timeScale = 1f;
        }
    }

    // pause -> jokuaGeldituta = true -> dialog-a gelditzeko !!!
    // pause eta dialog-ak
    // puzlea dagoen bitartean ere
    public void TogglePause()
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
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
        trantzizioa.FadeOut();
        // jokalaria azken checkpointera mugitu eta jokoaren aurreko egoera berrezarri
        yield return new WaitForSeconds(hilAnimazioa);
        jokalaria.transform.position = checkpoint.transform.position;
        checkpoint.GetComponent<Checkpoint>().EtsaiakAgerrarazi();
        // camera bound aldatu
        cam.GetComponent<VCam>().CameraConfinerKudeatu(checkpoint.transform.position);
        jokalaria.GetComponent<Renderer>().enabled = false;
        trantzizioa.FadeIn();
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
}
