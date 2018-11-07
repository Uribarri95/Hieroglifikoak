using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour {

    TranpaManager tranpak;

	// Use this for initialization
	void Start () {
        tranpak = GetComponentInParent<TranpaManager>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            tranpak.TranpakKudeatu(true);
        }
    }

}
