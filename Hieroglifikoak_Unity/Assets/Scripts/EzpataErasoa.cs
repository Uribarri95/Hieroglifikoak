﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzpataErasoa : MonoBehaviour {

    private float coolDown;
    public float denboraTartea;

    public Transform erasoPuntua;
    public float erradioa;
    public int damage;
    public LayerMask etsaiak;

    JokalariMug jokalaria;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        jokalaria = FindObjectOfType<JokalariMug>();
    }
	
	// Update is called once per frame
	void Update () {
		if(coolDown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                if(!jokalaria.GetMakurtu())
                {
                    Collider2D[] kolpatutakoEtsaiak = Physics2D.OverlapCircleAll(erasoPuntua.position, erradioa, etsaiak);
                    for (int i = 0; i < kolpatutakoEtsaiak.Length; i++)
                    {
                        kolpatutakoEtsaiak[i].GetComponent<Etsaia>().KolpeaJaso(damage);
                    }
                    anim.SetBool("eraso", true);
                    coolDown = denboraTartea;
                }
            }
        }
        else
        {
            coolDown -= Time.deltaTime;
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(erasoPuntua.position, erradioa);
    }
}