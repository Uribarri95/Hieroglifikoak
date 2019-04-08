using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranpakEzgaitu : MonoBehaviour {

    TranpaKudeaketa tranpaKudeaketa;

	// Use this for initialization
	void Start () {
        tranpaKudeaketa = GetComponentInParent<TranpaKudeaketa>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(tranpaKudeaketa != null)
            {
                tranpaKudeaketa.TranpakDesaktibatu();
            }
        }
    }
}
