using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelaAldaketa : MonoBehaviour {

    public FadeManager fadeManager;
    public GameObject cam;
    public EtsaiKudeaketa etsaiak;
    public GelaAldaketa irteeraGela;
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
                //StartCoroutine(GelatikIrten());
            }
            else // urteten
            {
                //cam.GetComponent<VCam>().CameraConfinerKudeatu(jokalaria.transform.position);
                cam.GetComponent<VCam>().CameraConfinerKudeatu(transform.position);
                fadeManager.Argitu(.5f);
                if (etsaiak != null)
                {
                    etsaiak.EtsaiakReset();
                }
                //StartCoroutine(GelanSartu());
            }
        }
        else if (collision.tag == "Etsaia")
        {
            // etsaia erreseteatu
            collision.GetComponent<Etsaia>().Hil();
        }
    }

    IEnumerator GelatikIrten()
    {
        jokalaria.SetGelaAldaketa(eskuma);
        fadeManager.Ilundu();
        yield return new WaitForSeconds(1.5f);
        jokalaria.transform.position = irteeraGela.transform.position;
    }

    IEnumerator GelanSartu()
    {
        yield return new WaitForSeconds(.5f);
        cam.GetComponent<VCam>().CameraConfinerKudeatu(transform.position);
        fadeManager.Argitu(.5f);
        if (etsaiak != null)
        {
            etsaiak.EtsaiakReset();
        }
    }
}