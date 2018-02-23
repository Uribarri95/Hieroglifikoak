using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class MugKudeatzaile : MonoBehaviour {

    public LayerMask kolpeGainazalak;

    public KolpeInfo kolpeak;

    public int izpiHorKop = 4;
    public int izpiBertKop = 4;

    const float azalZabalera = .015f;

    float horIzpiTartea;
    float bertIzpiTartea;

    BoxCollider2D bc2d;
    IzpiJatorria izpiJatorria;

	// Use this for initialization
	void Start () {
        bc2d = GetComponent<BoxCollider2D> ();
        IzpTarteakKalkulatu();
    }
	
	// Update is called once per frame
	//void Update () {
    //    
	//}

    public void Mugitu(Vector3 abiadura)
    {
        IzpiJatorriaEguneratu();
        kolpeak.Reset();

        if (abiadura.x != 0)
        {
            KolpeHorizontalak(ref abiadura);
        }
        if (abiadura.y != 0)
        {
            KolpeBertikalak(ref abiadura);
        }

        transform.Translate(abiadura);
    }

    void KolpeHorizontalak(ref Vector3 abiadura)
    {
        float xNoranzkoa = Mathf.Sign(abiadura.x);
        float izpiLuzera = Mathf.Abs(abiadura.x) + azalZabalera;

        for (int i = 0; i < izpiHorKop; i++)
        {
            Vector2 jatorriIzpia = (xNoranzkoa == -1) ? izpiJatorria.bottomLeft : izpiJatorria.bottomRight;
            jatorriIzpia += Vector2.up * (horIzpiTartea * i);
            RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.right * xNoranzkoa, izpiLuzera, kolpeGainazalak);

            Debug.DrawRay(jatorriIzpia, Vector2.right * xNoranzkoa * izpiLuzera, Color.red);

            if (kolpatu)
            {
                abiadura.x = (kolpatu.distance - azalZabalera) * xNoranzkoa;
                izpiLuzera = kolpatu.distance;

                kolpeak.ezkerra = xNoranzkoa == -1;
                kolpeak.eskuma = xNoranzkoa == 1;
            }
        }
    }

    void KolpeBertikalak(ref Vector3 abiadura)
    {
        float yNoranzkoa = Mathf.Sign(abiadura.y);
        float izpiLuzera = Mathf.Abs(abiadura.y) + azalZabalera;

        for (int i = 0; i < izpiBertKop; i++)
        {
            Vector2 jatorriIzpia = (yNoranzkoa == -1) ? izpiJatorria.bottomLeft : izpiJatorria.topLeft;
            jatorriIzpia += Vector2.right * (bertIzpiTartea * i + abiadura.x);
            RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.up * yNoranzkoa, izpiLuzera, kolpeGainazalak);

            Debug.DrawRay(jatorriIzpia, Vector2.up * yNoranzkoa * izpiLuzera, Color.red);

            if (kolpatu)
            {
                abiadura.y = (kolpatu.distance - azalZabalera) * yNoranzkoa;
                izpiLuzera = kolpatu.distance;

                kolpeak.gainean = yNoranzkoa == 1;
                kolpeak.azpian = yNoranzkoa == -1;
            }
        }
    }

    struct IzpiJatorria
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    void IzpiJatorriaEguneratu()
    {
        //bi lerro hauek behean ere errepikatzen dira, startera mugitu?
        Bounds mugak = bc2d.bounds;
        mugak.Expand(azalZabalera * -2);

        izpiJatorria.bottomLeft = new Vector2(mugak.min.x, mugak.min.y);
        izpiJatorria.bottomRight = new Vector2(mugak.max.x, mugak.min.y);
        izpiJatorria.topLeft = new Vector2(mugak.min.x, mugak.max.y);
        izpiJatorria.topRight = new Vector2(mugak.max.x, mugak.max.y);
    }

    void IzpTarteakKalkulatu()
    {
        Bounds mugak = bc2d.bounds;
        mugak.Expand(azalZabalera * -2);

        izpiHorKop = Mathf.Clamp(izpiHorKop, 2, int.MaxValue);
        izpiBertKop = Mathf.Clamp(izpiBertKop, 2, int.MaxValue);

        horIzpiTartea = mugak.size.y / (izpiHorKop - 1);
        bertIzpiTartea = mugak.size.x / (izpiBertKop - 1);
    }

    public struct KolpeInfo
    {
        public bool gainean, azpian;
        public bool eskuma, ezkerra;

        public void Reset()
        {
            gainean = false;
            azpian = false;
            eskuma = false;
            ezkerra = false;
        }
    }
}
