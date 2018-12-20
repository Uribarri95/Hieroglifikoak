using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinEmanKolpatzean : MonoBehaviour {

    // jokalaria kolpatu da
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            bool eskuma = transform.position.x > collision.transform.position.x ? true : false;
            JokalariaKolpatu(collision.transform.GetComponent<Eraso>(), eskuma);
        }
    }

    // jokalaria kolpatzen da, saguzar txikia bada, suntsitu egiten da
    public void JokalariaKolpatu(Eraso eraso, bool eskuma)
    {
        eraso.KolpeaJaso(eskuma);
        if (gameObject.name.Contains("Saguzar_txikia"))
        {
            GetComponent<Etsaia>().Hil();
        }
    }

}
