using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokalariKudetzailea : MonoBehaviour {

    public GameObject checkpoint;

    private JokalariMug jokalaria;

    public float hilAnimazioa = 1.2f;
    public float berpiztuAnimazioa = 1.2f;

	// Use this for initialization
	void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void JokalariaHil()
    {
        if (!jokalaria.hiltzen)
        {
            StartCoroutine("ItxaronJokalariaHil");
        }
    }

    public IEnumerator ItxaronJokalariaHil()
    {
        // jokalariaren mugimendua ezgaitu
        jokalaria.hiltzen = true;

        // jokalaria azken checkpointera mugitu eta jokoaren aurreko egoera berrezarri
        yield return new WaitForSeconds(hilAnimazioa);
        jokalaria.transform.position = checkpoint.transform.position;
        jokalaria.GetComponent<Renderer>().enabled = false;
        // mapa erreseteatu

        // animazioa kargatzeko behar duen denbora
        yield return new WaitForSeconds(.4f);
        jokalaria.berpizten = true;
        yield return new WaitForSeconds(.04f);
        jokalaria.GetComponent<Renderer>().enabled = true;
        // jokalariari txanpon batzuk kendu
        // etsaiak berpiztu

        yield return new WaitForSeconds(berpiztuAnimazioa);
        jokalaria.hiltzen = false;
        jokalaria.berpizten = false;
    }
}
