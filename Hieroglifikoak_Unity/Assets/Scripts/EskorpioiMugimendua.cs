using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EskorpioiMugimendua : MonoBehaviour {

    public GameObject jaurtigaia;       // jokalariari jaurtitzen den pozoi objektua
    public Transform erasoPuntua;       // pozoia botatzen den puntua
    public LayerMask jokalaria;         // jokalaria ikusteko, pozoia jaurtitzeko
    public LayerMask izpia;             // erasoa detektatuko dituen objektuak

    Animator anim;
    Vector3 jokalariPos;                // pozoia jaurtiko den puntua, jokalaria dagoen tokia

    public float erasoMaiztasuna = 8;       // erasoen arteko denbora tartea
    float abiadura = .5f;                   // mugimendu abiadura
    float knockBack = 15;                   // kolpea jasotzean bultzatzen den indarra
    float erasoErradioa = 10;               // eskorpioia ikusten den distantzia
    float offset = 180;                     // pozoia ezkerrerantz botatzen da beti, 180º
    bool eskumaBegira;                      // irudiari buelta emateko
    bool erasoDezaket;                      // erasoMaiztasuna betetzeko

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        eskumaBegira = false;
        erasoDezaket = true;
        StartCoroutine("ErasoaKargatu");
    }
	
	// jokalaria ikusten ez badu, pixka bat mugitzen da eta gero gelditu
    // jokalaria ikusten badu, eraso ahal badu erasotzen du, bestela pixka bat mugitzen da edo geldi geratzen da segundu batzuk
	void Update ()
    {
        KolpeaJaso();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("scorpion_idle"))
        {
            anim.SetTrigger("mugitu");
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("scorpion_move_forward"))
        {
            transform.Translate(Vector2.left * abiadura * Time.deltaTime);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("scorpion_move_backward"))
        {
            transform.Translate(Vector2.right * abiadura * Time.deltaTime);
        }

        Collider2D target = Physics2D.OverlapCircle(transform.position, erasoErradioa, jokalaria);
        if (target != null)
        {
            jokalariPos = target.transform.position;
            if ((transform.position.x < jokalariPos.x && !eskumaBegira) || (transform.position.x > jokalariPos.x && eskumaBegira))
            {
                BueltaEman();
            }
            /*if (erasoDezaket)
                {
                    erasoDezaket = false;
                    anim.SetBool("eraso", true);
                }*/

            Vector2 norabidea = target.transform.position - erasoPuntua.transform.position;
            RaycastHit2D erasoIzpia = Physics2D.Raycast(erasoPuntua.transform.position, norabidea, erasoErradioa, izpia);
            if (erasoIzpia.collider.tag == "Player")
            {
                if (erasoDezaket)
                {
                    erasoDezaket = false;
                    anim.SetBool("eraso", true);
                }
            }
        }
	}

    // animazioko event jaurtitzen du
    // pozoia jaurtitzen da jokalaria ikusi duen norabidean
    void PozoiaJaurti()
    {
        anim.SetBool("eraso", false);
        Vector3 noranzkoa = jokalariPos - transform.position;
        float zAngelua = Mathf.Atan2(noranzkoa.y, noranzkoa.x) * Mathf.Rad2Deg;
        Instantiate(jaurtigaia, erasoPuntua.position, Quaternion.Euler(transform.rotation.x, transform.position.y ,zAngelua + offset));
    }

    // eraso dezake / ezin du eraso zikloa kontrolatzen du
    IEnumerator ErasoaKargatu()
    {
        yield return new WaitForSeconds(erasoMaiztasuna);
        if (!erasoDezaket)
        {
            erasoDezaket = true;
        }
        StartCoroutine("ErasoaKargatu");
    }

    // kolpea jasotzean etsaia atzerantz bultzatu
    public void KolpeaJaso()
    {
        if ((!anim.GetCurrentAnimatorStateInfo(0).IsName("scorpion_move_forward") || !anim.GetCurrentAnimatorStateInfo(0).IsName("scorpion_move_backward")) 
            && GetComponent<Etsaia>().GetKnockBack())
        {
            transform.Translate(Vector2.right * knockBack * Time.deltaTime);
            GetComponent<Etsaia>().KnockBackErreseteatu();
        }
    }

    // jokalariari begiratzeko irudia eman irudiari
    void BueltaEman()
    {
        eskumaBegira = !eskumaBegira;
        transform.eulerAngles = new Vector3(0, eskumaBegira ? -180 : 0, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, erasoErradioa);
    }
}
