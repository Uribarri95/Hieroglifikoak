using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokalariKudetzailea : MonoBehaviour {

    public GameObject checkpoint;

    private JokalariMug jokalaria;

    float hilDenbora = .6f;

	// Use this for initialization
	void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerRespawn()
    {
        StartCoroutine("PlayerRespawnItxaron");
    }

    public IEnumerator PlayerRespawnItxaron()
    {
        jokalaria.enabled = false;
        jokalaria.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(hilDenbora);
        jokalaria.AbiaduraAldatu(new Vector2(0, 0));
        jokalaria.NoranzkoaAldatu(1);
        jokalaria.transform.position = checkpoint.transform.position;
        jokalaria.enabled = true;
        jokalaria.GetComponent<Renderer>().enabled = true;
    }
}
