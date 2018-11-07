using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranpaManager : MonoBehaviour {

    PintxoTranpa[] tranpak;

	// Use this for initialization
	void Start () {
        tranpak = GetComponentsInChildren<PintxoTranpa>();
    }

    public void TranpakKudeatu(bool aktibatu)
    {
        for (int i = 0; i < tranpak.Length; i++)
        {
            tranpak[i].Aktibatu(aktibatu);
        }
    }
}
