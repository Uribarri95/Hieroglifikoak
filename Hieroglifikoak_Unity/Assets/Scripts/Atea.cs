using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atea : MonoBehaviour {

    JokalariMug jokalaria;
    Animator playerAnim;
    Animator anim;
    Animator exitAnim;

    public GameObject irteeraAtea;
    public GameObject cam;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        exitAnim = irteeraAtea.GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            jokalaria = collision.GetComponent<JokalariMug>();
            playerAnim = collision.GetComponent<Animator>();
            jokalaria.setAteAurrean(true);
            if (Input.GetKeyDown(KeyCode.DownArrow) && jokalaria.GetLurrean() && !jokalaria.GetAteaZeharkatzen())
            {
                jokalaria.SetAteaZeharkatzen(true);

                anim.SetBool("zabaldu", true);
                exitAnim.SetBool("zabaldu", true);

                StartCoroutine(AteanSartu());
            }
        }
    }

    IEnumerator AteanSartu()
    {
        yield return new WaitForSeconds(.1f);

        float posX = gameObject.transform.position.x;
        jokalaria.transform.position = new Vector2(posX, jokalaria.transform.position.y);
        jokalaria.SetAbiaduraHorizontala(0);

        yield return new WaitForSeconds(.3f);

        playerAnim.SetBool("ateanSartu", true);
        // trantzizio animazioa sartu

        yield return new WaitForSeconds(.1f);

        playerAnim.SetBool("ateanSartu", false);

        yield return new WaitForSeconds(2);

        cam.GetComponent<VCam>().CameraConfinerKudeatu(irteeraAtea.transform.position);
        jokalaria.transform.position = new Vector2(irteeraAtea.transform.position.x, irteeraAtea.transform.position.y);
        // trantzizio animazioa irten

        yield return new WaitForSeconds(.3f);

        playerAnim.SetBool("atetikIrten", true);

        yield return new WaitForSeconds(.6f);
        
        playerAnim.SetBool("atetikIrten", false);

        yield return new WaitForSeconds(.4f);

        anim.SetBool("zabaldu", false);
        exitAnim.SetBool("zabaldu", false);

        yield return new WaitForSeconds(.4f);
        jokalaria.SetAteaZeharkatzen(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            jokalaria = collision.GetComponent<JokalariMug>();
            jokalaria.setAteAurrean(false);
        }
    }

}
