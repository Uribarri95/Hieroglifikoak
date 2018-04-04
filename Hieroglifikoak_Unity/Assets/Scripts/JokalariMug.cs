using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MugKudeatzaile))]
public class JokalariMug : MonoBehaviour {

    public float mugimendua;
    public float leunketa;

    public float abiaduraMakurtuta = 2;
    public float abiaduraOinez = 6;
    public float korrikaAbiadura = 10;

    public float irristapenLeunketa = .6f;
    public float leuntzeNormala = .1f;
    public float noranzkoaLeundu = .4f;

    public float paretaIrristatuAbiadura = 3;
    public Vector2 paretaSaltoa = new Vector2(18, 14);
    public float denboraItsatsita = .25f;
    public float askatzeDenbora;

    bool korrika = false;
    bool makurtu = false;
    bool irristatu = false;
    bool paretaItsatsi;

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

    //Teklatutik jasotako eginduak kudeatu. Mugimendu bertikalari grabitate indarra aplikatzen zaio
    void Aginduak()
    {
        //egoera arrunta, hasieraketa
        if (!korrika && !makurtu && !irristatu)
        {
            mugimendua = abiaduraOinez;
            leunketa = leuntzeNormala;
        }

        float input = Input.GetAxisRaw("Horizontal");

        int paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1; 
        paretaItsatsi = false;
        if ((kudeatzailea.kolpeak.ezkerra || kudeatzailea.kolpeak.eskuma) && !kudeatzailea.kolpeak.azpian && abiadura.y < 0) //paretara itsatsi
        {
            paretaItsatsi = true;
            if(abiadura.y < -paretaIrristatuAbiadura)
            {
                abiadura.y = -paretaIrristatuAbiadura;
            }
            if (askatzeDenbora > 0)
            {
                if (input != 0 && input != paretaNoranzkoa)
                {
                    askatzeDenbora -= Time.deltaTime;
                }
                else
                {
                    askatzeDenbora = denboraItsatsita;
                }
            }
        }

        if (kudeatzailea.kolpeak.gainean || kudeatzailea.kolpeak.azpian)
        {
            if (kudeatzailea.kolpeak.aldapaIrristatu) // aldapan 'jauzten' abiadura
            {
                abiadura.y += kudeatzailea.kolpeak.normala.y * grabitatea * Time.deltaTime;
            }
            else // mugimendu bertikala gelditu zapai edo zoruaren kontra jotzean, malda handiko aldapetan ez
            {
                abiadura.y = 0;
            }
        }

        if (Input.GetButtonDown("Jump")) // salto handia
        {
            if (kudeatzailea.kolpeak.azpian)
            {
                if (kudeatzailea.kolpeak.aldapaIrristatu) // aldapatik 'jauztean' saltoa behera, ezin da berriz igo
                {
                    if (Mathf.Sign(abiadura.x) == Mathf.Sign(kudeatzailea.kolpeak.normala.x) || Input.GetAxisRaw("Horizontal") == 0) // ezin da paretaren kontra salto egin
                    {
                        abiadura.y = saltoIndarra * kudeatzailea.kolpeak.normala.y;
                        abiadura.x = saltoIndarra * kudeatzailea.kolpeak.normala.x;
                    }
                }
                else //salto normala
                {
                    if (makurtu && kudeatzailea.AltzatuNaiteke()) //makurtuta bagaude bakarrik salto altzatu ostean
                    {
                        kudeatzailea.kolpeak.azpian = false;
                        Altzatu();
                        abiadura.y = saltoIndarra;
                    }else if (!makurtu)
                    {
                        abiadura.y = saltoIndarra;
                    }
                }
            }
            if (paretaItsatsi)
            {
                if (paretaNoranzkoa != input) // salto paretaren kontrako zentzuan
                {
                    abiadura.x = -paretaNoranzkoa * paretaSaltoa.x;
                    abiadura.y = paretaSaltoa.y;
                }
            }
        }
        else if (Input.GetButtonUp("Jump") && abiadura.y > 0) //salto txikia
        {
            abiadura.y = abiadura.y * .5f;
        }

        // aldapa max kudeatu animazteko orduan, irristatu aldapan inklinazioa kontuan hartu!!!!!************
        //makurtu eta irristatu
        if (Input.GetKey(KeyCode.DownArrow) && !makurtu && kudeatzailea.kolpeak.azpian)
        {
            //if...ate baten aurrean ez bagaude
            makurtu = true;
            mugimendua = abiaduraMakurtuta;
            kudeatzailea.GeruzaBertikalaAldatu(false);

            if (makurtu)
            {
                if (Mathf.Abs(abiadura.x) > 3.2 && !kudeatzailea.kolpeak.eskuma && ! kudeatzailea.kolpeak.ezkerra) //leunketa denbora gehiago ematen du irristatze efektua ematen
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
        else if (!Input.GetKey(KeyCode.DownArrow) && makurtu)
        {
            if (kudeatzailea.AltzatuNaiteke()) // gainean ezer
            {
                Altzatu();
            }
        }

        //talka baten ondorioz irristatzea gelditu
        if (makurtu && irristatu && (kudeatzailea.kolpeak.ezkerra || kudeatzailea.kolpeak.eskuma || Mathf.Abs(abiadura.x) < 3.2)) 
        {
            irristatu = false;
            leunketa = leuntzeNormala;
            kudeatzailea.GeruzaHorizontalaAldatu(true);
        }
        
        //korrika
        if (Input.GetKeyDown(KeyCode.LeftShift) && !korrika && !makurtu) // korrika botoia sakatu
        {
            korrika = true;
            mugimendua = korrikaAbiadura;
            leunketa = noranzkoaLeundu;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && korrika) // korrika botoia askatu
        {
            korrika = false;
            mugimendua = makurtu ? abiaduraMakurtuta : abiaduraOinez;
            leunketa = leuntzeNormala;
        }

        abiadura.x = Mathf.SmoothDamp(abiadura.x, input * mugimendua, ref currentVelocity, leunketa); //SmoothDamp abiadura aldaketa leuntzen du
        //abiadura.x = input * mugimendua;
        abiadura.y += grabitatea * Time.deltaTime;
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
    }

    void Altzatu()
    {
        makurtu = false;
        leunketa = korrika ? noranzkoaLeundu : leuntzeNormala;
        mugimendua = korrika ? korrikaAbiadura : abiaduraOinez;
        kudeatzailea.GeruzaBertikalaAldatu(true);
        if (irristatu)
        {
            kudeatzailea.GeruzaHorizontalaAldatu(true);
            irristatu = false;
        }
    }
}