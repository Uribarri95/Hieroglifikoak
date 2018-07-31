using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MugKudeatzaile))]
public class JokalariMug : MonoBehaviour {

    MugKudeatzaile kudeatzailea;

    Vector2 abiadura;

    float currentVelocity;

    public float aginduHorizontala;
    float mugimendua;
    public float leunketa;
    public float grabitatea = -50;
    public float saltoIndarra = 16;

    public float abiaduraOinez = 6;
    public float leuntzeNormala = .1f;

    public float abiaduraMakurtuta = 2;
    public float irristapenLeunketa = .8f;
    bool makurtu = false;
    bool irristatu = false;

    bool aldapaIrristatu = false;
    float aldapaIrristatuDenbora = .3f;
    float denboraIrristatu;

    public float korrikaAbiadura = 10;
    public float noranzkoaLeundu = .3f;
    bool korrika = false;

    public Vector2 paretaSaltoa = new Vector2(18, 14);
    float paretaIrristatuAbiadura = 3;
    int paretaNoranzkoa;
    bool paretaItsatsi;
    public float denboraItsatsita = .15f;
    float askatzeDenbora;

    public float aireanDenbora = .3f;
    float denbora;

    float irristatuDenbora = 0.05f;
    float denboraI;

    public bool eskileran;
    public bool eskileraIgotzen;

    public bool kutxaIkutzen;
    public Transform helduPuntua;
    public float erradioa;
    public LayerMask kutxa;
    public float kutxaBultzatuAbiadura = 4;

    Animator anim;
    SpriteRenderer nireSpriteRenderer;

    // Use this for initialization
    void Start () {
        kudeatzailea = GetComponent<MugKudeatzaile> ();

        mugimendua = abiaduraOinez;
        leunketa = leuntzeNormala;
        askatzeDenbora = denboraItsatsita;

        anim = GetComponent<Animator>();
        nireSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        Aginduak();
    }

    // Teklatutik jasotako eginduak kudeatu. Mugimendu bertikalari grabitate indarra aplikatzen zaio
    void Aginduak()
    {
        aginduHorizontala = Input.GetAxisRaw("Horizontal");
        if (eskileraIgotzen && eskileran)
            aginduHorizontala = 0;

        // SmoothDamp abiadura aldaketa leuntzeko (leunketa faktorearen arabera)
        abiadura.x = Mathf.SmoothDamp(abiadura.x, aginduHorizontala * mugimendua, ref currentVelocity, kudeatzailea.kolpeak.azpian ? leunketa : noranzkoaLeundu);

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

        // kudeatzailea.kolpeak.aldapaIrristatu erreseteatzen da, animazioa aldatzeko itxarote denbora txiki bat ezarri
        if (!kudeatzailea.kolpeak.aldapaIrristatu)
        {
            denboraI -= Time.deltaTime;
            if (denboraI <= 0)
            {
                anim.SetBool("aldapaIrristatu", kudeatzailea.kolpeak.aldapaIrristatu);
                denboraI = irristatuDenbora;
            }
        }
        else
        {
            denboraI = irristatuDenbora;
            anim.SetBool("aldapaIrristatu", kudeatzailea.kolpeak.aldapaIrristatu);
        }

        if (!paretaItsatsi && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_slide") && !aldapaIrristatu)
            NoranzkoaAldatu(aginduHorizontala);

        if (((anim.GetCurrentAnimatorStateInfo(0).IsName("player_stairs_climb_up") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_stairs_climb_down")) && abiadura.y == 0)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("player_push_box") && aginduHorizontala == 0 && kudeatzailea.kolpeak.azpian))
            anim.speed = 0;
        else
            anim.speed = 1;
        // animazioak

        // talkak zapai eta zoruarekin abiadura bertikala = 0 eta aldapa irristatu
        if (kudeatzailea.kolpeak.gainean || kudeatzailea.kolpeak.azpian)
        {
            if (kudeatzailea.kolpeak.aldapaIrristatu)
                abiadura.y += kudeatzailea.kolpeak.normala.y * grabitatea * Time.deltaTime;
            else
                abiadura.y = 0;
        }

        // saltoak
        if (Input.GetButton("Jump"))
        //if (Input.GetKey(KeyCode.UpArrow))
            SaltoBotoiaSakatu();
        else if (Input.GetButtonUp("Jump"))
        //else if(Input.GetKeyUp(KeyCode.UpArrow))
            SaltoBotoiaAskatu();

        // !!! diseinua: aldapa guztiak 45º-ko angelua edo gutxiago, animazioa egiteko orduan kontuan hartu !!!
        // makurtu eta irristatu
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // !!! ate baten aurrean
            if (kudeatzailea.kolpeak.azpian)
                MakurtuSakatu();

            // airean bagaude animazioa aldatuko da
            else if(makurtu && !kudeatzailea.kolpeak.azpian)
            {
                if (denbora >= aireanDenbora)
                {
                    MakurtuBotoiaAskatu();
                    denbora = 0;
                }
                else
                    denbora += Time.deltaTime;
            }
        }
        else if (!Input.GetKey(KeyCode.DownArrow))
        {
            if (makurtu)
                MakurtuBotoiaAskatu();
        }

        // kutxa bultzatu
        if (!makurtu)
        {
            kutxaIkutzen = Physics2D.OverlapCircle(helduPuntua.position, erradioa, kutxa);
            if (kutxaIkutzen)
            {
                mugimendua = kutxaBultzatuAbiadura;
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

        // korrika
        if (Input.GetKey(KeyCode.LeftShift))
            KorrikaBotoiaSakatu();
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            KorrikaBotoiaAskatu();

        if(!eskileraIgotzen)
            abiadura.y += grabitatea * Time.deltaTime;
        kudeatzailea.Mugitu(abiadura * Time.deltaTime, irristatu: makurtu);
    }

    // jokalariaren salto kudeaketa: aldapa irristatzen saltoa behera bakarrik, paretara itsatsita badago horma saltoa eta bestela salto normala
    public void SaltoBotoiaSakatu()
    {
        if (kudeatzailea.kolpeak.azpian)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide"))
            {
                abiadura.y = saltoIndarra * kudeatzailea.kolpeak.normala.y;
                abiadura.x = saltoIndarra * kudeatzailea.kolpeak.normala.x * 4; ///!!! 1.8f parametro bihurtu
            }
            else
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
        else if (paretaItsatsi)
        {
            mugimendua = abiaduraOinez;
            paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1;
            abiadura.x = -paretaNoranzkoa * paretaSaltoa.x;
            abiadura.y = paretaSaltoa.y;
        }
    }

    // salto botoia aldez aurretik askatzean salto txikiagoa egiten da
    public void SaltoBotoiaAskatu()
    {
        if (abiadura.y > 0)
            abiadura.y = abiadura.y * .5f;
    }

    // jokalaria makurtzen da, oso azkar badoa lurretik irristatuko da
    // !!! ate baten aurrean portaera ezberdinak !!!
    public void MakurtuSakatu()
    {
        if (!makurtu)
            kudeatzailea.GeruzaBertikalaHanditu(false);
        makurtu = true;
        mugimendua = abiaduraMakurtuta;

        // leunketa denbora gehiago ematen du irristatze efektua ematen
        if (Mathf.Abs(abiadura.x) > 6 && !kudeatzailea.kolpeak.eskuma && !kudeatzailea.kolpeak.ezkerra)
        {
            irristatu = true;
            leunketa = irristapenLeunketa;
        }
        else
        {
            irristatu = false;
            leunketa = leuntzeNormala;
        }
    }

    // jokalaria altzatzen da gainean oztoporik ez badauka
    public void MakurtuBotoiaAskatu()
    {
        irristatu = false;
        leunketa = leuntzeNormala;
        if (kudeatzailea.AltzatuNaiteke())
            Altzatu();
    }

    // jokalariaren abiadura areagotzen da makurtuta ez badago
    public void KorrikaBotoiaSakatu()
    {
        if (!makurtu && kudeatzailea.kolpeak.azpian)
        {
            korrika = true;
            mugimendua = korrikaAbiadura;
            leunketa = noranzkoaLeundu;
        }
    }

    // jokalariaren abiadura egoera berrira aldatzen da
    public void KorrikaBotoiaAskatu()
    {
        korrika = false;
        mugimendua = makurtu ? abiaduraMakurtuta : abiaduraOinez;
        leunketa = irristatu ? irristapenLeunketa : leuntzeNormala;
    }

    // jokalaria airean dagoenean paretara itsatsi daiteke hormaren kontrako noranzkoan salto egiteko
    public void HormaItsatsiKudeaketa()
    {
        int paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1;
        if (kudeatzailea.kolpeak.paretaitsatsi && !kudeatzailea.kolpeak.azpian && abiadura.y < 0)
        {
            paretaItsatsi = true;
            NoranzkoaAldatu(paretaNoranzkoa * -1);
            if (abiadura.y < -paretaIrristatuAbiadura)
                abiadura.y = -paretaIrristatuAbiadura;
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

    // mugimendua egoera berrira egokitzen da, jokalariak okupatzen duen gainazala berriro handitzen da
    void Altzatu()
    {
        makurtu = false;
        irristatu = false;
        leunketa = korrika ? noranzkoaLeundu : leuntzeNormala;
        mugimendua = korrika ? korrikaAbiadura : abiaduraOinez;
        kudeatzailea.GeruzaBertikalaHanditu(true);
    }

    public void AbiaduraAldatu(Vector2 eskileraAbiadura)
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

    // aldapa irristatu ostean pixka bat irristatu
    void AldapaIrristatu()
    {
        if (makurtu)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide"))
            {
                denboraIrristatu -= Time.deltaTime;
                if (denboraIrristatu <= 0)
                {
                    denboraIrristatu = aldapaIrristatuDenbora;
                    aldapaIrristatu = false;
                    leunketa = leuntzeNormala;
                }
            }
            else
            {
                denboraIrristatu = aldapaIrristatuDenbora;
                aldapaIrristatu = true;
                leunketa = irristapenLeunketa;
                //irristatu
                if (aldapaIrristatu && kudeatzailea.kolpeak.azpian)
                    abiadura.x = 8 * Mathf.Sign(kudeatzailea.kolpeak.normala.x); /// !!! paretaSaltoa apurtzen du
            }
        }
        else
        {
            denboraIrristatu = aldapaIrristatuDenbora;
            aldapaIrristatu = false;
            leunketa = leuntzeNormala;
        }
    }

    // sprite-aren noranzkoa aldatzen da
    public void NoranzkoaAldatu(float noranzkoa)
    {
        if (noranzkoa > 0 && nireSpriteRenderer.flipX)
        {
            nireSpriteRenderer.flipX = false;
            helduPuntua.position = new Vector2(transform.position.x + .4f, transform.position.y - .15f);
        }
        else if (noranzkoa < 0 && !nireSpriteRenderer.flipX)
        {
            nireSpriteRenderer.flipX = true;
            helduPuntua.position = new Vector2(transform.position.x - .4f, transform.position.y - .15f);
        }
    }
}