using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    JokalariKudetzailea kudetzailea;
    Animator anim;

	// Use this for initialization
	void Start () {
        kudetzailea = GetComponentInParent<JokalariKudetzailea>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            kudetzailea.checkpoint = gameObject;
            anim.SetBool("zabaldu", true);
        }
    }
}
