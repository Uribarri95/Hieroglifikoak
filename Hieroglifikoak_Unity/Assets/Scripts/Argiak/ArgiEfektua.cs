using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgiEfektua : MonoBehaviour {

    public float itxaroteDenbora;               // argi efektua kentzeko denbora
    public float murrizketaAbiadura;            // argi txikitzen den abiadura

    Vector3 tamainaHasiera;                     // argiaren hasierako esfera tamaina
    float erradioa;                             // erasotzean argiak hartzen duen erradioa
    float offset = .05f;                        // argia txikitzean errore margina kentzeko
    bool txikitu;                               // argia berriro ere txikitzen denean gradualki egiteko

    float shakeTime = .05f;                     // argia kliskatzen ari den efektua emateko
    float randomTamaina;                        // argia kliskatzeko tamaina ezberdinak
    float denbora;                              // argia kliskatze denbora
    bool handitu;                               // kliskatze efekturako
    bool erasotzen;                             // erasoa gertatzen denean ez dago kliskatze efekturik

    // Use this for initialization
    void Start () {
        tamainaHasiera = transform.localScale;
        denbora = 0;
        handitu = true;
        erasotzen = false;
        txikitu = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!erasotzen)
        {
            if (denbora < shakeTime)
            {
                randomTamaina = Random.Range(.01f,.05f);
                if (handitu)
                {
                    transform.localScale = new Vector3(tamainaHasiera.x + randomTamaina, tamainaHasiera.y + randomTamaina, tamainaHasiera.z);
                }
                else
                {
                    transform.localScale = new Vector3(tamainaHasiera.x - randomTamaina, tamainaHasiera.y - randomTamaina, tamainaHasiera.z);
                }
                denbora += Time.deltaTime;
            }
            else
            {
                denbora = 0;
                handitu = !handitu;
            }
        }
        if(erasotzen && txikitu)
        {
            erradioa = Mathf.Lerp(erradioa, tamainaHasiera.x, Time.deltaTime * 3f);
            transform.localScale = new Vector3(erradioa, erradioa, tamainaHasiera.z);
            if (transform.localScale.x <= tamainaHasiera.x + offset)
            {
                erasotzen = false;
            }
        }
	}

    public void ArgiErasoa(float r)
    {
        txikitu = false;
        erradioa = r;
        transform.localScale = new Vector3(erradioa, erradioa, tamainaHasiera.z);
        StartCoroutine("ErradioaTxikitu");
    }

    IEnumerator ErradioaTxikitu()
    {
        txikitu = false;
        erasotzen = true;
        yield return new WaitForSeconds(itxaroteDenbora);
        txikitu = true;
    }
}
