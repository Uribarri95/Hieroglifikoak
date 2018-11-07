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

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        yOffset = bc2d.offset.y;
    }
	
	// Update is called once per frame
	void Update () {
        if (anim.GetBool("Aktibatuta") && bc2d.offset.y != end)
        {
            dest = end;
            yOffset = Mathf.Lerp(yOffset, dest, Time.deltaTime * s1);
            bc2d.offset = new Vector2(0, yOffset);
        }
        if (!anim.GetBool("Aktibatuta") && bc2d.offset.y != init)
        {
            dest = init;
            yOffset = Mathf.Lerp(yOffset, dest, Time.deltaTime * s2);
            bc2d.offset = new Vector2(0, yOffset);
        }
    }

    public void Aktibatu(bool eragin)
    {
        anim.SetBool("Aktibatuta", eragin);
    }
}
