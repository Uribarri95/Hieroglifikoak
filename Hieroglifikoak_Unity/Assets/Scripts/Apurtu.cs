using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apurtu : MonoBehaviour {

    public float denbora;
    public bool apurtu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (apurtu)
        {
            Suntsitu();
        }
	}

    public void Suntsitu()
    {
        Animator[] anim = GetComponentsInChildren<Animator>();
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger("apurtu");
        }
        StartCoroutine("Desagertu");
    }

    IEnumerator Desagertu()
    {
        yield return new WaitForSeconds(denbora);
        GetComponent<ItemaAgerrarazi>().ItemAtera();
        Destroy(gameObject);
    }
}
