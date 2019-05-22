using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PintxoTranpa : MonoBehaviour {

    Animator anim;
    BoxCollider2D bc2d;
    float s1 = 4;
    float s2 = 2;    
    float yOffset;

    float dest;
    float init = -.8f;
    float end = .3f;

    bool reset = false;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        yOffset = bc2d.offset.y;
    }
	
	// Update is called once per frame
	void Update () {
        if (!reset)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Trap_action") && bc2d.offset.y != end)
            {
                PintxoakAtera();
            }
            if (!anim.GetBool("Aktibatuta") && bc2d.offset.y != init)
            {
                PintxoakSartu();
            }
        }
    }

    void PintxoakAtera()
    {
        dest = end;
        yOffset = Mathf.Lerp(yOffset, dest, Time.deltaTime * s1);
        bc2d.offset = new Vector2(0, yOffset);
    }

    void PintxoakSartu()
    {
        dest = init;
        yOffset = Mathf.Lerp(yOffset, dest, Time.deltaTime * s2);
        bc2d.offset = new Vector2(0, yOffset);
    }

    public void Aktibatu(bool eragin)
    {
        if (!reset)
        {
            anim.SetBool("Aktibatuta", eragin);
        }
    }

    public void Itzali()
    {
        reset = true;
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }
        anim.SetBool("Aktibatuta", false);
        anim.SetTrigger("reset");
        bc2d.offset = new Vector2(0, init);
    }

    public void Piztu()
    {
        reset = false;
    }
}
