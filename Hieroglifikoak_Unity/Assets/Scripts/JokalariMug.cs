using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MugKudeatzaile))]
public class JokalariMug : MonoBehaviour
{
    MugKudeatzaile kudeatzailea;

    Vector2 abiadura;
    float currentVelocity;

    float aginduHorizontala;
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
    float saltoNeurria = .75f;

    public float korrikaAbiadura = 6;
    public float noranzkoaLeundu = .2f;
    bool korrika = false;

    public Vector2 paretaSaltoa = new Vector2(10, 6);
    float paretaIrristatuAbiadura = 3;
    int paretaNoranzkoa;
    bool paretaItsatsi;
    public float denboraItsatsita = .15f;
    float askatzeDenbora;

    public bool eskileraIgotzen;

    public bool kutxaIkutzen;
    public Transform helduPuntua;
    public float erradioa = .1f;
    public LayerMask zerDaKutxa;

    public Transform erasoPuntua;

    public bool hiltzen = false;
    public bool berpizten = false;

    bool ateAurrean;
    bool ateaZeharkatzen;
    public bool gelaAldaketa = false;
    bool aldatu;
    bool eskumarantz;
    public float aldaketaDenbora;

    Animator anim;
    SpriteRenderer nireSpriteRenderer;

    // Use this for initialization
    void Start()
    {
        abiadura = new Vector2(0, 0);
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
        {
            abiadura.y = 0;
        }

        // eskilera gainean ez dago grabitate indarrik
        if (!eskileraIgotzen)
        {
            abiadura.y += grabitatea * Time.deltaTime;
        }

        // hil/berpiztu animazioak agindutik kanpo
        anim.SetBool("hiltzen", hiltzen);
        anim.SetBool("berpiztu", berpizten);

        // hil animazio bitartean mugimendua ezgaituta, berpiztu egoera arruntera itzultzeko. Bestela aginduak jaso
        if (hiltzen)
        {
            Berpiztu();
        }
        else if (gelaAldaketa)
        {
            GelaAldatu();
            anim.SetFloat("xAbiadura", mugimendua);
        }
        else
        {
            Aginduak();
        }
        
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

        // !!! anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_bow") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_torch") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_sword")
        // jokua gelditu edo tokia segurua

        // erasotzen bagaude mugimendua 0 da
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_sword_ground_attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_bow_ground_attack") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("player_torch_ground_attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_torch_light_up") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_bow") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_torch") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_sword") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_take_damage") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("player_torch_take_damage") || ateaZeharkatzen)
        {
            aginduHorizontala = 0;
        }
        else
        {
            if (!Pause.jokuaGeldituta)
            {
                aginduHorizontala = Input.GetAxisRaw("Horizontal");
                if(aginduHorizontala == 1 && !Ekintzak.instantzia.GetEskuma())
                {
                    aginduHorizontala = 0;
                }
                else if (aginduHorizontala == -1 && !Ekintzak.instantzia.GetEzkerra())
                {
                    aginduHorizontala = 0;
                }
            }
            else
            {
                aginduHorizontala = 0;
            }
        }

        // SmoothDamp abiadura aldaketa leuntzeko (leunketa faktorearen arabera)
        //print(aginduHorizontala);

        // !!! pause ez dagoenean soilik !!!
        abiadura.x = Mathf.SmoothDamp(abiadura.x, aginduHorizontala * mugimendua, ref currentVelocity, !kudeatzailea.kolpeak.azpian && !irristatu ? .2f : leunketa);

        // hormara itsatzita gauden konprobatu
        HormaItsatsiKudeaketa();

        // aldapaIrristatu kudeatu
        AldapaIrristatu();

        // animazioak
        AnimazioakKudeatu();

        // saltoak
        SaltoKudeaketa();

        // makurtu eta irristatu
        if (Input.GetKey(KeyCode.DownArrow) && kudeatzailea.kolpeak.azpian && !ateAurrean) // makurtu botoia sakatu lurrean gaudenean
        {
            MakurtuSakatu();
        }   
        else if (!Input.GetKey(KeyCode.DownArrow) && makurtu) // makurtutua bagaude altzatu
        {
            MakurtuAskatu();
        }

        // kutxa bultzatu
        KutxaBultzatu();

        // korrika
        Korrika();
    }

    void AnimazioakKudeatu()
    {
        anim.SetFloat("xAbiadura", Mathf.Abs(aginduHorizontala * mugimendua));
        anim.SetFloat("yAbiadura", abiadura.y);
        anim.SetBool("lurrean", kudeatzailea.kolpeak.azpian);
        anim.SetBool("makurtuta", makurtu); // !!! aldapairristatu
        anim.SetBool("irristatzen", irristatu);  // !!! aldapairristatu
        anim.SetBool("itsatsita", paretaItsatsi);
        anim.SetBool("eskileraIgo", eskileraIgotzen);
        anim.SetBool("kutxaBultzatu", kutxaIkutzen && kudeatzailea.kolpeak.azpian);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_bow") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_torch") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_sword"))
        {
            anim.SetBool("newSua", false);
            anim.SetBool("newEzpata", false);
            anim.SetBool("newArkua", false);
            anim.SetBool("newItem", false);
        }

        // kutxa bultzatzen eta eskilerak igotzen ezin da eraso
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_push_box") || eskileraIgotzen)
        {
            ErasoaEten();
        }

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

        // irristatzen edo paretara itsatsita ez bagaude noranzkoa teklatuko aginduaren arabera
        if (!paretaItsatsi && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_slide"))
        {
            NoranzkoaAldatu(aginduHorizontala);
        }

        // mugmiendua gelditzean animazioa ere gelditzen da
        if (((anim.GetCurrentAnimatorStateInfo(0).IsName("player_stairs_climb_up") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_stairs_climb_down")) && abiadura.y == 0)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("player_push_box") && aginduHorizontala == 0 && kudeatzailea.kolpeak.azpian))
        {
            anim.speed = 0;
        }
        else
        {
            anim.speed = 1;
        }
    }

    void Berpiztu()
    {
        if (!berpizten)
        {
            // jauzi efektua
            if (GetAbiaduraBertikala() >= 0)
            {
                SetAbiadura(new Vector2(0, 0));
            }
            else
            {
                SetAbiaduraHorizontala(0);
            }

            MinEmanKendu();
            ErasoaEten();
            KorrikaBotoiaAskatu();
            kutxaIkutzen = false;
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

    // aldapa irristatu ostean pixka bat irristatu
    void AldapaIrristatu()
    {
        if (makurtu && anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide") && kudeatzailea.kolpeak.azpian)
        {
            if (kudeatzailea.kolpeak.normala.x != 0 && !kudeatzailea.kolpeak.ezkerra && !kudeatzailea.kolpeak.eskuma)
            {
                abiadura.x = Mathf.Sign(kudeatzailea.kolpeak.normala.x) * 4.8f;  /// !!! parametro? aldapasaltoan eragina du!
            }
        }
    }

    // !!! pause ez dagoenean soilik !!!
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

    // jokalaria airean dagoenean paretara itsatsi daiteke hormaren kontrako noranzkoan salto egiteko
    public void HormaItsatsiKudeaketa()
    {
        int paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1;
        if (kudeatzailea.kolpeak.paretaitsatsi && !kudeatzailea.kolpeak.azpian && abiadura.y < 0)
        {
            paretaItsatsi = true;
            ErasoaEten();
            // jokalaria paretaren kontrako zentzuan begiratzen
            NoranzkoaAldatu(paretaNoranzkoa * -1);
            // ohikoa baino geldoago erortzen da
            if (abiadura.y < -paretaIrristatuAbiadura)
            {
                abiadura.y = -paretaIrristatuAbiadura;
            }
            // salto egin gabe askatzeko .15 segundu baino gehiago ikutu behar du mugimendu gezia
            if (askatzeDenbora > 0)
            {
                abiadura.x = paretaNoranzkoa;
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
                paretaItsatsi = false;
                askatzeDenbora = denboraItsatsita;
            }
        }
        else
        {
            paretaItsatsi = false;
        }
    }

    public void KnockBack(bool eskuma)
    {
        if (makurtu)
        {
            MakurtuAskatu();
        }
        if (eskuma)
        {
            abiadura.x = -5;
        }
        else
        {
            abiadura.x = 5;
        }
        abiadura.y = 5;
    }

    void SaltoKudeaketa()
    {
        if (Input.GetButton("Jump") && !anim.GetBool("eraso") && !ateaZeharkatzen)
        {
            if (kudeatzailea.kolpeak.azpian && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_sword_ground_attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_bow_ground_attack") && 
                !anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_bow") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_torch") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_sword"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide") && !kudeatzailea.kolpeak.ezkerra && !kudeatzailea.kolpeak.eskuma) //aldapa irristatzen saltoa saltoa
                {
                    abiadura.y = saltoIndarra * saltoNeurria;
                    abiadura.x = Mathf.Sign(kudeatzailea.kolpeak.normala.x) * aldapaSaltoa;
                }
                else //salto normala
                {
                    if (makurtu && kudeatzailea.AltzatuNaiteke())
                    {
                        Altzatu();
                        abiadura.y = Ekintzak.instantzia.GetSaltoTxikia()? saltoIndarra / 1.3f : saltoIndarra;
                        //abiadura.y = saltoIndarra;
                        anim.SetBool("eraso", false);
                    }
                    else if (!makurtu)
                    {
                        abiadura.y = Ekintzak.instantzia.GetSaltoTxikia() ? saltoIndarra / 1.3f : saltoIndarra;
                        //abiadura.y = saltoIndarra;
                        anim.SetBool("eraso", false);
                    }
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
        {
            if (abiadura.y > 0)
            {
                abiadura.y = abiadura.y * .5f;
            }
        }   
    }

    // jokalaria makurtzen da, oso azkar badoa lurretik irristatuko da
    public void MakurtuSakatu()
    {
        if (!makurtu)
        {
            kudeatzailea.GeruzaBertikalaHanditu(false);
        }
        makurtu = true;
        mugimendua = abiaduraMakurtuta;

        if(kudeatzailea.kolpeak.ezkerra || kudeatzailea.kolpeak.eskuma || kudeatzailea.kolpeak.aldapaIgotzen || kudeatzailea.kolpeak.aldapaJaisten)
        {
            abiadura.x = 0;
            aginduHorizontala = 0;
        }

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
        {
            Altzatu();
        }
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
            Collider2D kutxa = Physics2D.OverlapCircle(helduPuntua.position, erradioa, zerDaKutxa);
            kutxaIkutzen = kutxa != null;
            if (kutxaIkutzen && !Input.GetButton("Jump"))
            {
                kutxa.GetComponent<KutxaMugKud>().Mugitu(abiadura * Time.deltaTime);
                /*KutxaMugitu kutxaMugimendua = kutxa.GetComponent<KutxaMugitu>();
                kutxaMugimendua.SetBultzatzen(true);
                kutxaMugimendua.SetAbiadura(abiadura);*/
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
        {
            KorrikaBotoiaAskatu();
        }
    }

    // jokalariaren abiadura egoera berrira aldatzen da
    public void KorrikaBotoiaAskatu()
    {
        korrika = false;
        mugimendua = makurtu ? abiaduraMakurtuta : abiaduraOinez;
        leunketa = irristatu ? irristapenLeunketa : leuntzeNormala;
    }

    void GelaAldatu()
    {
        mugimendua = abiaduraOinez;
        abiadura.x = mugimendua * (eskumarantz ? 1 : -1);
        MinEmanKendu();
        ErasoaEten();
        KorrikaBotoiaAskatu();
        kutxaIkutzen = false;
        if (makurtu)
        {
            MakurtuAskatu();
            anim.SetBool("makurtuta", false);
        }
        if (aldatu)
        {
            StartCoroutine("AldaketaBukatu");
        }
    }

    IEnumerator AldaketaBukatu()
    {
        aldatu = false;
        yield return new WaitForSeconds(aldaketaDenbora);
        gelaAldaketa = false;
    }

    public void SetGelaAldaketa(bool aldaketa)
    {
        eskumarantz = aldaketa;
        gelaAldaketa = true;
        aldatu = true;
    }

    public bool ErasoDezaket()
    {
        return (!makurtu && !paretaItsatsi && !eskileraIgotzen && !ateaZeharkatzen);
    }

    public void ErasoaEten()
    {
        anim.SetBool("eraso", false);
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

    public bool GetEskileran()
    {
        return eskileraIgotzen;
    }

    public void SetEskileran(bool eskileran)
    {
        eskileraIgotzen = eskileran;
    }

    public bool GetHiltzen()
    {
        return hiltzen;
    }

    public void setAteAurrean(bool atean)
    {
        ateAurrean = atean;
    }

    public void MinEmanKendu()
    {
        anim.SetBool("minEman", false);
    }

    public void SetAteaZeharkatzen(bool zeharkatzen)
    {
        ateaZeharkatzen = zeharkatzen;
    }

    public bool GetAteaZeharkatzen()
    {
        return ateaZeharkatzen;
    }
}