using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaguzarMugimendua : MonoBehaviour {

    public LayerMask jokalaria;                                 // jokalaria etiketa duen objektua jarraituko du saguzarrak

    private float aktibazioErradioa = 4.5f;                     // saguzarra jokalaria jarraitzeko egon behar den distantzia
    private float erasoDistantzia = 2.5f;                       // saguzarra jokalaria erasotzeko distantzia
    private float jarraituDistantziaMax = 8;                    // saguzarra jokalaria jarraitzen uzteko distantzia
    private float abiaduraNormala = 1.8f;                       // saguzarraren mugimendu abiadura
    private float erasoAbiadura = 3.6f;                         // saguzarra erasotzen dagoenean duen abiadura
    private float erasoDenbora = .2f;                           // eraso aurretik geldi dagoen denbora, jokalaria erasoko dela adierazten du
    private float coolDownDenbora = 3;                          // eraso batetik bestera itxaron behar duen denbora
    private float jauziDistantzia = .8f;                        // saguzarra zorutik eskegitzen da. animazioa aldatu egiten da distantzia hau pasatu ondoren

    Animator anim;
    SpriteRenderer sprite;

    float erradioa;
    float abiadura;
    bool aktibatuta;
    bool zintzilik;
    bool erasotzen;
    bool erasoDezaket;
    Vector2 erasoPos;
    float jauziYpos;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        abiadura = abiaduraNormala;
        aktibatuta = false;
        zintzilik = true;
        erasotzen = false;
        erasoDezaket = false;
        jauziYpos = transform.position.y - jauziDistantzia;
	}
	
	// Update is called once per frame
	void Update () {
        erradioa = aktibatuta ? jarraituDistantziaMax : aktibazioErradioa;
        
        Collider2D target = Physics2D.OverlapCircle(transform.position, erradioa, jokalaria);
        if(target != null)
        {
            SaguzarraMugitu(target.transform.position);

            // sprite noranzkoa aldatu
            if (target.transform.position.x > transform.position.x && !sprite.flipX)
            {
                //eskumara
                sprite.flipX = true;
            }
            else if (target.transform.position.x < transform.position.x && sprite.flipX)
            {
                //ezkerrera
                sprite.flipX = false;
            }
        }
    }

    void SaguzarraMugitu(Vector2 targetPos)
    {
        // zorutik eskegitu
        if (zintzilik)
        {
            aktibatuta = true;
            bool eskuma = targetPos.x >= transform.position.x;
            SaguzarraJeisti(eskuma);
        }
        // jokalaria jarriatu
        else
        {
            JokalariaJarraitu(targetPos);
        }
    }

    // saguzarra zorutik eskegitu .8 unitate distantzia jokalariarengana gerturatuz. gero jokalaria jarraituko du
    void SaguzarraJeisti(bool eskuma)
    {
        anim.SetBool("esnaturik", true);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("bat_getDown"))
        {
            // saguzarraJeitsiAnimazioa
            if (transform.position.y >= jauziYpos)
            {
                transform.Translate(Vector2.down * abiadura * Time.deltaTime);
                if (eskuma)
                {
                    transform.Translate(Vector2.right * .6f * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector2.left * .6f * Time.deltaTime);
                }
            }
            // saguzarraJeitsiAnimazioa bukatu eta saguzarMugimenduAnimazioa hasi
            else
            {
                zintzilik = false;
                anim.SetBool("zintzilik", false);
                StartCoroutine("CoolDown");
            }
        }
    }

    // saguzarra jokalaria jarraitzen du
    void JokalariaJarraitu(Vector2 targetPos)
    {
        // saguzar erasoa. jokalariaren posizioa gordetzen da, posizio horretara joango jokalaria jarraitu beharrean
        if (Vector2.Distance(transform.position, targetPos) <= erasoDistantzia && !erasotzen && erasoDezaket)
        {
            StartCoroutine("SaguzarErasoa");
            erasoPos = targetPos;
        }
        // jokalaria zegoen tokira joango da abiadura azkarrean
        if (erasotzen)
        {
            transform.position = Vector2.MoveTowards(transform.position, erasoPos, abiadura * Time.deltaTime);
        }
        // jokalariaren posizio zehatza jarraitzen du
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, abiadura * Time.deltaTime);
        }
        // eraso egoera kentzen da jokalaria zegoen tokira heltzen denean
        if(Vector2.Distance(transform.position, erasoPos) <= .1f && erasotzen)
        {
            erasotzen = false;
            anim.speed = 1;
            abiadura = abiaduraNormala;
            if (!erasoDezaket)
            {
                StartCoroutine("CoolDown");
            }
        }
    }

    // abiadura areagotzen da, ezin da berriz eraso eta eraso aurretik denbora txiki bat geldi dago
    IEnumerator SaguzarErasoa()
    {
        anim.speed = 2;
        erasoDezaket = false;
        erasotzen = true;
        abiadura = .4f;
        yield return new WaitForSeconds(erasoDenbora);
        abiadura = erasoAbiadura;
    }

    // bi eraso artean itxaron beharreko denbora
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDownDenbora);
        erasoDezaket = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aktibazioErradioa);
        Gizmos.DrawWireSphere(transform.position, jarraituDistantziaMax);
    }
}
