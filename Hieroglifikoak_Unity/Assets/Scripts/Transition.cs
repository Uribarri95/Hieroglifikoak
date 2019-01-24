using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FadeOut()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("fadeOut");
    }

    public bool FadeIn()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.SetTrigger("fadeIn");
            return true;
        }
        return false;
    }
}
