using System.Collections;
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
    public bool itxaron;                                        // segituan mugitzen edo itxaroten den
    public float offsetDenbora;                                 // hasi aurretik zenbat denbora itzaronten den

    float offset = .01f;
    float hasieraPos;
    float bukaeraPos;
    bool ateratzen;
    bool gelditu = true;

    AudioSource audioa;
    public bool soinuJarraia;
    public float soinuBlokea;
    bool soinuaJarri = true;

	// Use this for initialization
	void Start () {
        audioa = GetComponent<AudioSource>();
        if (audioa == null)
        {
            Debug.Log("Audio pista jartzea ahaztu zaizu");
        }

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
        if (itxaron)
        {
            StartCoroutine(HasieraAtzeratu());
        }
        else
        {
            gelditu = false;
        }
    }

    IEnumerator HasieraAtzeratu()
    {
        yield return new WaitForSeconds(offsetDenbora);
        gelditu = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (reseta)
        {
            reseta = false;
            Erreseteatu();
        }
        if (aktibatu)
        {
            aktibatu = false;
            StartCoroutine(HasieraAtzeratu());
        }

        if (!gelditu)
        {
            if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
            {
                if (soinuJarraia)
                {
                    if (!audioa.isPlaying)
                    {
                        audioa.Play();
                    }
                }
            }
            else
            {
                audioa.Stop();
            }

            if (ateratzen)
            {
                if(GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
                {
                    if (!soinuJarraia)
                    {
                        if (transform.position.y < bukaeraPos + soinuBlokea)
                        {
                            if (soinuaJarri)
                            {
                                soinuaJarri = false;
                                audioa.Play();
                            }
                        }
                    }
                }

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
                    soinuaJarri = true;
                }
            }
        }
        else
        {
            if (soinuJarraia)
            {
                audioa.Stop();
            }
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

    public void TranpakKudeatu(bool aktibatu)
    {
        if (aktibatu)
        {
            StartCoroutine(HasieraAtzeratu());
        }
        else
        {
            Erreseteatu();
        }
    }

    public void Aktibatu()
    {
        gelditu = false;
    }

    public void Erreseteatu()
    {
        StopAllCoroutines();
        soinuaJarri = true;
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
