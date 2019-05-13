using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArgibideakAldatu : MonoBehaviour {

    Ekintzak argibideak;
    Text textua;
    bool argibideBerria = false;
    string argibideMezua;

	// Use this for initialization
	void Start () {
        argibideak = Ekintzak.instantzia;
        textua = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            argibideMezua = argibideak.GetArgibidea();
        }
        if (!argibideBerria)
        {
            textua.text = argibideak.GetArgibidea();
        }
	}

    public void IkusiEzkutatu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void textuaAldatu(string mezua)
    {
        argibideBerria = true;
        textua.text = mezua;
    }

    public void ArgibideakJarri()
    {
        argibideBerria = false;
        textua.text = argibideak.GetArgibidea();
    }

    public IEnumerator BehinBehinekoTextua(string mezua, Color kolorea)
    {
        argibideBerria = true;
        textua.color = kolorea;
        textua.text = mezua;
        yield return new WaitForSeconds(1f);
        argibideBerria = false;
        textua.color = Color.black;
        textua.text = argibideMezua;
    }
}
