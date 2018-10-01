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
        jokalaria.berpizten = true;
        jokalaria.transform.position = checkpoint.transform.position;
        // jokalariari txanpon batzuk kendu
        // etsaiak berpiztu

        yield return new WaitForSeconds(berpiztuAnimazioa);
        jokalaria.hiltzen = false;
        jokalaria.berpizten = false;
    }
}
