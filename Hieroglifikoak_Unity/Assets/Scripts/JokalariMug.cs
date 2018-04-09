using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MugKudeatzaile))]
public class JokalariMug : MonoBehaviour {

    float aginduHorizontala;

    public float mugimendua;
    public float leunketa;

    public float abiaduraMakurtuta = 2;
    public float abiaduraOinez = 6;
    public float korrikaAbiadura = 10;

    public float irristapenLeunketa = .6f;
    public float leuntzeNormala = .1f;
    public float noranzkoaLeundu = .3f;

    public float paretaIrristatuAbiadura = 3;
    public Vector2 paretaSaltoa = new Vector2(18, 10);
    public float denboraItsatsita = .25f;
    public float askatzeDenbora;
    int paretaNoranzkoa;
    bool paretaItsatsi;

    bool korrika = false;
    bool makurtu = false;
    bool irristatu = false;

    public float grabitatea = -50;
    public float saltoIndarra = 20;

    float currentVelocity;
    Vector2 abiadura;

    MugKudeatzaile kudeatzailea;

	// Use this for initialization
	void Start () {
        kudeatzailea = GetComponent<MugKudeatzaile> ();
	}
	
	// Update is called once per frame
	void Update () {
        Aginduak();
    }

    // Teklatutik jasotako eginduak kudeatu. Mugimendu bertikalari grabitate indarra aplikatzen zaio
    void Aginduak()
    {
        // egoera arrunta, hasierakoa
        if (!korrika && !makurtu && !irristatu)
        {
            mugimendua = abiaduraOinez;
            leunketa = leuntzeNormala;
        }

        // teklatuAginduak hobeto dagoenean lerroa ezabatu
        aginduHorizontala = Input.GetAxisRaw("Horizontal");

        // SmoothDamp abiadura aldaketa leuntzeko (leunketa faktorearen arabera)
        abiadura.x = Mathf.SmoothDamp(abiadura.x, aginduHorizontala * mugimendua, ref currentVelocity, leunketa);

        HormaItsatsiKudeaketa();

        // aldapaIrristatu eta talkak zapai eta zoruarekin abiadura bertikala = 0
        if (kudeatzailea.kolpeak.gainean || kudeatzailea.kolpeak.azpian)
        {
            if (kudeatzailea.kolpeak.aldapaIrristatu) // !!! aldapa angelua kontuan hartu animazioa ezartzeko orduan !!!
            {
                abiadura.y += kudeatzailea.kolpeak.normala.y * grabitatea * Time.deltaTime;
            }
            else
            {
                abiadura.y = 0;
            }
        }

        // saltoak
        if (Input.GetButtonDown("Jump"))
        {
            SaltoBotoiaSakatu();
        }
        else if (Input.GetButtonUp("Jump") && abiadura.y > 0)
        {
            SaltoBotoiaAskatu();
        }

        // !!! aldapa max kudeatu animazteko orduan, irristatu aldapan inklinazioa kontuan hartu !!!
        // makurtu eta irristatu
        if (Input.GetKey(KeyCode.DownArrow) && !makurtu && kudeatzailea.kolpeak.azpian)
        {
            MakurtuSakatu();
        }
        else if (!Input.GetKey(KeyCode.DownArrow) && makurtu)
        {
            MakurtuBotoiaAskatu();
        }

        // talka baten ondorioz irristatzea gelditu
        if (makurtu && irristatu && (kudeatzailea.kolpeak.ezkerra || kudeatzailea.kolpeak.eskuma || Mathf.Abs(abiadura.x) < 3.2)) 
        {
            irristatu = false;
            leunketa = leuntzeNormala;
            kudeatzailea.GeruzaHorizontalaAldatu(true);
        }
        
        // korrika
        if (Input.GetKey(KeyCode.LeftShift) && !korrika && !paretaItsatsi && kudeatzailea.kolpeak.azpian)
        {
            KorrikaBotoiaSakatu();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && korrika || paretaItsatsi)
        {
            KorrikaBotoiaAskatu();
        }

        abiadura.y += grabitatea * Time.deltaTime;
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
    }

    // jokalariaren salto kudeaketa: aldapa irristaten saltoa behera bakarrik, paretara itsatsita badago horma saltoa eta bestela salto normala
    public void SaltoBotoiaSakatu()
    {
        paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1;
        if (kudeatzailea.kolpeak.azpian)
        {
            if (kudeatzailea.kolpeak.aldapaIrristatu)
            {
                if (Mathf.Sign(abiadura.x) == Mathf.Sign(kudeatzailea.kolpeak.normala.x) || Input.GetAxisRaw("Horizontal") == 0)
                {
                    abiadura.y = saltoIndarra * kudeatzailea.kolpeak.normala.y;
                    abiadura.x = saltoIndarra * kudeatzailea.kolpeak.normala.x;
                }
            }
            else
            {
                if (makurtu && kudeatzailea.AltzatuNaiteke())
                {
                    kudeatzailea.kolpeak.azpian = false;
                    Altzatu();
                    abiadura.y = saltoIndarra;
                }
                else if (!makurtu)
                {
                    abiadura.y = saltoIndarra;
                }
            }
        }
        if (paretaItsatsi)
        {
            abiadura.x = -paretaNoranzkoa * paretaSaltoa.x;
            abiadura.y = paretaSaltoa.y;
        }
    }

    // salto botoia aldez aurretik askatzean salto txikiagoa egiten da
    public void SaltoBotoiaAskatu()
    {
        abiadura.y = abiadura.y * .5f;
    }

    // jokalaria makurtzen da, oso azkar badoa lurretik irristatuko da
    public void MakurtuSakatu()
    {
        // !!! if ate baten aurrean ez bagaude !!!
        makurtu = true;
        mugimendua = abiaduraMakurtuta;
        kudeatzailea.GeruzaBertikalaHanditu(false);

        if (makurtu)
        {
            if (Mathf.Abs(abiadura.x) > 3.2 && !kudeatzailea.kolpeak.eskuma && !kudeatzailea.kolpeak.ezkerra) //leunketa denbora gehiago ematen du irristatze efektua ematen
            {
                irristatu = true;
                leunketa = irristapenLeunketa;
                kudeatzailea.GeruzaHorizontalaAldatu(false);
            }
            else
            {
                irristatu = false;
                leunketa = leuntzeNormala;
            }
        }
    }

    // jokalaria altzatzen da gainean oztoporik ez badauka
    public void MakurtuBotoiaAskatu()
    {
        if (kudeatzailea.AltzatuNaiteke())
        {
            Altzatu();
        }
    }

    // jokalariaren abiadura areagotzen da makurtuta ez badago
    public void KorrikaBotoiaSakatu()
    {
        korrika = true;
        if (!makurtu)
        {
            mugimendua = korrikaAbiadura;
            leunketa = noranzkoaLeundu;
        }
    }

    // jokalariaren abiadura egoera berrira aldatzen da
    public void KorrikaBotoiaAskatu()
    {
        korrika = false;
        mugimendua = makurtu ? abiaduraMakurtuta : abiaduraOinez;
        leunketa = leuntzeNormala;
    }

    // jokalaria airean dagoenean paretara itsatsi daiteke hormaren kontrako noranzkoan salto egiteko
    public void HormaItsatsiKudeaketa()
    {
        int paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1;
        if (kudeatzailea.kolpeak.ezkerra || kudeatzailea.kolpeak.eskuma)
        {
            if (!kudeatzailea.kolpeak.azpian && abiadura.y < 0)
            {
                paretaItsatsi = true;   // !!! goiko eta beheko izpiak daudenean bakarrik !!!
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
        else
        {
            paretaItsatsi = false;
        }
    }

    // mugimendua egoera berrira egokitzen da, jokalariak okupatzen duen gainazala berriro handitzen da
    void Altzatu()
    {
        makurtu = false;
        leunketa = korrika ? noranzkoaLeundu : leuntzeNormala;
        mugimendua = korrika ? korrikaAbiadura : abiaduraOinez;
        kudeatzailea.GeruzaBertikalaHanditu(true);
        if (irristatu)
        {
            kudeatzailea.GeruzaHorizontalaAldatu(true);
            irristatu = false;
        }
    }
}