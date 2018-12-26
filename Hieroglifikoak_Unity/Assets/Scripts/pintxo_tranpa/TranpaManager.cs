using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranpaManager : MonoBehaviour {

    PintxoTranpa[] tranpak;
    public bool ziklikoa;
    public float zikloDenbora;
    bool aktibatuta;

	// Use this for initialization
	void Start () {
        tranpak = GetComponentsInChildren<PintxoTranpa>();
        aktibatuta = false;
        if (ziklikoa)
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
        aktibatuta = !aktibatuta;
        yield return new WaitForSeconds(zikloDenbora);
        TranpakKudeatu(aktibatuta);
        StartCoroutine("ZikloKudeaketa");
    }
}
