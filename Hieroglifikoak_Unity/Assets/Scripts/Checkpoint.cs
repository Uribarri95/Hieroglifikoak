﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private JokalariKudetzailea kudetzailea;

	// Use this for initialization
	void Start () {
        kudetzailea = GetComponentInParent<JokalariKudetzailea>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            kudetzailea.checkpoint = gameObject;
    }
}
