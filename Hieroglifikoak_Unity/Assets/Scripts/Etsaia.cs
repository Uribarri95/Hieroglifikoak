using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etsaia : MonoBehaviour {

    public int bizitzaPuntuak;
    public GameObject hilParticula;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void KolpeaJaso(int minPuntuak)
    {
        bizitzaPuntuak -= minPuntuak;
        Debug.Log("Ouch. " + minPuntuak + "bizitzaPuntu gutxiago.");

        if(bizitzaPuntuak <= 0)
        {
            Hil();
        }
    }

    void Hil()
    {
        Destroy(gameObject);
        Instantiate(hilParticula, transform.position, Quaternion.identity);
        Debug.Log(gameObject.name + " hil da.");
    }
}
