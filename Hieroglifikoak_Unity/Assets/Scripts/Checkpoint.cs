using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public JokalariKudetzailea kudetzailea;

	// Use this for initialization
	void Start () {
		kudetzailea = FindObjectOfType<JokalariKudetzailea>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            kudetzailea.checkpoint = gameObject;
    }
}
