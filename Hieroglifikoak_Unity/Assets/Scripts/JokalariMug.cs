using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MugKudeatzaile))]
public class JokalariMug : MonoBehaviour {

    public float mugimendua;
    public float abiaduraMakurtuta = 2;
    public float abiaduraOinez = 6;
    public float korrikaAbiadura = 10;
    public float leunketa;
    public float leuntzeNormala = .1f;
    public float leunduIrristatzea = 1.2f;
    public float noranzkoaLeundu = .4f;

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

    //Teklatutik jasotako eginduak kudeatu. Mugimendu bertikalari grabitate indarra aplikatzen zaio
    void Aginduak()
    {
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

        if (Input.GetButtonDown("Jump") && kudeatzailea.kolpeak.azpian) // salto handia
        {
            if (kudeatzailea.kolpeak.aldapaIrristatu) // aldapatik 'jauztean' saltoa behera, ezin da berriz igo
            {
                //if (Mathf.Sign(abiadura.x) == Mathf.Sign(kudeatzailea.kolpeak.normala.x) || Input.GetAxisRaw("Horizontal") == 0) //ezin da paretaren kontra salto egin 
                //{
                abiadura.y = saltoIndarra * kudeatzailea.kolpeak.normala.y;
                    abiadura.x = saltoIndarra * kudeatzailea.kolpeak.normala.x;
                //}
            }
            else //salto normala
            {
                abiadura.y = saltoIndarra;
            }
        }
        else if (Input.GetButtonUp("Jump") && abiadura.y > 0) //salto txikia
        {
            abiadura.y = abiadura.y * .5f;
        }

        //egoera arrunta
        if (!korrika && !makurtu && !irristatu)
        {
            mugimendua = abiaduraOinez;
            leunketa = leuntzeNormala;
        }

        // aldapa max kudeatu animazteko orduan, irristatu aldapan inklinazioa kontuan hartu!!!!!************
        //makurtu eta irristatu
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //if...ate baten aurrean ez bagaude
            makurtu = true;
            mugimendua = abiaduraMakurtuta;
            // geruza eraldatu
            GeruzaAldatu(false);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) && makurtu)  // && gainean ezer
        {
            //if (AltzatuNaiteke)
            makurtu = false;
            leunketa = leuntzeNormala;
            mugimendua = korrika ? korrikaAbiadura : abiaduraOinez;
            GeruzaAldatu(true);
        }
        if (makurtu)
        {
            if (Mathf.Abs(abiadura.x) > 3.2) //leunketa denbora gehiago ematen du irristatze efektua ematen
            {
                irristatu = true;
                leunketa = kudeatzailea.kolpeak.aldapaIgotzen ? noranzkoaLeundu : leunduIrristatzea;
            }
            else
            {
                irristatu = false;
                leunketa = leuntzeNormala;
            }
        }
        //korrika
        if (Input.GetKeyDown(KeyCode.LeftShift) && !korrika && !makurtu) // korrika botoia sakatu
        {
            korrika = true;
            mugimendua = korrikaAbiadura;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && korrika) // korrika botoia askatu
        {
            korrika = false;
            mugimendua = makurtu ? abiaduraMakurtuta : abiaduraOinez;
        }

        if (korrika && !makurtu) //korrika gaudela eta noranzkoa aldatzean frenatzea
        {
            leunketa = noranzkoaLeundu;
        }
        if (!kudeatzailea.kolpeak.azpian)
        {
            leunketa = leuntzeNormala + .1f;
        }

        float input = Input.GetAxisRaw("Horizontal");
        abiadura.x = Mathf.SmoothDamp(abiadura.x, input * mugimendua, ref currentVelocity, leunketa); //SmoothDamp abiadura aldaketa leuntzen du
        abiadura.y += grabitatea * Time.deltaTime;
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
    }

    void GeruzaAldatu(bool handitu)
    {
        BoxCollider2D colliderra = kudeatzailea.ColliderLortu();
        Vector3 eskala = colliderra.transform.localScale;
        Vector3 posizioa = colliderra.transform.position;
        float pos = colliderra.transform.position.y;
        if (handitu)
        {
            eskala.y *= 2;
            colliderra.transform.localScale = eskala;
            pos = -.12f + pos / 2;
        }
        else
        {
            eskala.y *= .5f;
            colliderra.transform.localScale = eskala;
            pos = pos - eskala.y / 2;
        }
        colliderra.transform.position = new Vector3(posizioa.x, pos, posizioa.z);
        kudeatzailea.IzpTarteakKalkulatu();
    }

    //public bool AltzatuNaiteke()
    //{
        //raycast...
        //if (!hit)
        //Vector2 ezkerIzpia = izpiJatorria.topLeft;
        //Vector2 eskumaIzpia = izpiJatorria.topRight;
        //RaycastHit2D kolpea = Physics2D.Raycast(izpia, Vector2.up, jokalariLuzera, kolpeGainazalak);
        //return !kolpea
    //}

}