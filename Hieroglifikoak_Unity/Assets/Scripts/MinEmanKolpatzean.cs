using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinEmanKolpatzean : MonoBehaviour {

    private JokalariKudetzailea jokalariKudetzailea;

    void Start()
    {
        jokalariKudetzailea = FindObjectOfType<JokalariKudetzailea>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            bool eskuma = transform.position.x > collision.transform.position.x ? true : false;
            JokalariaKolpatu(collision.transform.GetComponent<Eraso>(), eskuma);
        }
    }

    public void JokalariaKolpatu(Eraso eraso, bool eskuma)
    {
        Debug.Log("Jokalaria kolpatu!");
        if (eraso.KolpeaJaso(eskuma))
        {
            jokalariKudetzailea.JokalariaHil();
        }
        if (gameObject.name == "Saguzar_txikia")
        {
            GetComponent<Etsaia>().Hil();
        }
    }

}
