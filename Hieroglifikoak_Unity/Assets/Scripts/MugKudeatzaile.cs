using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MugKudeatzaile : IzpiKudeaketa {

    public KolpeInfo kolpeak;

    public float aldapaAngeluMax = 45;

	// Use this for initialization
	public override void Start () {
        base.Start();
        kolpeak.noranzkoa = 1;
    }

    public BoxCollider2D ColliderLortu()
    {
        return bc2d;
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
            kolpeak.noranzkoa = (int)Mathf.Sign(abiadura.x);
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
        float xNoranzkoa = kolpeak.noranzkoa;
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
                if (aldapaAngelua != 0 && aldapaAngelua <= aldapaAngeluMax) // aldapa normala (ez horma, ez malda handia)
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
                kolpeak.aldapaAngelu = angelua;
                kolpeak.normala = kolpea.normal;
            }
        }
    }

    //irristatzean jokalaria etzan egiten da (altuera / 2 eta alderantziz)
    public void GeruzaBertikalaHanditu(bool handitu)
    {
        Vector3 eskala = bc2d.transform.localScale;
        Vector3 posizioa = bc2d.transform.position;
        float pos = bc2d.transform.position.y;
        if (handitu)
        {
            eskala.y *= 2;
            bc2d.transform.localScale = eskala;
            pos = pos + eskala.y / 4;
        }
        else
        {
            eskala.y *= .5f;
            bc2d.transform.localScale = eskala;
            pos = pos - eskala.y / 2;
        }
        bc2d.transform.position = new Vector3(posizioa.x, pos, posizioa.z);
        IzpTarteakKalkulatu();
    }

    //irristatzean jokalaria etzan egiten da (zabalera * 2 eta alderantziz)
    public void GeruzaHorizontalaAldatu(bool handitu)
    {
        Vector3 eskala = bc2d.transform.localScale;
        Vector3 posizioa = bc2d.transform.position;
        //float pos = colliderra.transform.position.x; //posizioa ikutu nahi badugu noranzkoa jakin behar da
        if (handitu)
        {
            eskala.x *= .5f;
            bc2d.transform.localScale = eskala;
            //pos = 
        }
        else
        {
            eskala.x *= 2;
            bc2d.transform.localScale = eskala;
            //pos = 
        }
        //colliderra.transform.position = new Vector3(posizioa.x, pos, posizioa.z);
        IzpTarteakKalkulatu();
    }

    //true gainean oztoporik ez badago
    public bool AltzatuNaiteke()
    {
        BoxCollider2D colliderra = ColliderLortu();
        float jokalariLuzera = colliderra.transform.localScale.y;
        
        RaycastHit2D ezkerKolpea = Physics2D.Raycast(izpiJatorria.topLeft, Vector2.up, jokalariLuzera, kolpeGainazalak);
        RaycastHit2D eskuinKolpea = Physics2D.Raycast(izpiJatorria.topRight, Vector2.up, jokalariLuzera, kolpeGainazalak);

        return (!ezkerKolpea && !eskuinKolpea);
    }

    //zein gainazal ukitzen gauden jakiteko
    public struct KolpeInfo
    {
        public bool gainean, azpian;
        public bool eskuma, ezkerra;

        public bool aldapaIgotzen;
        public bool aldapaJaisten;
        public bool aldapaIrristatu;

        public float aldapaAngelu;
        public Vector2 normala;
        public int noranzkoa;

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

            aldapaAngelu = 0;
            normala = Vector2.zero;
        }
    }
}
