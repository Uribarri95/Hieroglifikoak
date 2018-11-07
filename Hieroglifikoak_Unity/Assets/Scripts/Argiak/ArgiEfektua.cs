using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgiEfektua : MonoBehaviour {

    Vector3 tamainaHasiera;
    float shakeTime = .05f;
    float itxaroteDenbora = 2.2f;
    float offset = .05f;
    float tamaina;

    float denbora;
    float erradioa;
    bool handitu;
    bool erasotzen;
    bool txikitu;

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
                tamaina = Random.Range(.01f,.05f);
                if (handitu)
                {
                    transform.localScale = new Vector3(tamainaHasiera.x + tamaina, tamainaHasiera.y + tamaina, tamainaHasiera.z);
                }
                else
                {
                    transform.localScale = new Vector3(tamainaHasiera.x - tamaina, tamainaHasiera.y - tamaina, tamainaHasiera.z);
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
            erradioa = Mathf.Lerp(erradioa, tamainaHasiera.x, Time.deltaTime * 1.8f);
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
