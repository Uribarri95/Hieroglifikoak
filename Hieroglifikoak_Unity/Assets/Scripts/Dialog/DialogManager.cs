﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    #region Singleton
    public static DialogManager instantzia;

    private void Awake()
    {
        if (instantzia != null)
        {
            Debug.LogWarning("Error Singleton: DialogManager bat baino gehiago aurkitu da!");
            return;
        }
        instantzia = this;
    }
    #endregion

    public Text dialogBox;
    public Text botoia;
    public Animator anim;
    public GameObject dialogCanvas;
    public GameObject puzleCanvas;
    public FadeManager fadeManager;
    //public Transition trantzizioa;
    
    private Queue<string> esaldiak;
    bool puzleaJarri = false;
    int zenbakia;

    // Use this for initialization
    void Start () {
        esaldiak = new Queue<string>();
	}

    // !!! if canvasJarri -> pause -> jokalariaren kontrola kendu !!!
    public void StartDialog(Dialog dialog, bool canvasJarri, int zenb)
    {
        Pause.jokuaGeldituta = true;

        puzleaJarri = canvasJarri;
        zenbakia = zenb;

        anim.SetBool("zabalduta", true);

        esaldiak.Clear();
        foreach (var esaldia in dialog.esaldiak)
        {
            esaldiak.Enqueue(esaldia);
        }
        HurrengoEsaldiaErakutsi();
    }

    // !!! pause sakatzean ezgaitu !!!
    public void HurrengoEsaldiaErakutsi()
    {
        if (esaldiak.Count == 0)
        {
            HizketaBukatu();
            return;
        }
        if (esaldiak.Count == 1)
        {
            botoia.text = "Ados";
        }
        if (esaldiak.Count > 1)
        {
            botoia.text = "Jarraitu >";
        }
        string esaldia = esaldiak.Dequeue();
        StopAllCoroutines();
        StartCoroutine(EsaldiaIdatzi(esaldia));
        /*if (!Pause.jokuaGeldituta)
        {
            
        }*/
    }

    IEnumerator EsaldiaIdatzi(string esaldia)
    {
        dialogBox.text = "";
        foreach (char letra in esaldia)
        {
            dialogBox.text += letra;
            yield return null;
        }
    }

    // !!! canvas ezgaitu ? 
    public void HizketaBukatu()
    {
        anim.SetBool("zabalduta", false);
        if (puzleaJarri)
        {
            StartCoroutine(ErakutsiPuzzleUI());
        }
        else
        {
            if (!puzleCanvas.GetComponent<PuzleManager>().aktibatuta)
            {
                Pause.jokuaGeldituta = false;
            }
        }
        dialogCanvas.SetActive(false);
    }

    IEnumerator ErakutsiPuzzleUI()
    {
        yield return new WaitForSeconds(.5f);
        fadeManager.Ilundu();
        //trantzizioa.FadeOut();
        yield return new WaitForSeconds(1.5f);
        puzleCanvas.SetActive(true);
        puzleCanvas.GetComponent<PuzleManager>().PanelGaitu(zenbakia);
        //trantzizioa.FadeIn();
        fadeManager.Argitu();
        yield return new WaitForSeconds(1);
    }
}
