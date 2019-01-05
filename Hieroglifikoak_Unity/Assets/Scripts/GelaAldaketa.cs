using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelaAldaketa : MonoBehaviour {

    public Transition trantzizioa;
    public GameObject cam;
    public EtsaiKudeaketa etsaiak;
    public float aldaketaDenbora;
    public bool eskuma = true;
    
    JokalariMug jokalaria;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            jokalaria = collision.GetComponent<JokalariMug>();
            if (!trantzizioa.FadeIn())
            {
                jokalaria.SetGelaAldaketa(eskuma);
                trantzizioa.FadeOut();
            }
            else
            {
                cam.GetComponent<VCam>().CameraConfinerKudeatu(jokalaria.transform.position);
                trantzizioa.FadeIn();
                if(etsaiak != null)
                {
                    etsaiak.EtsaiakReset();
                }
            }
        }
        else if (collision.tag == "Etsaia")
        {
            // etsaia erreseteatu
            collision.GetComponent<Etsaia>().Hil();
        }
    }
}
