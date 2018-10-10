using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutxaMugKud : IzpiKudeaketa {

    public LayerMask kolpeGainazalak;

    public KolpeInfo kolpeak;

    public float aldapaAngeluMax = 45;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }


    // erabiltzailearen aginduak jaso eta fisika indarrak aplikatzen zaizkio jokalaritik ateratzen diren izpiak erabilita
    public void Mugitu(Vector2 abiadura, bool plataformaGainean = false)
    {
        IzpiJatorriaEguneratu();
        kolpeak.Reset();

        if (abiadura.y < 0) { }
            AldapaJaitsi(ref abiadura);
        if (abiadura.x != 0)
            KolpeHorizontalak(ref abiadura);
        if (abiadura.y != 0)
            KolpeBertikalak(ref abiadura);

        if (plataformaGainean)
            kolpeak.azpian = true;

        transform.Translate(abiadura);
    }

    // Mugimendu horizontala jaso. Beste gorputz baten aurka talka egingo bada abiadura eraldatzen da
    // Kolpea eskuman edo ezkerran jaso den gordetzen da
    void KolpeHorizontalak(ref Vector2 abiadura)
    {
        float xNoranzkoa = Mathf.Sign(abiadura.x);
        float izpiLuzera = Mathf.Abs(abiadura.x) + azalZabalera;

        // izpi bakoitzeko talkak konprobatzen dira
        for (int i = 0; i < izpiHorKop; i++)
        {
            Vector2 jatorriIzpia = (xNoranzkoa == -1) ? izpiJatorria.bottomLeft : izpiJatorria.bottomRight;
            jatorriIzpia += Vector2.up * (horIzpiTartea * i);
            RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.right * xNoranzkoa, izpiLuzera, kolpeGainazalak);

            Debug.DrawRay(jatorriIzpia, Vector2.right * xNoranzkoa, Color.red);

            if (kolpatu)
            {
                if (kolpatu.transform.tag != "zeharkatu" && kolpatu.distance == 0)
                    abiadura.x = 0;

                if (kolpatu.transform.tag == "zeharkatu" || kolpatu.distance == 0)
                    continue;

                float aldapaAngelua = Vector2.Angle(kolpatu.normal, Vector2.up);

                // talka aldapa igogarri baten aurka
                if (i == 0 && aldapaAngelua <= aldapaAngeluMax)
                    AldapaIgo(ref abiadura, aldapaAngelua, kolpatu.normal);
                // ezin  da igo
                else
                {
                    // izpi luzera murriztu, hurrengo izpiak aurrekoak ikusi dutena baino lehen dagoena konprobatuko dute bakarrik
                    abiadura.x = (kolpatu.distance - azalZabalera) * xNoranzkoa;
                    izpiLuzera = kolpatu.distance;

                    //aldapan gaudenean abiadura bertikala eraldatu behar dugu
                    if (kolpeak.aldapaIgotzen)
                        abiadura.y = Mathf.Tan(kolpeak.aldapaAngelu * Mathf.Deg2Rad) * Mathf.Abs(abiadura.x);
                    else
                    {
                        kolpeak.ezkerra = xNoranzkoa == -1;
                        kolpeak.eskuma = xNoranzkoa == 1;
                    }
                    kolpeak.normala = kolpatu.normal;
                }
            }
        }
    }

    // Mugimendu bertikala jaso. Beste gorputz baten aurka talka egingo bada abiadura eraldatzen da
    // Kolpea goian edo azpian jaso den gordetzen da
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
                if (kolpatu.distance < 0.01f)
                    abiadura.y = 0;
                else
                {
                    // izpi luzera murriztu, hurrengo izpiak aurrekoak ikusi dutena baino lehen dagoena konprobatuko dute bakarrik
                    abiadura.y = (kolpatu.distance - azalZabalera) * yNoranzkoa;
                    izpiLuzera = kolpatu.distance;
                }

                // aldapan gaudenean abiadura horizontala ere aldatu behar da
                if (kolpeak.aldapaIgotzen)
                    abiadura.x = abiadura.y / Mathf.Tan(kolpeak.aldapaAngelu * Mathf.Deg2Rad) * Mathf.Sign(abiadura.x);

                kolpeak.gainean = yNoranzkoa == 1;
                kolpeak.azpian = yNoranzkoa == -1;
            }
        }
    }

    // abiadura horizontala bertikalean eta horizontalean banatzen da aldapan
    // sin a = altuera / hipotenusa -> altuera = hipotenusa * sin a
    // cos a = luzera / hipotenusa -> luzera = hipotenusa * cos a
    // tan a = altuera / luzera
    void AldapaIgo(ref Vector2 abiadura, float angelua, Vector2 normala)
    {
        float distantzia = Mathf.Abs(abiadura.x);
        float abiaduraY = Mathf.Sin(angelua * Mathf.Deg2Rad) * distantzia;

        // salto egiten ez bagauz lurrean gaudela esango dugu aldapetan ere salto egin ahal izateko
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

    // abiadura horizontala bertikalean eta horizontalean banatzen da aldapan
    void AldapaJaitsi(ref Vector2 abiadura)
    {
        float xNoranzkoa = Mathf.Sign(abiadura.x);
        Vector2 izpia = (xNoranzkoa == -1) ? izpiJatorria.bottomRight : izpiJatorria.bottomLeft;
        RaycastHit2D kolpea = Physics2D.Raycast(izpia, Vector2.down, Mathf.Infinity, kolpeGainazalak);
        if (kolpea)
        {
            float aldapaAngelua = Vector2.Angle(kolpea.normal, Vector2.up);
            // aldapa normala: ez horma, ez malda handia
            if (aldapaAngelua != 0 && aldapaAngelua <= aldapaAngeluMax)
            {
                // aldapa behera goaz
                if (Mathf.Sign(kolpea.normal.x) == xNoranzkoa)
                {
                    // aldapa jaitsi kontaktuan gaudenean bakarrik
                    if (kolpea.distance - azalZabalera <= Mathf.Tan(aldapaAngelua * Mathf.Deg2Rad) * Mathf.Abs(abiadura.x))
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

    // zein gainazal ukitzen gauden jakiteko
    public struct KolpeInfo
    {
        public bool gainean, azpian;
        public bool eskuma, ezkerra;

        public bool aldapaIgotzen;
        public bool aldapaJaisten;
        public bool aldapaIrristatu;
        public bool paretaitsatsi;

        public float aldapaAngelu;
        public Vector2 normala;

        public void Reset()
        {
            gainean = false;
            azpian = false;
            eskuma = false;
            ezkerra = false;

            aldapaIgotzen = false;
            aldapaJaisten = false;
            aldapaIrristatu = false;
            paretaitsatsi = false;

            aldapaAngelu = 0;
            normala = Vector2.zero;
        }
    }
}
