using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gorputza_fisikak : MonoBehaviour {

    public float grabitateAldaketa = 1f;
    public float normalMinY = .065f;

    protected bool lurrean;
    protected Vector2 zoruNormala;
    protected Vector2 abiadura;
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter; //zein gorputz mota kontuan hartzen da
    protected RaycastHit2D[] kolpekatu = new RaycastHit2D[16];
    protected List<RaycastHit2D> kolpeZerrenda = new List<RaycastHit2D>(16);

    protected const float mugimenduDistMin = 0.001f;
    protected const float azalErradioa = 0.01f;

    protected Vector2 targetVelocity;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        abiadura += grabitateAldaketa * Physics2D.gravity * Time.deltaTime;

        /**//**/abiadura.x = targetVelocity.x;

        lurrean = false;
        Vector2 deltaPos = abiadura * Time.deltaTime;

        /***//**/Vector2 moveAlongGround = new Vector2(zoruNormala.y, -zoruNormala.x);
        /****/Vector2 mugimendua = moveAlongGround * deltaPos.x;

        /**/
        Mugitu(mugimendua, false);

        /*Vector2 */mugimendua = Vector2.up * deltaPos.y;
        Mugitu(mugimendua, true);
    }

    void Mugitu(Vector2 mugimendua, bool yMovement)
    {
        float distantzia = mugimendua.magnitude;
        if (distantzia > mugimenduDistMin)
        {
            int kont = rb2d.Cast(mugimendua,contactFilter, kolpekatu, distantzia + azalErradioa);
            kolpeZerrenda.Clear();
            for (int i=0; i<kont; i++)
            {
                kolpeZerrenda.Add(kolpekatu[i]);
            }
            for(int i=0; i<kolpeZerrenda.Count; i++)
            {
                Vector2 normala = kolpeZerrenda[i].normal;
                if (normala.y > normalMinY)
                {
                    lurrean = true;
                    if (yMovement)
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
                float distantziaAldaketa = kolpeZerrenda[i].distance - azalErradioa;
                distantzia = distantziaAldaketa < distantzia ? distantziaAldaketa : distantzia;
            }
        } 
        rb2d.position = rb2d.position + mugimendua.normalized * distantzia;
    }
}
