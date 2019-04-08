using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxanponakUI : MonoBehaviour {

    public bool gaitu = false;

    Inbentarioa inbentario;
    Text zenbakia;
    Image irudia;
    bool gaituta = false;

	// Use this for initialization
	void Start () {
        inbentario = Inbentarioa.instantzia;
        zenbakia = GetComponentInChildren<Text>();
        irudia = GetComponentInChildren<Image>();
        inbentario.itemJasoDeitu += AddTxanponak;
        inbentario.UIEguneratu();
    }

    private void Update()
    {
        if (gaitu)
        {
            gaitu = false;
            UIGaitu();
        }
    }

    void AddTxanponak()
    {
        //if (!gaituta && inbentario.txanponKopurua != 0)
        if (!gaituta && inbentario.GetTxanponKopurua() != 0)
        {
            irudia.enabled = true;
            zenbakia.enabled = true;
            gaituta = true;
        }
        //zenbakia.text = inbentario.txanponKopurua.ToString();
        zenbakia.text = inbentario.GetTxanponKopurua().ToString();
    }

    public void UIGaitu()
    {
        irudia.enabled = true;
        zenbakia.enabled = true;
    }
}
