using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HormaMugimendua : MonoBehaviour {

    // ezkerretik eskumara luzea --> bukaerara heltzean geldi
    // hiltzean reseta, berpiztean aktibatu

    public float abiaduraAtera;
    public float abiaduraSartu;
    public float zikloDenbora;
    public float blokeKop;
    public bool eskuman;
    public bool ezkerrean;
    public bool reseta;
    public bool aktibatu;
    public bool ziklikoa;

    float offset = .1f;
    float hasieraPos;
    float bukaeraPos;
    bool ateratzen;
    bool gelditu;

    // Use this for initialization
    void Start()
    {
        gelditu = false;
        if (eskuman)
        {
            hasieraPos = transform.position.x;
            bukaeraPos = hasieraPos - blokeKop;
            ateratzen = true;
        }
        else
        {
            bukaeraPos = transform.position.x;
            hasieraPos = bukaeraPos + blokeKop;
            ateratzen = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gelditu)
        {
            if (ateratzen)
            {
                if (transform.position.x >= bukaeraPos + offset)
                {
                    Vector3 pos = transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(bukaeraPos, transform.position.y), (ezkerrean ? abiaduraSartu : abiaduraAtera) * Time.deltaTime);
                }
                else
                {
                    gelditu = true;
                    if (ziklikoa)
                    {
                        StartCoroutine("Itxaron");
                        ateratzen = false;
                    }
                }
            }
            else
            {
                if (transform.position.x <= hasieraPos - offset)
                {
                    Vector3 pos = transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(hasieraPos, transform.position.y), (ezkerrean ? abiaduraAtera : abiaduraSartu) * Time.deltaTime);
                }
                else
                {
                    gelditu = true;
                    if (ziklikoa)
                    {
                        StartCoroutine("Itxaron");
                        ateratzen = true;
                    }
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

    public void TranpakKudeatu(bool aktibatu)
    {
        if (aktibatu)
        {
            Aktibatu();
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
        gelditu = true;
        if (eskuman)
        {
            ateratzen = true;
            transform.position = new Vector2(hasieraPos, transform.position.y);
        }
        else
        {
            ateratzen = false;
            transform.position = new Vector2(bukaeraPos, transform.position.y);
        }
    }
}
