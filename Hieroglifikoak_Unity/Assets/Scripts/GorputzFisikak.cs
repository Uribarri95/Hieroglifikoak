using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorputzFisikak : MonoBehaviour {

    public float grabitateAldaketa = 1f;
    //zutunik egon ahal den ala ez erabakitzeko (angeluaren arabera) !!! kontuz !!! maldaren arabera gorputza lurrean edo airean egongo da naiz eta 'zutik' egon
    public float normalMinY = .065f;

    protected Vector2 helmugaAbiadura;
    protected bool lurrean;
    protected Vector2 zoruNormala;
    protected Rigidbody2D rb2d;
    protected Vector2 abiadura;
    //protected ContactFilter2D contactFilter; //zein gorputz mota kontuan hartzen da
    //gorputzaren mugimenduaren noranzkoan 16 izpi
    protected RaycastHit2D[] kolpekatu = new RaycastHit2D[16];
    protected List<RaycastHit2D> kolpeZerrenda = new List<RaycastHit2D>(16);

    //gorputza etengabe talkak kalkulatu ez ditzan
    protected const float mugimenduDistMin = 0.001f;
    //gorputz bat beste baten barruan sartu ez daiten
    protected const float babesDistantzia = 0.01f;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        //contactFilter.useTriggers = false;
        //contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        //contactFilter.useLayerMask = true;
        //gorputza airean hasten bada eta alborantz mugitu nahi du
        //zoruNormala = new Vector2(0f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
        helmugaAbiadura = Vector2.zero;
        abiaduraKalkulatu();
        Debug.Log(lurrean);
	}

    protected virtual void abiaduraKalkulatu()
    {

    }

    void FixedUpdate()
    {
        abiadura += grabitateAldaketa * Physics2D.gravity * Time.deltaTime;
        abiadura.x = helmugaAbiadura.x;
        lurrean = false;
        Vector2 deltaPos = abiadura * Time.deltaTime;
        Vector2 lurreanMugitu = new Vector2(zoruNormala.y, -zoruNormala.x);
        Vector2 mugimendua = lurreanMugitu * deltaPos.x;
        Mugitu(mugimendua, false);
        mugimendua = Vector2.up * deltaPos.y;
        Mugitu(mugimendua, true);
    }

    void Mugitu(Vector2 mugimendua, bool mugimenduBertikala)
    {
        float distantzia = mugimendua.magnitude;
        if (distantzia > mugimenduDistMin)
        {
            //gorputzaren talka kopurua eta informazioa kolpekatu arrayaren barruan
            int kont = rb2d.Cast(mugimendua,/*contactFilter,*/ kolpekatu, distantzia + babesDistantzia);
            kolpeZerrenda.Clear();
            for (int i=0; i<kont; i++)
            {
                //zerrenda batera pasatu behar da, bestela ez du funtzionatzen
                kolpeZerrenda.Add(kolpekatu[i]);
            }
            for(int i=0; i<kolpeZerrenda.Count; i++)
            {
                Vector2 normala = kolpeZerrenda[i].normal;
                //Debug.Log(normala);
                //horma zoru bezala kontuan har ez dezan
                if (normala.y > normalMinY)
                {
                    lurrean = true;
                    if (mugimenduBertikala)
                    {
                        zoruNormala = normala;
                        normala.x = 0;
                    }
                }
                float proiekzioa = Vector2.Dot(abiadura, normala);
                if (proiekzioa < 0)
                {
                    abiadura = abiadura - proiekzioa * normala;
                }
                float distantziaAldaketa = kolpeZerrenda[i].distance - babesDistantzia;
                distantzia = distantziaAldaketa < distantzia ? distantziaAldaketa : distantzia;
            }
        }
        rb2d.position = rb2d.position + mugimendua.normalized * distantzia;
    }
}
