﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zapaldu : MonoBehaviour {

    public float abiaduraAtera;
    public float abiaduraSartu;
    public float zikloDenbora;
    public float blokeKop;
    public bool goianHasi;
    public bool reseta;
    public bool aktibatu;

    float offset = .1f;
    float hasieraPos;
    float bukaeraPos;
    bool ateratzen;
    bool gelditu;

	// Use this for initialization
	void Start () {
        gelditu = false;
        if (goianHasi)
        {
            hasieraPos = transform.position.y;
            bukaeraPos = hasieraPos - blokeKop;
            ateratzen = true;
        }
        else
        {
            bukaeraPos = transform.position.y;
            hasieraPos = bukaeraPos + blokeKop;
            ateratzen = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!gelditu)
        {
            if (ateratzen)
            {
                if (transform.position.y >= bukaeraPos + offset)
                {
                    Vector3 pos = transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, bukaeraPos), abiaduraAtera * Time.deltaTime);
                }
                else
                {
                    gelditu = true;
                    StartCoroutine("Itxaron");
                    ateratzen = false;
                }
            }
            else
            {
                if (transform.position.y <= hasieraPos - offset)
                {
                    Vector3 pos = transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, hasieraPos), abiaduraSartu * Time.deltaTime);
                }
                else
                {
                    gelditu = true;
                    StartCoroutine("Itxaron");
                    ateratzen = true;
                }
            }
        }
        
        if (reseta)
        {
            reseta = false;
            Erreseteatu();
        }
        if (aktibatu)
        {
            aktibatu = false;
            Aktibatu();
        }
	}

    IEnumerator Itxaron()
    {
        yield return new WaitForSeconds(zikloDenbora);
        if (!reseta)
        {
            Aktibatu();
        }
    }

    public void Aktibatu()
    {
        gelditu = false;
    }

    public void Erreseteatu()
    {
        gelditu = true;
        if (goianHasi)
        {
            ateratzen = true;
            transform.position = new Vector2(transform.position.x, hasieraPos);
        }
        else
        {
            ateratzen = false;
            transform.position = new Vector2(transform.position.x, bukaeraPos);
        }
    }
}