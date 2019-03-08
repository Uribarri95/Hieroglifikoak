using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinEmanKolpatzean : MonoBehaviour {

    // jokalaria kolpatu da
    private void OnCollisionStay2D(Collision2D collision)
    {
        print(collision.transform.name);
        if (collision.transform.tag == "Player")
        {
            bool eskuma = transform.position.x > collision.transform.position.x ? true : false;
            JokalariaKolpatu(collision.transform.GetComponent<Eraso>(), eskuma);
        }
        else if(collision.transform.name == "Oztopoak" || collision.transform.name == "Plataforma_Zeharkagarria")
        {
            if (gameObject.name.Contains("Saguzar_txikia"))
            {
                GetComponent<Etsaia>().Hil();
            }
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
