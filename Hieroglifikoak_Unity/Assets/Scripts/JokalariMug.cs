using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MugKudeatzaile))]
public class JokalariMug : MonoBehaviour
{

    MugKudeatzaile kudeatzailea;

    Vector2 abiadura;

    float currentVelocity;

    public float aginduHorizontala;
    float mugimendua;
    public float leunketa;
    public float grabitatea = -20;
    public float saltoIndarra = 10;

    public float abiaduraOinez = 4;
    public float leuntzeNormala = .1f;

    public float abiaduraMakurtuta = 1;
    public float irristapenLeunketa = 1.5f;
    bool makurtu = false;
    bool irristatu = false;

    float irristatuDenbora = 0.05f;
    float denboraIrristatu;

    public float aldapaSaltoa = 12;

    public float korrikaAbiadura = 6;
    public float noranzkoaLeundu = .2f;
    bool korrika = false;

    public Vector2 paretaSaltoa = new Vector2(10, 6);
    float paretaIrristatuAbiadura = 3;
    int paretaNoranzkoa;
    bool paretaItsatsi;
    public float denboraItsatsita = .15f;
    float askatzeDenbora;

    public bool eskileran;
    public bool eskileraIgotzen;

    public bool kutxaIkutzen;
    public Transform helduPuntua;
    public float erradioa = .1f;
    public LayerMask zerDaKutxa;
    public float kutxaBultzatuAbiadura = 3;

    public Transform erasoPuntua;
    public bool ezpata;
    public bool arkua;
    public bool sua;

    public bool hiltzen = false;
    public bool berpizten = false;

    Animator anim;
    SpriteRenderer nireSpriteRenderer;

    // Use this for initialization
    void Start()
    {
        kudeatzailea = GetComponent<MugKudeatzaile>();

        mugimendua = abiaduraOinez;
        leunketa = leuntzeNormala;
        askatzeDenbora = denboraItsatsita;
        irristatu = false;
        makurtu = false;

        anim = GetComponent<Animator>();
        nireSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // talkak zapai eta zoruaren aurka
        if (kudeatzailea.kolpeak.gainean || kudeatzailea.kolpeak.azpian)
            abiadura.y = 0;

        // hil/berpiztu animazioak agindutik kanpo
        anim.SetBool("hiltzen", hiltzen);
        anim.SetBool("berpiztu", berpizten);

        // hil animazio bitartean mugimendua ezgaituta eta jokalaria egoera arrunta jarri, bestela aginduak jaso
        if (hiltzen)
        {
            if (!berpizten)
            {
                ErasoaEten();
                KorrikaBotoiaAskatu();

                // jauzi efektua
                SetAbiaduraHorizontala(0);
                if (GetAbiaduraBertikala() >= 0)
                    SetAbiadura(new Vector2(0, 0));

                if (kutxaIkutzen)
                {
                    kutxaIkutzen = false;
                    KutxaBultzatu();
                }

                if (makurtu)
                {
                    MakurtuAskatu();
                    anim.SetBool("makurtuta", false);
                }
            }
            else
            {
                NoranzkoaAldatu(1);
                // jokalariari txanpon batzuk kendu
            }
        }
        else
        {
            Aginduak();
        }

        // eskilera gainean ez dago grabitate indarrik
        if (!eskileraIgotzen)
            abiadura.y += grabitatea * Time.deltaTime;

        kudeatzailea.Mugitu(abiadura * Time.deltaTime, irristatu: makurtu);
    }

    void Aginduak()
    {
        // lehen puzzlea, animazioa jarri behar da
        /*if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3.Lerp(transform.position, transform.position += new Vector3(-2, 0, 0), abiaduraOinez);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3.Lerp(transform.position, transform.position += new Vector3(-2, 0, 0), abiaduraOinez);
        }*/

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_sword_ground_attack"))
            aginduHorizontala = 0;
        else
            aginduHorizontala = Input.GetAxisRaw("Horizontal");

        // SmoothDamp abiadura aldaketa leuntzeko (leunketa faktorearen arabera)
        abiadura.x = Mathf.SmoothDamp(abiadura.x, aginduHorizontala * mugimendua, ref currentVelocity, !kudeatzailea.kolpeak.azpian && !irristatu ? .2f : leunketa);

        // hormara itsatzita gauden konprobatu
        HormaItsatsiKudeaketa();

        // aldapaIrristatu kudeatu
        AldapaIrristatu();

        // animazioak
        anim.SetFloat("xAbiadura", Mathf.Abs(aginduHorizontala * mugimendua));
        anim.SetFloat("yAbiadura", abiadura.y);
        anim.SetBool("lurrean", kudeatzailea.kolpeak.azpian);
        anim.SetBool("makurtuta", makurtu);
        anim.SetBool("irristatzen", irristatu);
        anim.SetBool("itsatsita", paretaItsatsi);
        anim.SetBool("eskileraIgo", eskileraIgotzen);
        anim.SetBool("kutxaBultzatu", kutxaIkutzen && kudeatzailea.kolpeak.azpian);

        anim.SetBool("ezpata", ezpata);
        anim.SetBool("arkua", arkua);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_push_box") || eskileraIgotzen)
            anim.SetBool("eraso", false);

        // kudeatzailea.kolpeak.aldapaIrristatu erreseteatzen da, animazioa aldatzeko itxarote denbora txiki bat ezarri
        if (!kudeatzailea.kolpeak.aldapaIrristatu)
        {
            denboraIrristatu -= Time.deltaTime;
            if (denboraIrristatu <= 0)
            {
                anim.SetBool("aldapaIrristatu", kudeatzailea.kolpeak.aldapaIrristatu);
                denboraIrristatu = irristatuDenbora;
            }
        }
        else
        {
            denboraIrristatu = irristatuDenbora;
            anim.SetBool("aldapaIrristatu", kudeatzailea.kolpeak.aldapaIrristatu);
        }

        if (!paretaItsatsi && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_slide"))
            NoranzkoaAldatu(aginduHorizontala);

        if (((anim.GetCurrentAnimatorStateInfo(0).IsName("player_stairs_climb_up") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_stairs_climb_down")) && abiadura.y == 0)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("player_push_box") && aginduHorizontala == 0 && kudeatzailea.kolpeak.azpian))
            anim.speed = 0;
        else
            anim.speed = 1;
        // animazioak

        // saltoak
        SaltoKudeaketa();


        // atea dagoenean
        /*if (Input.GetKey(KeyCode.DownArrow))
        {
            if (ateAurrean)
            {
                print("atea zabaltzen");
                //AteaZabaldu();
            }
            else if (kudeatzailea.kolpeak.azpian)
            {
                MakurtuSakatu();
            }
        }

        if (!Input.GetKey(KeyCode.DownArrow) && makurtu)
        {
            MakurtuAskatu();
        }*/


        // makurtu eta irristatu
        if (Input.GetKey(KeyCode.DownArrow) && kudeatzailea.kolpeak.azpian) // makurtu botoia sakatu lurrean gaudenean
            MakurtuSakatu();
        else if (!Input.GetKey(KeyCode.DownArrow) && makurtu) // makurtutua bagaude altzatu
            MakurtuAskatu();

        // kutxa bultzatu
        KutxaBultzatu();

        // korrika
        Korrika();

    }

    // jokalaria airean dagoenean paretara itsatsi daiteke hormaren kontrako noranzkoan salto egiteko
    public void HormaItsatsiKudeaketa()
    {
        int paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1;
        if (kudeatzailea.kolpeak.paretaitsatsi && !kudeatzailea.kolpeak.azpian && abiadura.y < 0)
        {
            paretaItsatsi = true;
            anim.SetBool("eraso", false);
            // jokalaria paretaren kontrako zentzuan begiratzen
            NoranzkoaAldatu(paretaNoranzkoa * -1);
            // ohikoa baino geldoago erortzen da
            if (abiadura.y < -paretaIrristatuAbiadura)
                abiadura.y = -paretaIrristatuAbiadura;
            // salto egin gabe askatzeko .15 segundu baino gehiago ikutu behar du mugimendu gezia
            if (askatzeDenbora > 0)
            {
                abiadura.x = paretaNoranzkoa;
                if (aginduHorizontala != 0 && aginduHorizontala != paretaNoranzkoa)
                    askatzeDenbora -= Time.deltaTime;
                else
                    askatzeDenbora = denboraItsatsita;
            }
            else
            {
                abiadura.x = 0;
                paretaItsatsi = false;
                askatzeDenbora = denboraItsatsita;
            }
        }
        else
            paretaItsatsi = false;
    }

    void SaltoKudeaketa()
    {
        if (Input.GetButton("Jump")) //salto botoia sakatu hil animaioa ez dagoen bitartean
        {
            if (kudeatzailea.kolpeak.azpian && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_sword_ground_attack"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide")) //aldapa irristatzen saltoa saltoa
                {
                    abiadura.y = saltoIndarra * .75f; /// !!! .75 parametro bihurtu
                    abiadura.x = Mathf.Sign(kudeatzailea.kolpeak.normala.x) * aldapaSaltoa;
                }
                else //salto normala
                {
                    if (makurtu && kudeatzailea.AltzatuNaiteke())
                    {
                        Altzatu();
                        abiadura.y = saltoIndarra;
                    }
                    else if (!makurtu)
                        abiadura.y = saltoIndarra;
                }
            }
            else if (paretaItsatsi) // pareta saltoa
            {
                KorrikaBotoiaAskatu();
                paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1;
                abiadura.x = -paretaNoranzkoa * paretaSaltoa.x;
                abiadura.y = paretaSaltoa.y;
            }
        }

        else if (Input.GetButtonUp("Jump")) // salto botoia askatu altuera maximoa lortu baino lehen
            if (abiadura.y > 0)
                abiadura.y = abiadura.y * .5f;
    }

    // jokalaria makurtzen da, oso azkar badoa lurretik irristatuko da
    public void MakurtuSakatu()
    {
        if (!makurtu)
            kudeatzailea.GeruzaBertikalaHanditu(false);
        makurtu = true;
        mugimendua = abiaduraMakurtuta;

        // azkar badoa leunketa denbora gehiago ematen da irristatze efektua ematen
        if (Mathf.Abs(abiadura.x) > 4 && !kudeatzailea.kolpeak.eskuma && !kudeatzailea.kolpeak.ezkerra)
        {
            leunketa = irristapenLeunketa;
            irristatu = true;
        }
        else
        {
            leunketa = leuntzeNormala;
            irristatu = false;
        }
    }

    // jokalaria altzatzen da gainean oztoporik ez badauka
    public void MakurtuAskatu()
    {
        irristatu = false;
        leunketa = korrika ? noranzkoaLeundu : leuntzeNormala;
        if (kudeatzailea.AltzatuNaiteke())
            Altzatu();
    }

    // jokalariak okupatzen duen gainazala berriro handitzen da, mugimendua egoera berrira egokitzen da
    void Altzatu()
    {
        makurtu = false;
        mugimendua = korrika ? korrikaAbiadura : abiaduraOinez;
        kudeatzailea.GeruzaBertikalaHanditu(true);
    }

    void KutxaBultzatu()
    {
        if (!makurtu) // makurtuta bagaude kutxa geldo eta animazio gabe mugituko da
        {
            //kutxaIkutzen = Physics2D.OverlapCircle(helduPuntua.position, erradioa, kutxa);
            Collider2D kutxa = Physics2D.OverlapCircle(helduPuntua.position, erradioa, zerDaKutxa);
            kutxaIkutzen = kutxa != null;
            if (kutxaIkutzen)
            {
                mugimendua = kutxaBultzatuAbiadura;
                //kutxa.GetComponent<KutxaMugKud>().Mugitu(new Vector2(abiadura.x,0) * Time.deltaTime);
                leunketa = 0;
            }
            else
            {
                leunketa = korrika ? noranzkoaLeundu : leuntzeNormala;
                if (makurtu)
                    mugimendua = abiaduraMakurtuta;
                else if (korrika)
                    mugimendua = korrikaAbiadura;
                else
                    mugimendua = abiaduraOinez;
            }
        }
    }

    void Korrika()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !makurtu && kudeatzailea.kolpeak.azpian) // jokalariaren abiadura areagotzen da makurtuta ez badago, noranzko aldaketa motelagoa da
        {
            korrika = true;
            mugimendua = korrikaAbiadura;
            leunketa = noranzkoaLeundu;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) // korrika botoia askatzean abiadura egoera berrira aldatzen da
            KorrikaBotoiaAskatu();
    }

    // jokalariaren abiadura egoera berrira aldatzen da
    public void KorrikaBotoiaAskatu()
    {
        korrika = false;
        mugimendua = makurtu ? abiaduraMakurtuta : abiaduraOinez;
        leunketa = irristatu ? irristapenLeunketa : leuntzeNormala;
    }

    public void ErasoaEten()
    {
        anim.SetBool("eraso", false);
    }

    void Berpiztu()
    {
        // animazioa
    }

    public void SetAbiaduraHorizontala(float x)
    {
        abiadura = new Vector2(x, abiadura.y);
    }

    public void SetAbiadura(Vector2 eskileraAbiadura)
    {
        abiadura = eskileraAbiadura;
    }

    public bool GetLurrean()
    {
        return kudeatzailea.kolpeak.azpian;
    }

    public bool GetMakurtu()
    {
        return makurtu;
    }

    public float GetAbiaduraBertikala()
    {
        return abiadura.y;
    }

    // aldapa irristatu ostean pixka bat irristatu
    void AldapaIrristatu()
    {
        if (makurtu && anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide") && kudeatzailea.kolpeak.azpian)
            if (!kudeatzailea.kolpeak.ezkerra && !kudeatzailea.kolpeak.eskuma)
                abiadura.x = 5 * Mathf.Sign(kudeatzailea.kolpeak.normala.x);
            else
                abiadura.x = 0;
    }

    // sprite-aren noranzkoa aldatzen da
    public void NoranzkoaAldatu(float noranzkoa)
    {
        if (noranzkoa > 0 && nireSpriteRenderer.flipX)
        {
            nireSpriteRenderer.flipX = false;
            helduPuntua.position = new Vector2(transform.position.x + .15f, transform.position.y - .15f);
            erasoPuntua.position = new Vector2(transform.position.x + .35f, transform.position.y - .115f);
            erasoPuntua.transform.Rotate(0f, 180f, 0f);
        }
        else if (noranzkoa < 0 && !nireSpriteRenderer.flipX)
        {
            nireSpriteRenderer.flipX = true;
            helduPuntua.position = new Vector2(transform.position.x - .15f, transform.position.y - .15f);
            erasoPuntua.position = new Vector2(transform.position.x - .35f, transform.position.y - .115f);
            erasoPuntua.transform.Rotate(0f, 180f, 0f);
        }
    }
}