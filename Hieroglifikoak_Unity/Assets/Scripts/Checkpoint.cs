using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public EtsaiKudeaketa etsaiak;
    // !!! reset
    JokalariKudetzailea kudetzailea;
    Animator anim;
    bool datuakGorde = false;

	// Use this for initialization
	void Start () {
        kudetzailea = GetComponentInParent<JokalariKudetzailea>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!datuakGorde)
            {
                datuakGorde = true;
                kudetzailea.checkpoint = gameObject;
                kudetzailea.DatuakGorde();
                anim.SetTrigger("zabaldu");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            datuakGorde = false;
        }
    }

    public void EtsaiakAgerrarazi()
    {
        if(etsaiak != null)
        {
            etsaiak.EtsaiakReset();
        }
    }
}
