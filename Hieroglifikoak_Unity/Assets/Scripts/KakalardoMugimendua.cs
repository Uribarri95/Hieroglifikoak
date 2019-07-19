using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KakalardoMugimendua : MonoBehaviour {

    public GameObject bola;
    public Transform erasoPuntua;
    public float erasoMaiztasuna;
    float knockBack = 15;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        StartCoroutine("Eraso");
    }
	
	// Update is called once per frame
	void Update () {
        KolpeaJaso();
	}

    public void KolpeaJaso()
    {
        if (GetComponent<Etsaia>().GetKnockBack())
        {
            transform.Translate(Vector2.right * knockBack * Time.deltaTime);
            GetComponent<Etsaia>().KnockBackErreseteatu();
        }
    }

    IEnumerator Eraso()
    {
        yield return new WaitForSeconds(erasoMaiztasuna);
        anim.SetTrigger("eraso");
        StartCoroutine("Eraso");
    }

    // animazioko event jaurtitzen du
    void BolaJaurti()
    {
        GameObject kakaBola = Instantiate(bola, erasoPuntua.position, erasoPuntua.rotation);
        kakaBola.transform.SetParent(transform);
    }

    public void SoinuaJarri()
    {
        if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
        {
            AudioSource soinua = GetComponent<AudioSource>();
            if (soinua != null)
            {
                if (!soinua.isPlaying)
                {
                    soinua.Play();
                }
            }
        }
    }

    public void SoinuaKendu()
    {
        if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
        {
            AudioSource soinua = GetComponent<AudioSource>();
            if (soinua != null)
            {
                soinua.Stop();
            }
        }
    }
}
