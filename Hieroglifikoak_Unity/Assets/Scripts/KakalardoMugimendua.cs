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
        Instantiate(bola, erasoPuntua.position, erasoPuntua.rotation);
    }
}
