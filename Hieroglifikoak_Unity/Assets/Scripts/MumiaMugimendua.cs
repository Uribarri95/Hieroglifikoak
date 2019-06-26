using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MumiaMugimendua : MonoBehaviour {

    public GameObject mumiBesoa;        // hiltzean besoa objectua jauzteko
    public LayerMask jokalaria;         // jokalaria jarraitu/kolpatzeko
    public LayerMask oztopoak;          // horma/lurra detektatzeko
    public Transform groundCheck;       // horma/lurra detektatzeko
    public Transform erasoPuntua;       // eraso zirkunferentziaren erdigunea

    Animator anim;
    float abiadura;                     // mumiaren mugimendu abiadura
    float abiaduraNormala = .7f;        // abiadura jokalaria ikusten ez duenean
    float abiaduraAzkarra = 1f;         // abiadura jokalaria jarraitzen
    float zoruDistantzia = .3f;         // zorua topatzeko izpi luzera
    float hormaDistantzia = .1f;        // horma topatzeko izpi luzera
    float knockBack = 15;               // kolpea jasotzean bultzatzen den indarra
    bool eskumaBegira = false;          // irudiari buelta emateko
    bool mugituAgindua = false;         // errena den efektua emoteko
    bool idleMoveKontrola = false;      // false denean mugitu daiteke, bestela ez
    bool idleBukatuta = false;          // horma/zuloa topatzean pixka bat itzarongo du buelta eman baino lehen
    bool erasoDezaket = true;           // erasoDenbora betetzeko 

    // ezberdinak mumia eta besoan
    public float mugitzekoDenbora;      // bi pausoren artean denbora txiki bat emoteko
    public float begiradaErradioa;      // jokalaria jarraitzeko distantzia
    public float erasoErradioa;         // jokalaria kolpatzeko zirkunferentziaren erradioa
    public float erasoDistantzia;       // erasotzeko distantzia minimoa
    public float erasoDenbora;          // denbora txiki bat bi erasoen artean
    

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        abiadura = abiaduraNormala;
    }

    // mugituAgindua eta idleMoveKontrola mumia erren dagoen efektua emoten diote
    // jokalaria begiradaErradioa-ren barruan dagioenean mumia jokalaria jarraituko du, bestela alde batetik bestera mugituko da zulo baten jauzi gabe eta hormaren aurka joan gabe
    // jokalaria jarritzen dagoenean azkarrago mugitzen da
    // jokalaria gertu dagoenean eraso egiten du
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

    // kolpea jasotzean etsaia atzerantz bultzatu
    // mumia hiltzen denean besoa jauzten utziko du
    public void KolpeaJaso()
    {
        if (GetComponent<Etsaia>().GetKnockBack() /*&& (!anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_attack") || !anim.GetCurrentAnimatorStateInfo(0).IsName("mummy_arm_attack"))*/)
        {
            transform.Translate(Vector2.right * knockBack * Time.deltaTime);
            GetComponent<Etsaia>().KnockBackErreseteatu();
        }
        if (!GetComponent<Etsaia>().GetBizirik())
        {
            if (!name.Contains("arm"))
            {
                GameObject mumiBesoaGO = Instantiate(mumiBesoa, transform.position, transform.rotation);
                mumiBesoaGO.transform.parent = transform.parent;
                MumiaMugimendua mumia = mumiBesoaGO.GetComponent<MumiaMugimendua>();
                if (mumia != null)
                {
                    mumia.GetComponent<Animator>().SetTrigger(Random.Range(0, 2) == 0 ? "rspin" : "lspin");
                    mumia.BesoaJauzi();
                }
            }
            Destroy(gameObject);
        }
        /*if (!name.Contains("arm") && !GetComponent<Etsaia>().GetBizirik())
        {
            Destroy(gameObject);
            GameObject mumiBesoaGO = Instantiate(mumiBesoa, transform.position, transform.rotation);
            mumiBesoaGO.transform.parent = transform.parent;
            MumiaMugimendua mumia = mumiBesoaGO.GetComponent<MumiaMugimendua>();
            if (mumia != null)
            {
                mumia.GetComponent<Animator>().SetTrigger(Random.Range(0, 2) == 0 ? "rspin" : "lspin");
                mumia.BesoaJauzi();
            }
        }*/
    }

    // besoa mumitik jauzten denean salto txiki bat ematen du
    void BesoaJauzi()
    {
        transform.Translate(Vector2.up * 2 * knockBack * Time.deltaTime);
        GetComponent<Etsaia>().KnockBackErreseteatu();
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
        float auzaskoDenbora = Random.Range(0.5f, 2);
        yield return new WaitForSeconds(auzaskoDenbora);
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

    // jokalaria jarraitzen ez dagoenean, oztopoa/zuloa aurkitzean buelta ematen du
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

    // aurrean lurra dagoen esaten du, aldapan ere ez da sartzen
    bool LurraBilatu()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, zoruDistantzia, oztopoak);
    }

    // aurrean horma dagoen esaten du
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
