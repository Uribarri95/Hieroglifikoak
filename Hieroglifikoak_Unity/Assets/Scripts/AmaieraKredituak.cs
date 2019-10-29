using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmaieraKredituak : MonoBehaviour {

    public FadeManager FadeManager;

    public Text txanponak;
    public Text bihotzak;
    public Text heriotzak;

    Data.PlayerData jokalariDatuak;

    public Animator anim;

	// Use this for initialization
	void Start () {
        Data playerData = Data.instantzia;
        jokalariDatuak = playerData.JokalariDatuakKargatu();

        txanponak.text = jokalariDatuak.lortutakoTxanponGuztiak.ToString();
        bihotzak.text = (jokalariDatuak.bizitzaPuntuMax/2).ToString();
        heriotzak.text = jokalariDatuak.jokalariaHilZenbaketa.ToString();

        StartCoroutine(FadeManager.BukaeraFadeIn(.5f));
    }

    public void HasieraEszenatokia()
    {
        FadeManager.FadeToScene(0);
    }
}
