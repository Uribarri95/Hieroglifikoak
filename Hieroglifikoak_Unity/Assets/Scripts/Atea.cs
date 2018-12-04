using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atea : MonoBehaviour {

    JokalariMug jokalaria;
    Animator playerAnim;
    Animator anim;
    Animator exitAnim;

    public Transition trantzizioa;
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
        float posX = gameObject.transform.position.x;
        jokalaria.transform.position = new Vector2(posX, jokalaria.transform.position.y);
        jokalaria.SetAbiaduraHorizontala(0);

        yield return new WaitForSeconds(.3f); // jokalaria atea zabaldu baino lehen ez sartzeko

        playerAnim.SetTrigger("ateanSartu");
        trantzizioa.FadeOut();

        yield return new WaitForSeconds(1.25f); // atetik desagertzeko behar duen denbora

        cam.GetComponent<VCam>().CameraConfinerKudeatu(irteeraAtea.transform.position);
        jokalaria.transform.position = new Vector2(irteeraAtea.transform.position.x, irteeraAtea.transform.position.y);

        yield return new WaitForSeconds(1); // jokalaria eta kamera toki berrian denbora

        trantzizioa.FadeIn();
        playerAnim.SetTrigger("atetikIrten");

        yield return new WaitForSeconds(1f); // jokalaria irten ostean atea ixten da

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
