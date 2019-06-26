using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!transform.GetChild(0).gameObject.activeSelf)
        {
            GetComponentInParent<Animator>().SetBool("ItemHartu", true);
        }
	}
}
