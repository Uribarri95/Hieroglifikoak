using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointKudetzailea : MonoBehaviour {

    public GameObject checkpoint;

    private JokalariMug jokalaria;

	// Use this for initialization
	void Start () {
        //jokalaria = GetComponent<JokalariMug>();
        jokalaria = FindObjectOfType<JokalariMug>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerRespawn()
    {
        jokalaria.transform.position = checkpoint.transform.position;
    }
}
