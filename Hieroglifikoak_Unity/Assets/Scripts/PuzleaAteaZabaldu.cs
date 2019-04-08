using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzleaAteaZabaldu : MonoBehaviour {

    public int ekintzaZenbakia;
    Atea atea;

    // Use this for initialization
    void Start () {
        atea = GetComponent<Atea>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Ekintzak.instantzia.GetEkintza(ekintzaZenbakia))
        {
            atea.AteaZabaldu(true);
        }
	}
}
