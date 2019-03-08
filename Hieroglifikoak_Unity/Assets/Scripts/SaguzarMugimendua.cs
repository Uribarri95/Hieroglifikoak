using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaguzarMugimendua : MonoBehaviour {

    public LayerMask jokalaria;

    public float aktibazioErradioa = 4.5f;
    private float erasoDistantzia = 2.5f;
    private float jarraituDistantziaMax = 8;
    private float abiaduraNormala = 1.8f;
    private float erasoAbiadura = 3.6f;
    private float erasoDenbora = .2f;
    private float coolDownDenbora = 3;

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
        jauziYpos = transform.position.y - .8f;
	}
	
	// Update is called once per frame
	void Update () {
        erradioa = aktibatuta ? jarraituDistantziaMax : aktibazioErradioa;
        
        Collider2D target = Physics2D.OverlapCircle(transform.position, erradioa, jokalaria);
        if(target != null)
        {
            SaguzarraMugitu(target.transform.position);

            // flip script
            if (target.transform.position.x > transform.position.x && !sprite.flipX)
            {
                //face right
                sprite.flipX = true;
            }
            else if (target.transform.position.x < transform.position.x && sprite.flipX)
            {
                //face left
                sprite.flipX = false;
            }
        }
    }

    void SaguzarraMugitu(Vector2 targetPos)
    {
        if (zintzilik)
        {
            aktibatuta = true;
            bool eskuma = targetPos.x >= transform.position.x;
            SaguzarraJeisti(eskuma);
        }
        else
        {
            JokalariaJarraitu(targetPos);
        }
    }

    void SaguzarraJeisti(bool eskuma)
    {
        anim.SetBool("esnaturik", true);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("bat_getDown"))
        {
            if (transform.position.y >= jauziYpos)
            {
                // saguzarraJeitsiAnimazioa
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
            else
            {
                // saguzarMugimenduAnimazioa
                zintzilik = false;
                anim.SetBool("zintzilik", false);
                StartCoroutine("CoolDown");
            }
        }
    }

    void JokalariaJarraitu(Vector2 targetPos)
    {
        if (Vector2.Distance(transform.position, targetPos) <= erasoDistantzia && !erasotzen && erasoDezaket)
        {
            StartCoroutine("SaguzarErasoa");
            erasoPos = targetPos;
        }
        if (erasotzen)
        {
            transform.position = Vector2.MoveTowards(transform.position, erasoPos, abiadura * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, abiadura * Time.deltaTime);
        }
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

    IEnumerator SaguzarErasoa()
    {
        anim.speed = 2;
        erasoDezaket = false;
        erasotzen = true;
        abiadura = .4f;
        yield return new WaitForSeconds(erasoDenbora);
        abiadura = erasoAbiadura;
    }

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
