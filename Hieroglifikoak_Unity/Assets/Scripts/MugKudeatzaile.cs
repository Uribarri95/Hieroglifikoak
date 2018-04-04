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

    public BoxCollider2D ColliderLortu()
    {
        return bc2d;
    }

    // Update is called once per frame
    //void Update () {
    //    
    //}

    //izpiak bata bestetik zenbateko distatziara jaurtitzen diren kalkulatzeko, lehen izpia ertz baten eta bukaerakoa kontrako ertzean hasi behar direla kontuan hartuta
    //derrigorrez bi izpi, horretarako da Mathf.clamp
    public void IzpTarteakKalkulatu()
    {
        Bounds mugak = bc2d.bounds;
        mugak.Expand(azalZabalera * -2);

        izpiHorKop = Mathf.Clamp(izpiHorKop, 2, int.MaxValue);
        izpiBertKop = Mathf.Clamp(izpiBertKop, 2, int.MaxValue);

        horIzpiTartea = mugak.size.y / (izpiHorKop - 1);
        bertIzpiTartea = mugak.size.x / (izpiBertKop - 1);
    }

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
                if (i == 0 && aldapaAngelua <= aldapaAngeluMax) //talka aldapa 'igogarri' baten
                {
                    AldapaIgo(ref abiadura, aldapaAngelua, kolpatu.normal);
                }
                else //talka horma edo malda handiko aldapan
                {
                    //lurra laua denean
                    abiadura.x = (kolpatu.distance - azalZabalera) * xNoranzkoa;
                    izpiLuzera = kolpatu.distance;

                    //aldapan gaudenean
                    if (kolpeak.aldapaIgotzen) // aldapan gaudenez abiadura bertikala eraldatu behar dugu
                    {
                        abiadura.y = Mathf.Tan(kolpeak.aldapaAngelu * Mathf.Deg2Rad) * Mathf.Abs(abiadura.x);
                    }

                    kolpeak.ezkerra = xNoranzkoa == -1;
                    kolpeak.eskuma = xNoranzkoa == 1;
                    kolpeak.normala = kolpatu.normal;
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
                izpiLuzera = kolpatu.distance; //mugitu daitekeen distantzia murristen da, bestela, hurrengo izpia jaurtitzean distantzia handiagoa ikusten badu, lehen oztopoa zeharkatuko du

                if (kolpeak.aldapaIgotzen) // aldapan gaudenean abiadura horizontala ere aldatu behar da
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
    //tan a = altuera / luzera
    void AldapaIgo(ref Vector2 abiadura, float angelua, Vector2 normala)
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
            kolpeak.normala = normala;
        }
    }

    //abiadura horizontala bertikalean eta horizontalean banatzen da aldapan
    void AldapaJaitsi(ref Vector2 abiadura)
    {
        RaycastHit2D eskumaIrritatu = Physics2D.Raycast(izpiJatorria.bottomLeft, Vector2.down, Mathf.Abs(abiadura.y) + azalZabalera, kolpeGainazalak);
        RaycastHit2D ezkerraIrritatu = Physics2D.Raycast(izpiJatorria.bottomRight, Vector2.down, Mathf.Abs(abiadura.y) + azalZabalera, kolpeGainazalak);
        if (eskumaIrritatu ^ ezkerraIrritatu)
        {
            aldapaIrristatu(eskumaIrritatu, ref abiadura);
            aldapaIrristatu(ezkerraIrritatu, ref abiadura);
        }

        if (!kolpeak.aldapaIrristatu) {
            float xNoranzkoa = Mathf.Sign(abiadura.x);
            Vector2 izpia = (xNoranzkoa == -1) ? izpiJatorria.bottomRight : izpiJatorria.bottomLeft;
            RaycastHit2D kolpea = Physics2D.Raycast(izpia, Vector2.down, Mathf.Infinity, kolpeGainazalak);

            if (kolpea)
            {
                float aldapaAngelua = Vector2.Angle(kolpea.normal, Vector2.up);
                if (aldapaAngelua != 0 && aldapaAngelua <= aldapaAngeluMax) // aldapa normala (ez horma, ez mald handia)
                {
                    if(Mathf.Sign(kolpea.normal.x) == xNoranzkoa) // aldapa behera goaz
                    {
                        if (kolpea.distance - azalZabalera <= Mathf.Tan(aldapaAngelua * Mathf.Deg2Rad) * Mathf.Abs(abiadura.x)) // aldapa jaitsi kontaktuan gaudenean bakarrik
                        {
                            float distantzia = Mathf.Abs(abiadura.x);
                            abiadura.x = Mathf.Cos(aldapaAngelua * Mathf.Deg2Rad) * distantzia * Mathf.Sign(abiadura.x);
                            abiadura.y -= Mathf.Sin(aldapaAngelua * Mathf.Deg2Rad) * distantzia;

                            kolpeak.azpian = true;
                            kolpeak.aldapaJaisten = true;
                            kolpeak.aldapaAngelu = aldapaAngelua;
                            kolpeak.normala = kolpea.normal;
                        }
                    }
                }
            }
        }
    }

    void aldapaIrristatu(RaycastHit2D kolpea, ref Vector2 abiadura)
    {
        if (kolpea)
        {
            float angelua = Vector2.Angle(kolpea.normal, Vector2.up);
            if (angelua > aldapaAngeluMax)
            {
                abiadura.x = Mathf.Sign(kolpea.normal.x) * (Mathf.Abs(abiadura.y) - kolpea.distance) / Mathf.Tan(angelua * Mathf.Deg2Rad);

                kolpeak.aldapaIrristatu = true;
                //kolpeak.azpian = true;
                kolpeak.aldapaAngelu = angelua;
                kolpeak.normala = kolpea.normal;
            }
        }
    }

    //talkak kudeatzeko izpiak
    struct IzpiJatorria
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    //jokalariaren posizio berria kontuan hartuta izpi berrien igorpen puntuak kalkulatzen dira
    void IzpiJatorriaEguneratu()
    {
        Bounds mugak = bc2d.bounds;
        mugak.Expand(azalZabalera * -2);

        izpiJatorria.bottomLeft = new Vector2(mugak.min.x, mugak.min.y);
        izpiJatorria.bottomRight = new Vector2(mugak.max.x, mugak.min.y);
        izpiJatorria.topLeft = new Vector2(mugak.min.x, mugak.max.y);
        izpiJatorria.topRight = new Vector2(mugak.max.x, mugak.max.y);
    }

    //zein gainazal ukitzen gauden jakiteko
    public struct KolpeInfo
    {
        public bool gainean, azpian;
        public bool eskuma, ezkerra;

        public bool aldapaIgotzen;
        public bool aldapaJaisten;
        public bool aldapaIrristatu;

        public float aldapaAngeluZahar, aldapaAngelu;
        public Vector2 normala;

        //arazoak ekiditzeko (etengabe salto, hormaren kontra itsatsi, malda handietan gora igo...)
        public void Reset()
        {
            gainean = false;
            azpian = false;
            eskuma = false;
            ezkerra = false;

            aldapaIgotzen = false;
            aldapaJaisten = false;
            aldapaIrristatu = false;

            aldapaAngeluZahar = aldapaAngelu;
            aldapaAngelu = 0;
            normala = Vector2.zero;
        }
    }
}
