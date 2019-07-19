using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelaBerria : MonoBehaviour {

    public FadeManager fadeManager;
    public GameObject kamera;
    public EtsaiKudeaketa etsaiak;
    public GelaBerria irteeraPuntua;
    public bool eskuma;
    public bool mugitu;

    JokalariMug jokalaria;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            jokalaria = collision.GetComponent<JokalariMug>();
            // jokalaria gelatik irteten da
            if (mugitu)
            {
                if (!jokalaria.gelaAldatzen)
                {
                    jokalaria.SetGelaAldaketa(eskuma, true);
                    fadeManager.Ilundu();
                }
                else
                {
                    jokalaria.SetGelaAldaketa(eskuma, false);
                }
            }
            // jokalariaren tokia aldatzen da
            else
            {
                if (!jokalaria.eskumarantz != eskuma)
                {
                    if (etsaiak != null)
                    {
                        etsaiak.EtsaiakKendu();
                    }

                    jokalaria.transform.position = irteeraPuntua.transform.position;
                }
                else
                {
                    kamera.GetComponent<VCam>().CameraConfinerKudeatu(transform.position);
                    fadeManager.Argitu(.5f);
                    if (etsaiak != null)
                    {
                        etsaiak.EtsaiakReset();
                    }
                }
            }
        }
        // etsaia gelatik ez irteteko
        else if(collision.tag == "Etsaia")
        {
            collision.GetComponent<Etsaia>().Hil();
        }
    }
}
