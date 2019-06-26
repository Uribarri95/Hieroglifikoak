using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErreDaiteke : MonoBehaviour {

    Animator anim;
    SpriteMask argia;
    public bool piztuta = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        argia = GetComponentInChildren<SpriteMask>();
        argia.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Piztu()
    {
        piztuta = true;
        anim.SetBool("suaPiztuta", true);
        argia.enabled = true;
    }

    public bool GetPiztuta()
    {
        return piztuta;
    }
}
