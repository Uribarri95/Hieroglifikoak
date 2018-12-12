using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MumiaMugimendua : MonoBehaviour {

    public LayerMask jokalaria;
    public LayerMask oztopoak;
    public Transform groundCheck;
    public Transform erasoPuntua;

    Animator anim;

    public float mugitzekoDenbora = .5f;    // bi pausoren artean denbora txiki bat emoteko
    float abiadura;                         // mumiaren mugimendu abiadura
    public float abiaduraNormala = .7f;
    public float abiaduraAzkarra = 1;
    float zoruDistantzia = .5f;
    float hormaDistantzia = .1f;
    bool eskumaBegira = false;
    bool mugituAgindua = false;         // errena den efektua emoteko
    bool idleMoveKontrola = false;      // false denean mugitu daiteke, bestela ez
    bool idleBukatuta = false;

    public float begiradaErradioa;      // jokalaria jarraitzeko distantzia
    public float erasoErradioa;         // jokalaria kolpatzeko zirkunferentziaren erradioa
    public float erasoDistantzia;       // erasotzeko distantzia minimoa
    public float erasoDenbora;          // denbora txiki bat bi erasoen artean
    bool erasoDezaket = true;           // erasoDenbora betetzeko 

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        abiadura = abiaduraNormala;
    }

    // mugituAgindua eta idleMoveKontrola mumia erren dagoen efektua emoten diote
    // jokalaria begiradaErradioa-ren barruan dagioenean mumia jokalaria jarraituko du, bestela alde batetik bestera mugituko da zulo baten jauzi gabe eta hormaren aurka joan gabe
    // jokalaria jarritzen dagoenean azkarrago mugitzen da
    void Update()
    {
        KolpeaJaso();
        Collider2D target = Physics2D.OverlapCircle(transform.position, begiradaErradioa, jokalaria);
        if (target != null)
        {
            anim.speed = 1.8f;
            abiadura = abiaduraAzkarra;
            JokalariaJarraitu(target.transform.position);
        }
        else
        {
            anim.speed = 1;
            abiadura = abiaduraNormala;
            Behatu();
        }
        if (mugituAgindua)
        {
            transform.Translate(Vector2.left * abiadura * Time.deltaTime);
        }

        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_arm_idle")) && !idleMoveKontrola)
        {
            StartCoroutine("Mugimendua");
        }
    }

    public void KolpeaJaso()
    {
        if (GetComponent<Etsaia>().GetKnockBack() && (!anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_attack") || !anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_arm_attack")))
        {
            transform.Translate(Vector2.right * 15 * Time.deltaTime);
            GetComponent<Etsaia>().KnockBackErreseteatu();
        }
    }

    // mumiaren mugimendu animazioa hasiko da denbora txiki bat pasa ondoren
    IEnumerator Mugimendua()
    {
        yield return new WaitForSeconds(mugitzekoDenbora);
        anim.SetBool("mugimendua", true);
        idleBukatuta = false;
    }

    // mumia buelta erdia ematen du (oztopoa/zuloa aurkitu duelako edo jokalaria jarraitzeko)
    void BueltaEman()
    {
        eskumaBegira = !eskumaBegira;
        transform.eulerAngles = new Vector3(0, eskumaBegira ? -180 : 0, 0);
    }

    // jokalaria mumiaren begirada barruan dago eta bere atzetik doa
    // oztopo bat badago edo jokalaria ez badago irisgarri, itzaroten geratuko da, beti ahal bezain gertu
    // behar beste gerturatu denean jokalaria erasoko du.
    void JokalariaJarraitu(Vector2 jokalariPos)
    {
        if((transform.position.x < jokalariPos.x && !eskumaBegira) || (transform.position.x > jokalariPos.x && eskumaBegira))
        {
            BueltaEman();
        }
        idleMoveKontrola = (!LurraBilatu() || HormaBilatu()) ? true : false;
        if (Vector2.Distance(erasoPuntua.position, jokalariPos) <= erasoDistantzia)
        {
            idleMoveKontrola = true;
            if (erasoDezaket && (!anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_attack") || !anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_arm_attack")))
            {
                StartCoroutine("MumiErasoa");
            }
        }
    }

    // eraso animazioak jaurtitzen du
    // jokalaria kolpatzen du, denbora txiki bat pasa ondoren berriro erasotzeko prest dago
    IEnumerator MumiErasoa()
    {
        erasoDezaket = false;
        anim.SetTrigger("erasoa");        
        yield return new WaitForSeconds(erasoDenbora);
        erasoDezaket = true;
    }

    // animazioko eraso event jaurtitzen du
    void Eraso()
    {
        Collider2D kolpea = Physics2D.OverlapCircle(erasoPuntua.position, erasoErradioa, jokalaria);
        if (kolpea)
        {
            bool eskuma = transform.position.x > kolpea.transform.position.x ? true : false;
            GetComponent<MinEmanKolpatzean>().JokalariaKolpatu(kolpea.transform.GetComponent<Eraso>(), eskuma);
        }
    }

    // oztopoa/zuloa aurkitzean buelta ematen du
    void Behatu()
    {
        idleMoveKontrola = false;
        if ((!LurraBilatu() || HormaBilatu()) && (anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_arm_idle")))
        {
            idleMoveKontrola = true;
            if (idleBukatuta)
            {
                BueltaEman();
                idleBukatuta = false;
                idleMoveKontrola = (!LurraBilatu() || HormaBilatu()) ? true : false;
            }
        }
    }

    // animazioko event jaurtitzen du
    public void IdleBukatuta()
    {
        idleBukatuta = true;
    }

    // animazioko event jaurtitzen du
    public void Mugitu()
    {
        mugituAgindua = true;
    }

    // animazioko event jaurtitzen du
    public void Gelditu()
    {
        anim.SetBool("mugimendua", false);
        mugituAgindua = false;
    }

    bool LurraBilatu()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, zoruDistantzia, oztopoak);
    }

    bool HormaBilatu()
    {
        return Physics2D.Raycast(groundCheck.position, eskumaBegira ? Vector2.right : Vector2.left, hormaDistantzia, oztopoak); ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, begiradaErradioa);
        Gizmos.DrawWireSphere(erasoPuntua.position, erasoErradioa);
    }
}
