using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MugKudeatzaile))]
public class JokalariMug : MonoBehaviour {

    MugKudeatzaile kudeatzailea;

    Vector2 abiadura;

    float currentVelocity;

    float aginduHorizontala;
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

    float IrristatuDenbora = 0.05f;
    float denboraI;

    public bool eskileran;
    public bool eskileraIgotzen;

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

        // animazioak
        anim.SetFloat("xAbiadura", Mathf.Abs(aginduHorizontala * mugimendua));
        anim.SetFloat("yAbiadura", abiadura.y);
        anim.SetBool("lurrean", kudeatzailea.kolpeak.azpian);
        anim.SetBool("makurtuta", makurtu);
        anim.SetBool("irristatzen", irristatu);
        anim.SetBool("itsatsita", paretaItsatsi);
        anim.SetBool("eskileraIgo", eskileraIgotzen);

        // kudeatzailea.kolpeak.aldapaIrristatu erreseteatzen da, animazioa aldatzeko itzarote denbora txiki bat ezarri
        if (!kudeatzailea.kolpeak.aldapaIrristatu)
        {
            denboraI -= Time.deltaTime;
            if (denboraI <= 0)
            {
                anim.SetBool("aldapaIrristatu", kudeatzailea.kolpeak.aldapaIrristatu);
                denboraI = IrristatuDenbora;
            }
        }
        else
        {
            denboraI = IrristatuDenbora;
            anim.SetBool("aldapaIrristatu", kudeatzailea.kolpeak.aldapaIrristatu);
        }

        if (!paretaItsatsi && !kudeatzailea.kolpeak.aldapaIrristatu)
            NoranzkoaAldatu(aginduHorizontala);
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
            SaltoBotoiaSakatu();
        else if (Input.GetButtonUp("Jump"))
            SaltoBotoiaAskatu();

        // !!! diseinua: aldapa guztiak 45º-ko angelua, animazioa egiteko orduan kontuan hartu !!!
        // makurtu eta irristatu
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // !!! ate baten aurrean
            if (kudeatzailea.kolpeak.azpian)
                MakurtuSakatu();

            // airean bagaude animazioa aldatuko da, denbora akatsen erruz airean gaudenerako da
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

        // irristatzea gelditu
        if (irristatu && (Mathf.Abs(abiadura.x) < 3.2 || kudeatzailea.kolpeak.ezkerra || kudeatzailea.kolpeak.eskuma || kudeatzailea.kolpeak.aldapaIgotzen))
        {
            irristatu = false;
            leunketa = leuntzeNormala;
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
            if (kudeatzailea.kolpeak.aldapaIrristatu && makurtu)
            {
                if (Mathf.Sign(abiadura.x) == Mathf.Sign(kudeatzailea.kolpeak.normala.x) || Input.GetAxisRaw("Horizontal") == 0)
                {
                    abiadura.y = saltoIndarra * kudeatzailea.kolpeak.normala.y;
                    abiadura.x = saltoIndarra * kudeatzailea.kolpeak.normala.x * 1.8f;
                }
            }
            else
            {
                if (makurtu && kudeatzailea.AltzatuNaiteke())
                {
                    Altzatu();
                    abiadura.y = saltoIndarra;
                }
                else if (!makurtu)
                {
                    abiadura.y = saltoIndarra;
                }
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
        if (Mathf.Abs(abiadura.x) > 3.2 && !kudeatzailea.kolpeak.eskuma && !kudeatzailea.kolpeak.ezkerra)
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
            {
                abiadura.y = -paretaIrristatuAbiadura;
            }
            if (askatzeDenbora > 0)
            {
                abiadura.x = paretaNoranzkoa;
                currentVelocity = paretaNoranzkoa;

                if (aginduHorizontala != 0 && aginduHorizontala != paretaNoranzkoa)
                {
                    askatzeDenbora -= Time.deltaTime;
                }
                else
                {
                    askatzeDenbora = denboraItsatsita;
                }
            }
            else
            {
                abiadura.x = 0;
                currentVelocity = 0;
                paretaItsatsi = false;
                askatzeDenbora = denboraItsatsita;
            }
        }
        else
        {
            paretaItsatsi = false;
        }
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

    public void EskilerakIgo(Vector2 eskileraAbiadura)
    {
        abiadura = eskileraAbiadura;
    }

    public bool GetLurrean()
    {
        return kudeatzailea.kolpeak.azpian;
    }

    // sprite-aren noranzkoa aldatzen da
    public void NoranzkoaAldatu(float noranzkoa)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide"))
        {
            if (noranzkoa > 0 && nireSpriteRenderer.flipX)
            {
                nireSpriteRenderer.flipX = false;
            }
            else if (noranzkoa < 0 && !nireSpriteRenderer.flipX)
            {
                nireSpriteRenderer.flipX = true;
            }
        }
    }
}