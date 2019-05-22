using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinEmanKolpatzean : MonoBehaviour {

    // jokalaria kolpatu eta saguzar txikiaren kasuan hormaren aurka suntsitu
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            print("jokalaria kolpatu");
            bool eskuma = transform.position.x > collision.transform.position.x ? true : false;
            JokalariaKolpatu(collision.transform.GetComponent<Eraso>(), eskuma);
        }
        else if (collision.transform.name == "Oztopoak" || collision.transform.name == "Plataforma_Zeharkagarria")
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
        else if (gameObject.transform.parent.name.Contains("kakalardo_bola"))
        {
            GetComponentInParent<KakalardoBola>().BolaSuntsitu();
        }
    }

}
