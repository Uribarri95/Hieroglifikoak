using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etsaia : MonoBehaviour {

    public float bizitzaPuntuak;
    public GameObject hilPartikula;
    bool kolpatuta;

	// Use this for initialization
	void Start () {
        kolpatuta = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void KolpeaJaso(float minPuntuak)
    {
        if (!kolpatuta)
        {
            bizitzaPuntuak -= minPuntuak;

            Debug.Log("Ouch. " + minPuntuak + " bizitzaPuntu gutxiago.");
            Debug.Log(bizitzaPuntuak);

            if (bizitzaPuntuak <= 0)
            {
                Hil();
            }
        }
    }

    void Hil()
    {
        Destroy(gameObject);
        Instantiate(hilPartikula, transform.position, Quaternion.identity);
        Debug.Log(gameObject.name + " hil da.");
    }

    public void SetKolpea(bool kolpea)
    {
        kolpatuta = kolpea;
    }
}
