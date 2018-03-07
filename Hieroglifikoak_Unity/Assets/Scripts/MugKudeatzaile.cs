using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class MugKudeatzaile : MonoBehaviour {

    //kolpeGainazalak geruzan dauden gorputzekin bakarrik egingo dugu talka
    public LayerMask kolpeGainazalak;

    public KolpeInfo kolpeak;

    public int izpiHorKop = 4;
    public int izpiBertKop = 4;
    public float aldapaAngeluMax = 45;

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

    //momentu oro exekutatzen da. Erabiltzailearen aginduak jaso eta fisika indarrak aplikatzen zaizkio
    public void Mugitu(Vector2 abiadura)
    {
        IzpiJatorriaEguneratu();
        kolpeak.Reset();

        if(abiadura.y < 0)
        {
            AldapaJaitsi(ref abiadura);
        }
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

    // Mugimendu horizontala jaso. Beste gorputz baten aurka talka egingo bada abiadura eraldatzen da. Kolpea eskuman edo ezkerran jaso den gordetzen da.
    void KolpeHorizontalak(ref Vector2 abiadura)
    {
        float xNoranzkoa = Mathf.Sign(abiadura.x);
        float izpiLuzera = Mathf.Abs(abiadura.x) + azalZabalera;

        for (int i = 0; i < izpiHorKop; i++)
        {
            Vector2 jatorriIzpia = (xNoranzkoa == -1) ? izpiJatorria.bottomLeft : izpiJatorria.bottomRight;
            jatorriIzpia += Vector2.up * (horIzpiTartea * i);
            RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.right * xNoranzkoa, izpiLuzera, kolpeGainazalak);

            Debug.DrawRay(jatorriIzpia, Vector2.right * xNoranzkoa, Color.red);

            if (kolpatu)
            {
                float aldapaAngelua = Vector2.Angle(kolpatu.normal, Vector2.up);
                if (i == 0 && aldapaAngelua <= aldapaAngeluMax)
                {
                    AldapaIgo(ref abiadura, aldapaAngelua);
                }

                if(!kolpeak.aldapaIgotzen || aldapaAngelua > aldapaAngeluMax)
                {
                    abiadura.x = (kolpatu.distance - azalZabalera) * xNoranzkoa;
                    izpiLuzera = kolpatu.distance;

                    if (kolpeak.aldapaIgotzen)
                    {
                        abiadura.y = Mathf.Tan(kolpeak.aldapaAngelu * Mathf.Deg2Rad) * Mathf.Abs(abiadura.x);
                    }

                    kolpeak.ezkerra = xNoranzkoa == -1;
                    kolpeak.eskuma = xNoranzkoa == 1;
                }
                
                
            }
        }
    }

    // Mugimendu bertikala jaso. Beste gorputz baten aurka talka egingo bada abiadura eraldatzen da. Kolpea goian edo azpian jaso den gordetzen da.
    void KolpeBertikalak(ref Vector2 abiadura)
    {
        float yNoranzkoa = Mathf.Sign(abiadura.y);
        float izpiLuzera = Mathf.Abs(abiadura.y) + azalZabalera;

        for (int i = 0; i < izpiBertKop; i++)
        {
            Vector2 jatorriIzpia = (yNoranzkoa == -1) ? izpiJatorria.bottomLeft : izpiJatorria.topLeft;
            jatorriIzpia += Vector2.right * (bertIzpiTartea * i + abiadura.x);
            RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.up * yNoranzkoa, izpiLuzera, kolpeGainazalak);

            Debug.DrawRay(jatorriIzpia, Vector2.up * yNoranzkoa, Color.red);

            if (kolpatu)
            {
                abiadura.y = (kolpatu.distance - azalZabalera) * yNoranzkoa;
                izpiLuzera = kolpatu.distance;

                if (kolpeak.aldapaIgotzen)
                {
                    abiadura.x = abiadura.y / Mathf.Tan(kolpeak.aldapaAngelu * Mathf.Deg2Rad) * Mathf.Sign(abiadura.x);
                }

                kolpeak.gainean = yNoranzkoa == 1;
                kolpeak.azpian = yNoranzkoa == -1;
            }
        }
    }

    //abiadura horizontala bertikalean eta horizontalean banatzen da aldapan
    //sin a = altuera / hipotenusa -> altuera = hipotenusa * sin a
    //cos a = luzera / hipotenusa -> luzera = hipotenusa * cos a
    void AldapaIgo(ref Vector2 abiadura, float angelua)
    {
        float distantzia = Mathf.Abs(abiadura.x);
        float abiaduraY = Mathf.Sin(angelua * Mathf.Deg2Rad) * distantzia;

        //salto egiten ez bagauz lurrean gaudela esango dugu aldapetan ere salto egin ahal izateko
        if (abiadura.y <= abiaduraY)
        {
            abiadura.y = abiaduraY;
            abiadura.x = Mathf.Cos(angelua * Mathf.Deg2Rad) * distantzia * Mathf.Sign(abiadura.x);
            kolpeak.azpian = true;
            kolpeak.aldapaIgotzen = true;
            kolpeak.aldapaAngelu = angelua;
        }
    }

    //abiadura horizontala bertikalean eta horizontalean banatzen da aldapan
    void AldapaJaitsi(ref Vector2 abiadura)
    {
        float xNoranzkoa = Mathf.Sign(abiadura.x);
        Vector2 izpia = (xNoranzkoa == -1) ? izpiJatorria.bottomRight : izpiJatorria.bottomLeft;
        RaycastHit2D kolpea = Physics2D.Raycast(izpia, Vector2.down, Mathf.Infinity, kolpeGainazalak);

        if (kolpea)
        {
            float aldapaAngelua = Vector2.Angle(kolpea.normal, Vector2.up);
            if (aldapaAngelua != 0 && aldapaAngelua <= aldapaAngeluMax)
            {
                if(Mathf.Sign(kolpea.normal.x) == xNoranzkoa)
                {
                    if (kolpea.distance - azalZabalera <= Mathf.Tan(aldapaAngelua * Mathf.Deg2Rad * Mathf.Abs(abiadura.x)))
                    {
                        float distantzia = Mathf.Abs(abiadura.x);
                        float abiaduraY = Mathf.Sin(aldapaAngelua * Mathf.Deg2Rad) * distantzia;
                        abiadura.x = Mathf.Cos(aldapaAngelua * Mathf.Deg2Rad) * distantzia * Mathf.Sign(abiadura.x);
                        abiadura.y -= Mathf.Sin(aldapaAngelua * Mathf.Deg2Rad) * distantzia;

                        kolpeak.azpian = true;
                        kolpeak.aldapaJaisten = true;
                        kolpeak.aldapaAngelu = aldapaAngelua;
                    }
                }
            }
            else
            {
                kolpeak.azpian = false;
            }
        }
    }

    //talkak kudeatzeko izpiak
    struct IzpiJatorria
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    //momentu oro gorputzaren posizioa kalkulatzen da, izpiak posizio berritik igortzeko.
    void IzpiJatorriaEguneratu()
    {
        Bounds mugak = bc2d.bounds;
        mugak.Expand(azalZabalera * -2);

        izpiJatorria.bottomLeft = new Vector2(mugak.min.x, mugak.min.y);
        izpiJatorria.bottomRight = new Vector2(mugak.max.x, mugak.min.y);
        izpiJatorria.topLeft = new Vector2(mugak.min.x, mugak.max.y);
        izpiJatorria.topRight = new Vector2(mugak.max.x, mugak.max.y);
    }

    //izpiak bata bestetik zenbateko distatziara jaurtitzen diren kalkulatzeko, lehen izpia ertz baten eta bukaerakoa kontrako ertzean hasi behar direla kontuan hartuta
    //derrigorrez bi izpi, horretarako da Mathf.clamp
    void IzpTarteakKalkulatu()
    {
        Bounds mugak = bc2d.bounds;
        mugak.Expand(azalZabalera * -2);

        izpiHorKop = Mathf.Clamp(izpiHorKop, 2, int.MaxValue);
        izpiBertKop = Mathf.Clamp(izpiBertKop, 2, int.MaxValue);

        horIzpiTartea = mugak.size.y / (izpiHorKop - 1);
        bertIzpiTartea = mugak.size.x / (izpiBertKop - 1);
    }

    //zein gainazal ukitzen gauden jakiteko
    public struct KolpeInfo
    {
        public bool gainean, azpian;
        public bool eskuma, ezkerra;

        public bool aldapaIgotzen;
        public bool aldapaJaisten;
        public float aldapaAngeluZahar, aldapaAngelu;

        public void Reset()
        {
            gainean = false;
            azpian = false;
            eskuma = false;
            ezkerra = false;

            aldapaIgotzen = false;
            aldapaJaisten = false;
            aldapaAngeluZahar = aldapaAngelu;
            aldapaAngelu = 0;
        }
    }
}
