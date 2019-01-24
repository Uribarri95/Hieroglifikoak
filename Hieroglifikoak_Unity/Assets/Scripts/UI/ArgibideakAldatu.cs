using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArgibideakAldatu : MonoBehaviour {

    Ekintzak argibideak;
    Text textua;

	// Use this for initialization
	void Start () {
        argibideak = Ekintzak.instantzia;
        textua = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            textua.text = argibideak.GetArgibidea();
        }
	}
}
