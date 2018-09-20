using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokalariaHil : MonoBehaviour {

    JokalariKudetzailea kudetzailea;
    //JokalariMug jokalaria;

    float hilAnimazioa = .6f;

	// Use this for initialization
	void Start () {
        kudetzailea = FindObjectOfType<JokalariKudetzailea>();
        //jokalaria = FindObjectOfType<JokalariMug>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //StartCoroutine("JokalariaHil");
            kudetzailea.PlayerRespawn();
            //pantalla erreseteatu(etsaiak, itemak, kutxak...)
        }
    }

    /*public IEnumerator JokalariaHil()
    {
        //hil animazioa
        jokalaria.AbiaduraAldatu(new Vector2(0, 0));
        yield return new WaitForSeconds(hilAnimazioa);
        kudetzailea.PlayerRespawn();
    }*/
}
