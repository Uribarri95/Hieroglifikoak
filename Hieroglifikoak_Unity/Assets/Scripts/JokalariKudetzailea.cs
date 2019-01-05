using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokalariKudetzailea : MonoBehaviour {

    public GameObject checkpoint;
    public GameObject cam;
    public Transition trantzizioa;
    
    private JokalariMug jokalaria;
    Inbentarioa inbentarioa;

    public float hilAnimazioa = 1.2f;
    public float berpiztuAnimazioa = 1.2f;

	// Use this for initialization
	void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
        inbentarioa = Inbentarioa.instantzia;
    }

    public void JokalariaHil()
    {
        if (!jokalaria.hiltzen)
        {
            inbentarioa.JokalariaHil();
            StartCoroutine(ItxaronJokalariaHil());
        }
    }

    IEnumerator ItxaronJokalariaHil()
    {
        // jokalariaren mugimendua ezgaitu
        jokalaria.hiltzen = true;
        trantzizioa.FadeOut();
        // jokalaria azken checkpointera mugitu eta jokoaren aurreko egoera berrezarri
        yield return new WaitForSeconds(hilAnimazioa);
        jokalaria.transform.position = checkpoint.transform.position;
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
        // etsaiak berpiztu

        yield return new WaitForSeconds(berpiztuAnimazioa);
        jokalaria.hiltzen = false;
        jokalaria.berpizten = false;
    }
}
