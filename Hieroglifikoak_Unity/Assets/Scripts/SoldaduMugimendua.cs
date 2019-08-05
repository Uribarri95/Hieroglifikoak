using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldaduMugimendua : MonoBehaviour {

    public LayerMask jokalaria;         // jokalaria jarraitzeko
    public LayerMask oztopoak;          // horma / lurra detektatzeko
    public Transform groundCheck;       // horma / lurra detektatzeko
    public Transform erasoPuntua;       // lanza botatzeko puntua
    public Transform dashPoint;         // korrika egiten duen erasoan, partikula agertzen den tokia
    public GameObject dashParticle;     // korrika egiten duen erasoko partikula
    public GameObject spear;            // botatzen duen lanza objektua

    Animator anim;

    Vector2 jokalariPos;                // erasoa jokalariaren posiziora
    float abiadura;                     // mugimendu abiadura
    public float abiaduraNormala;       // oinez abiadura
    public float dashAbiadura;          // sprint erasoko abiadura
    float zoruDistantzia = .2f;         // zorua topatzeko izpi luzera
    float hormaDistantzia = .1f;        // horma topatzeko izpi luzera
    float knockBack = 18;               // kolpea jasotzean bultzatzen den indarra

    public float begiradaErradioa;      // jokalaria jarraitzeko distantzia
    public float erasoErradioa;         // jokalaria kolpatzeko erasoaren erradioa
    public float erasoDenbora;          // erasoen arteko denbora
    public float idleDenbora;           // erasoaren ostean geldi mantendu behar den denbora

    int erasoa;                         // hiru erasoetatik zein erabiliko den aukeratzen da
    bool erasoDezaket;                  // erasoDenbora betetzeko
    bool jokalariaJarraitu;             // jokalaria ikusten den ala ez
    bool eskumaBegira;                  // irudiari bueta emateko

    public AudioSource oinezSoinua;
    public AudioSource meleeSoinua;
    public AudioSource dashSoinua;
    public AudioSource spearSoinua;

    void Start () {
        anim = GetComponent<Animator>();
        abiadura = abiaduraNormala;
        erasoDezaket = false;
        jokalariaJarraitu = false;
        StartCoroutine("ErasoaKargatu");
    }
	
	// soldadua alde batetik bestera mugitzen da
    // jokalaria ikusten duenean gerturatu egingo da, 3 erasotik bat egiten du eta geldi geratzen da, gero berriro hasi
	void Update () {
        OinezJarri();
        KolpeaJaso();
        Collider2D target = Physics2D.OverlapCircle(transform.position, begiradaErradioa, jokalaria);
        if (target == null)
        {
            if (!jokalariaJarraitu)
            {
                Behatu();
            }
            else
            {
                jokalariaJarraitu = false;
            }
        }
        else
        {
            if (!jokalariaJarraitu)
            {
                jokalariaJarraitu = true;
                erasoDezaket = false;
                StartCoroutine("ErasoaKargatu");
            }
            else
            {
                jokalariPos = target.transform.position;
                if (!LurraBilatu() || HormaBilatu())
                {
                    BueltaEman();
                }
            }
        }

        anim.SetFloat("speed", abiadura);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_attack"))
        {
            abiadura = dashAbiadura;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_charge_attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_stop_attack") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_throw_spear") || anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_new_spear") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_melee_attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_stop_melee_attack"))
        {
            abiadura = 0;
        }

        transform.Translate(Vector2.left * abiadura * Time.deltaTime);
        // horma/zuloa edo jokalaritik gertu ->> gelditu !!!

        switch (erasoa)
        {
            case 0:
                DashAttack();
                break;
            case 1:
                MeleeAttack();
                break;
            case 2:
                ThrowSpear();
                break;
            default:
                break;
        }
    }

    // jokalaria jarraitzen ez dagoenean, oztopoa/zuloa aurkitzean buelta ematen du
    void Behatu()
    {
        if (!LurraBilatu() || HormaBilatu())
        {
            StartCoroutine("IdleDenbora");
            BueltaEman();
        }
    }

    // eraso aurretik jokalariarengana biratzeko
    bool JokalariaAtzeanDago()
    {
        return (transform.position.x < jokalariPos.x && !eskumaBegira) || (transform.position.x > jokalariPos.x && eskumaBegira);
    }

    // sprint erasoa
    void DashAttack()
    {
        if(erasoDezaket)
        {
            if (JokalariaAtzeanDago())
            {
                BueltaEman();
            }
            anim.SetBool("stop_dash", false);
            erasoDezaket = false;
            abiadura = 0;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_charge_attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_attack") &&
                !anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_stop_attack"))
            {
                anim.SetTrigger("dash_attack");
            }
        }
        if ((!LurraBilatu() || HormaBilatu()) && anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_attack"))
        {
            anim.SetBool("stop_dash", true);
        }
    }

    // animazioko event jaurtitzen du
    // sprint erasoko partikula agertzen da
    void DashEfect()
    {
        Instantiate(dashParticle, dashPoint.position, dashPoint.rotation);
    }

    // jokalaria kolpatzeko erasoa
    void MeleeAttack()
    {
        if (erasoDezaket)
        {
            erasoDezaket = false;
            abiadura = 0;
            anim.SetTrigger("melee_attack");
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_melee_attack"))
        {
            Collider2D kolpea = Physics2D.OverlapCircle(transform.position, erasoErradioa, jokalaria);
            if (kolpea)
            {
                bool eskuma = transform.position.x > kolpea.transform.position.x ? true : false;
                GetComponentInChildren<MinEmanKolpatzean>().JokalariaKolpatu(kolpea.transform.GetComponent<Eraso>(), eskuma);
            }
        }
    }

    // jokalariari gezia bota
    void ThrowSpear()
    {
        if (erasoDezaket)
        {
            if (JokalariaAtzeanDago())
            {
                BueltaEman();
            }
            erasoDezaket = false;
            abiadura = 0;
            anim.SetTrigger("throw_spear");
        }
    }

    // animazioko event jaurtitzen du
    // gezia agertzen da
    void Spear()
    {
        Instantiate(spear, erasoPuntua.position, erasoPuntua.rotation);
    }

    // kolpea jasotzean etsaia atzerantz bultzatu
    public void KolpeaJaso()
    {
        if ((!anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_attack") || !anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_throw_spear") ||
            !anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_melee_attack")) && GetComponent<Etsaia>().GetKnockBack())
        {
            transform.Translate(Vector2.right * knockBack * Time.deltaTime);
            GetComponent<Etsaia>().KnockBackErreseteatu();
        }
    }

    // soldaduaren irudia buelta erdia ematen du, pareta/zuloa dagoelako edo jokalaria jarraitzeko
    void BueltaEman()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_stop_attack"))
        {
            eskumaBegira = !eskumaBegira;
            transform.eulerAngles = new Vector3(0, eskumaBegira ? -180 : 0, 0);
        }
    }

    // animazioko event jaurtizen du
    // eraso ostean geldi segundu batzuk
    void ErasoaBukatuta()
    {
        StartCoroutine("IdleDenbora");
    }

    // eraso ostean geldi segundu batzuk
    // jokalariarengana pixka bat mugitu !!!
    IEnumerator IdleDenbora()
    {
        abiadura = 0;
        yield return new WaitForSeconds(idleDenbora);
        abiadura = abiaduraNormala;
        if (jokalariaJarraitu && JokalariaAtzeanDago())
        {
            BueltaEman();
        }
        if (jokalariaJarraitu)
        {
            StartCoroutine("ErasoaKargatu");
        }
    }

    // eraso ostean berriro erasotzeko itzaron behar duen denbora
    IEnumerator ErasoaKargatu()
    {
        yield return new WaitForSeconds(erasoDenbora);
        if (!erasoDezaket && jokalariaJarraitu)
        {
            erasoDezaket = true;
            erasoa = Random.Range(0, 3);
        }
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

    void OinezJarri()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_move"))
        {
            if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
            {
                if (oinezSoinua != null)
                {
                    if (!oinezSoinua.isPlaying)
                    {
                        oinezSoinua.Play();
                    }
                }
            }
            else
            {
                oinezSoinua.Stop();
            }
        }
        else
        {
            oinezSoinua.Stop();
        }
        
    }

    public void MeleeSoinua()
    {
        if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
        {
            if (meleeSoinua != null)
            {
                meleeSoinua.Play();
            }
        }
    }

    public void DashSoinua()
    {
        if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
        {
            if (dashSoinua != null)
            {
                dashSoinua.Play();
            }
        }
    }

    public void SpearSoinua()
    {
        if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
        {
            if (spearSoinua != null)
            {
                spearSoinua.Play();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, begiradaErradioa);
        Gizmos.DrawWireSphere(transform.position, erasoErradioa);
    }
}
