using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelaAldaketa : MonoBehaviour {

    //public Transition trantzizioa;
    public FadeManager fadeManager;
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
            if (!jokalaria.gelaAldaketa) // sartzen
            {
                jokalaria.SetGelaAldaketa(eskuma);
                fadeManager.Ilundu();
                //trantzizioa.FadeOut();
            }
            else // urteten
            {
                cam.GetComponent<VCam>().CameraConfinerKudeatu(jokalaria.transform.position);
                fadeManager.Argitu(.5f);
                //trantzizioa.FadeIn();
                if (etsaiak != null)
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
