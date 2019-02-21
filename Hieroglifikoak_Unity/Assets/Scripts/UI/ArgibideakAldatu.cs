﻿using System.Collections;
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
        textua = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            mezua = argibideak.GetArgibidea();
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

    public void textuaAldatu(string mezua) // !!! azpikoa funtzionatzen badu ezabatu
    {
        argibideBerria = true;
        textua.text = mezua;
    }

    public void ArgibideakJarri()
    {
        argibideBerria = false;
    }

    public IEnumerator BehinBehinekoTextua(string mezua)
    {
        argibideBerria = true;
        textua.color = Color.red;
        textua.text = mezua;
        yield return new WaitForSeconds(4f);
        argibideBerria = false;
        textua.color = Color.black;
        textua.text = mezua;
    }
}
