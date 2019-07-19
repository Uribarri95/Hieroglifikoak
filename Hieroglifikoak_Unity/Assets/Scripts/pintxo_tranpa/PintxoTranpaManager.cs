using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PintxoTranpaManager : MonoBehaviour {

    PintxoTranpa[] tranpak;
    public bool BetiAktibaturik;
    public bool aktibatutaHasi;
    public float zikloDenbora;
    bool eragin;
    bool aktibatuta;
    public bool tranpaBerezia;
    public bool alderantziz;

	// Use this for initialization
	void Start () {
        tranpak = GetComponentsInChildren<PintxoTranpa>();
        aktibatuta = true;
        if (!aktibatutaHasi)
        {
            Desaktibatu();
        }
        //eragin = false;
        eragin = alderantziz;
        if (BetiAktibaturik)
        {
            StartCoroutine("ZikloKudeaketa");
        }
    }

    public void TranpakKudeatu(bool aktibatu)
    {
        for (int i = 0; i < tranpak.Length; i++)
        {
            tranpak[i].Aktibatu(aktibatu);
        }
    }

    IEnumerator ZikloKudeaketa()
    {
        StartCoroutine("PintxoSoinua");
        eragin = !eragin;
        if (tranpaBerezia)
        {
            if (eragin)
            {
                yield return new WaitForSeconds(zikloDenbora);
            }
            else
            {
                yield return new WaitForSeconds(zikloDenbora / 2);
            }
        }
        else
        {
            yield return new WaitForSeconds(zikloDenbora);
        }
        TranpakKudeatu(eragin);
        StartCoroutine("ZikloKudeaketa");
    }

    IEnumerator PintxoSoinua()
    {
        yield return new WaitForSeconds(1.3f);
        if (GetComponentInChildren<SoinuGunea>() != null && !eragin)
        {
            if (GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
            {
                AudioSource audioa = GetComponent<AudioSource>();
                if (audioa != null)
                {
                    audioa.Play();
                }
            }   
        }
        yield return new WaitForSeconds(zikloDenbora - 1.3f);
    }

    public void Aktibatu()
    {
        if (!aktibatuta)
        {
            for (int i = 0; i < tranpak.Length; i++)
            {
                tranpak[i].Piztu();
            }
            aktibatuta = true;
            //eragin = true;
            TranpakKudeatu(eragin);
            StopAllCoroutines();
            StartCoroutine("ZikloKudeaketa");
            
        }
    }

    public void Desaktibatu()
    {
        aktibatuta = false;
        for (int i = 0; i < tranpak.Length; i++)
        {
            tranpak[i].Itzali();
        }
    }
}
