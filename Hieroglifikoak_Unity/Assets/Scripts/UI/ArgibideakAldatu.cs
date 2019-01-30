using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArgibideakAldatu : MonoBehaviour {

    Ekintzak argibideak;
    Text textua;
    bool argibideBerria = false;
    string mezua;

	// Use this for initialization
	void Start () {
        argibideak = Ekintzak.instantzia;
        textua = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf && !argibideBerria)
        {
            mezua = argibideak.GetArgibidea();
            textua.text = argibideak.GetArgibidea();
        }
	}

    public void textuaAldatu() // !!! azpikoa funtzionatzen badu ezabatu
    {
        StartCoroutine(BehinBehinekoTextua());
    }

    public IEnumerator BehinBehinekoTextua()
    {
        argibideBerria = true;
        textua.color = Color.red;
        textua.text = "KODEA EZ DA ZUZENA, DUDARIK BADAUKAZU SAKATU 'P' EDO 'ESC' TEKLA ETA BEGIRATU HIZTEGIA ATALA.";
        yield return new WaitForSeconds(5f);
        argibideBerria = false;
        textua.color = Color.black;
        textua.text = mezua;
    }
}
