using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErreDaiteke : MonoBehaviour {

    Animator anim;
    bool piztuta = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Piztu()
    {
        piztuta = true;
        anim.SetBool("suaPiztuta", true);
    }

    public bool GetPiztuta()
    {
        return piztuta;
    }
}
