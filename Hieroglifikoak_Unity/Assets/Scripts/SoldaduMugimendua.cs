using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldaduMugimendua : MonoBehaviour {

    public LayerMask jokalaria;
    public LayerMask oztopoak;
    public Transform groundCheck;
    public Transform erasoPuntua;
    public Transform dashPoint;
    public GameObject dashParticle;
    public GameObject spear;

    Animator anim;

    float abiadura;
    public float abiaduraNormala;
    public float dashAbiadura;
    bool eskumaBegira;

    float zoruDistantzia = .5f;
    float hormaDistantzia = .1f;

    public float begiradaErradioa;
    public float erasoErradioa;
    public float erasoDistantzia;
    public float erasoDenbora;
    public float idleDenbora;
    Vector2 jokalariPos;
    int erasoa;
    bool erasoDezaket;
    bool jokalariaJarraitu;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

        abiadura = abiaduraNormala;

        erasoDezaket = false;
        StartCoroutine("ErasoaKargatu");
    }
	
	// Update is called once per frame
	void Update () {
        Collider2D target = Physics2D.OverlapCircle(transform.position, begiradaErradioa, jokalaria);
        if (target != null)
        {
            jokalariaJarraitu = true;
            jokalariPos = target.transform.position;
        }
        else
        {
            if (!LurraBilatu() || HormaBilatu())
            {
                StartCoroutine("IdleDenbora");
                BueltaEman();
            }
            jokalariaJarraitu = false;
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

        // dash attack eta gezia bota -> urrundu
        // melee erasoa -> gerturatu
        transform.Translate(Vector2.left * abiadura * Time.deltaTime);

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

    void DashAttack()
    {
        if(erasoDezaket)
        {
            anim.SetBool("stop_dash", false);
            erasoDezaket = false;
            abiadura = 0;
            anim.SetTrigger("dash_attack");
        }
        if ((!LurraBilatu() || HormaBilatu()) && anim.GetCurrentAnimatorStateInfo(0).IsName("anubis_soldier_attack"))
        {
            anim.SetBool("stop_dash", true);
        }
    }

    //animazioko event jaurtitzen du
    void DashEfect()
    {
        Instantiate(dashParticle, dashPoint.position, dashPoint.rotation);
    }

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
                GetComponent<MinEmanKolpatzean>().JokalariaKolpatu(kolpea.transform.GetComponent<Eraso>(), eskuma);
            }
        }
    }

    void ThrowSpear()
    {
        if (erasoDezaket)
        {
            erasoDezaket = false;
            abiadura = 0;
            anim.SetTrigger("throw_spear");
        }
    }

    // animazioko event jaurtitzen du
    void Spear()
    {
        Instantiate(spear, erasoPuntua.position, erasoPuntua.rotation);
    }

    void BueltaEman()
    {
        eskumaBegira = !eskumaBegira;
        transform.eulerAngles = new Vector3(0, eskumaBegira ? -180 : 0, 0);
    }

    // animazioko event jaurtizen du
    void ErasoaBukatuta()
    {
        StartCoroutine("IdleDenbora");
    }

    IEnumerator IdleDenbora()
    {
        abiadura = 0;
        yield return new WaitForSeconds(idleDenbora);
        if ((transform.position.x < jokalariPos.x && !eskumaBegira) || (transform.position.x > jokalariPos.x && eskumaBegira))
        {
            BueltaEman();
        }
        StartCoroutine("ErasoaKargatu");
    }

    IEnumerator ErasoaKargatu()
    {
        abiadura = abiaduraNormala;
        yield return new WaitForSeconds(erasoDenbora);
        if (!erasoDezaket && jokalariaJarraitu)
        {
            erasoa = Random.Range(0,3);
            erasoDezaket = true;
        }
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
        Gizmos.DrawWireSphere(transform.position, erasoErradioa);
    }
}
