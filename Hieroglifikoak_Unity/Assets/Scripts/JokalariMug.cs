using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MugKudeatzaile))]
public class JokalariMug : MonoBehaviour
{

    Animator anim;                              // jokalariaren animazio kudeatzailea
    SpriteRenderer nireSpriteRenderer;          // jokalariaren sprite kudeatzailea
    MugKudeatzaile kudeatzailea;                // jokalariak talka egiten duen, aldapan dagoen eta paretaSaltoa egin dezakeen kudeatzen du, hauen arabera eta hemengo abiadurarekin jokalaria mugitzen du

    Vector2 abiadura;                                       // jokalariak hartuko duen abiadura
    float currentVelocity;                                  // jokalaria mugitzetik geldi egotera pasatzeko irristatze efektu txiki bat emateko

    float aginduHorizontala;                                // erabiltzaileak eskuma edo ezkerra gezia sakatu duen jasoko du, egoera berezietan aldatuko da (horma salto eta aldapa irristatu adibidez)
    float mugimendua;                                       // 3-4 abiadura posibletik zein erabiltzen ari den gordeko du
    float leunketa;                                         // abiadura aldaketak eta noranzko aldaketan irristatze efektu kantitatea

    float abiaduraOinez = 4;                                // jokalariaren oinezko abiadura
    float oinezLeunketa = .1f;                              // abiadura aldaketa egiteko leunketa faktorea oinez

    float korrikaAbiadura = 6;                              // jokalariaren abiadura korrika
    float korrikaLeunketa = .2f;                            // abiadura aldaketa egiteko leunketa faktorea korrika
    bool korrika = false;                                   // korrika gauden adierazten du

    float abiaduraMakurtuta = 1;                            // jokalariaren abiadura oinez
    float irristapenLeunketa = 1.5f;                        // abiadura aldaketa egiteko leunketa faktorea lurretik irristatzean
    bool makurtu = false;                                   // jokalaria makurtuta dagoen adierazten du
    bool irristatu = false;                                 // jokalaria irristatzen dagoen adierazten du (korrika egonda makurtu)

    float denboraIrristatu;                                 // aldapaIrristatu animazioa arazoa konpontzeko erlojua
    float irristatuDenbora = 0.05f;                         // aldapaIrristatu animazioa erraz kentzen da, .05 segundu baino denbora gehiagoan aldatuta badago orduan egingo da aldaketa


    float grabitatea = -20;                                 // grabiatate indarra, jokalaria lurrerantz bultzatzen duena
    float saltoIndarra = 7.55f;                             // jokalariaren hasierako salto indarra !!!
    float saltoIndarHobea = 9.8f;                           // jokalariaren bukaerako salto indarra

    float aldapaSaltoa = 12;                                // aldapan irristatzen gaudela salto eginda saltoa luzeagoa da (saltoaren abiadura horizontala)
    float saltoNeurria = .75f;                              // aldapan irristatzen gaudela salto eginda saltoa % 75 baxuagoa da (saltoaren abiadura bertikala)
    float irristatuNeurria = 4.8f;                          // aldapa irristatu ostean pixka bat gehiaggo irristatu, aldaparen behealdean ez geratzeko

    Vector2 paretaSaltoa = new Vector2(10, 7);              // hormara itsatsita gaudenean salto indarra (x ardatza, y ardatza)
    float paretaIrristatuAbiadura = -3;                     // hormara itsatsita gaudenean jauzten garen abiadura
    bool paretaItsatsi;                                     // hormara itsatsita gauden adierazten du (airean eta hormatik irristatzen)
    float denboraItsatsita = .15f;                          // hormatik askatzeko zenbat denbora sakatu behar den mugimendu botoia
    float askatzeDenbora;                                   // hormatik askatzeko denbora kontatzeko erlojua

    bool eskileraIgotzen;                                   // jokalaria eskilerara igota dagoen adierazten du

    bool kutxaIkutzen;                                      // jokalaria kutxa igotzen dagoen adierzten du (kutxa bultzatzeko)
    public Transform helduPuntua;                           // jokalariaren aurean dagoen puntua, kutxa ikutzen dagoen ikusiko duena
    float erradioa = .1f;                                   // jokalariaren aurean dagoen puntuaren tamaina
    public LayerMask zerDaKutxa;                            // kutxa zein multzo den, kutxa ez den beste objektu bat bultzatzen ez zailatzeko

    public Transform erasoPuntua;                           // jokalariak erasoa egiten duen punta, hemen jokalariaren noranzkoa aldatzean mugitzen da

    [HideInInspector]
    public bool hiltzen = false;                            // jokalaria hil animazioan dagoen adierazten du, aginduak eten behar dira. Jokalarikudeatzaileak aldatzen du
    [HideInInspector]
    public bool berpizten = false;                          // jokalaria berpizten dagoen adierazten du. Jokalarikudeatzaileak aldatzen du
    [HideInInspector]
    public bool kargatzen = false;                          // Lehen karga egiten ari den adierazten du. Mugimendua ezgaituta egon behar du

    bool ateAurrean;                                        // jokalaria ate baten aurrean dagoen adierazten du. Makurtu ezgaizten da
    bool ateaZeharkatzen;                                   // behin atea zeharkatzen gaudela ekintza batzuk ezgaitzen dira

    [HideInInspector]
    public bool gelaAldaketa = false;                       // gelaAldatzen gauden adierazten du (jokalariaren ekintzak ezgaitzen dira). GelaAldaketak erabiltzen du
    [HideInInspector]
    public bool gelaAldatzen;                               // gelaAldaketa behin bakarrik exekutatzen dela zihurtatzeko
    [HideInInspector]
    public bool eskumarantz;                                // gela berria eskuman edo ezkerrean dagoen
    float aldaketaDenbora = .1f;                            // gela batetik bestera joateko behar duen denbora (oinez dagoen denbora)

    // Use this for initialization
    void Start()
    {
        abiadura = new Vector2(0, 0);
        kudeatzailea = GetComponent<MugKudeatzaile>();

        mugimendua = abiaduraOinez;
        leunketa = oinezLeunketa;
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

        // hiltzen, aginduak ezgaituta, jokalaria jauzi eta egoera arruntera iztuli
        if (hiltzen || kargatzen)
        {
            BerpiztekoPrestatu();
        }
        // gela aldatzen, aginduak ezgaituta, jokalaria gela berrirantz mugitzen da
        else if (gelaAldaketa)
        {
            GelaAldatu();
            anim.SetFloat("xAbiadura", mugimendua);
        }
        // erabiltzailearen aginduen zain
        else
        {
            Aginduak();
        }
        
        // aginduen arabera jokalaria mugitu
        kudeatzailea.Mugitu(abiadura * Time.deltaTime, irristatu: makurtu);
    }

    void Aginduak()
    {
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
                // eskuma mugimendua desblokeatu gabe, mugimendua 0 da
                if(aginduHorizontala == 1 && !Ekintzak.instantzia.GetEskuma())
                {
                    aginduHorizontala = 0;
                }
                //ezkerra mugimendua desblokeatu gabe, mugimendua 0 da
                else if (aginduHorizontala == -1 && !Ekintzak.instantzia.GetEzkerra())
                {
                    aginduHorizontala = 0;
                }
            }
            // jokua gelditzen badugu mugimendua 0 da
            else
            {
                aginduHorizontala = 0;
            }
        }
        
        // abiadura aldaketak leundu
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
        if (Ekintzak.instantzia.GetMakurtu())
        {
            // makurtu botoia sakatu lurrean gaudenean
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && kudeatzailea.kolpeak.azpian && !ateAurrean)
            {
                MakurtuSakatu();
            }
            // makurtutua bagaude altxatu
            else if (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S) && makurtu)
            {
                MakurtuAskatu();
            }
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
        anim.SetBool("makurtuta", makurtu);
        anim.SetBool("irristatzen", irristatu);
        anim.SetBool("itsatsita", paretaItsatsi);
        anim.SetBool("eskileraIgo", eskileraIgotzen);
        anim.SetBool("kutxaBultzatu", kutxaIkutzen && kudeatzailea.kolpeak.azpian);

        // item berria jaso animazioa ezgaitu
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_bow") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_torch") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_sword"))
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

        // kudeatzailea.kolpeak.aldapaIrristatu erreseteatzen da, animazioa aldatzeko itxarote denbora txiki bat ezarri arazoa konpontzeko
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

        // kutxa bultzatzen eta eskilerak igotzen mugmiendua gelditzean animazioa ere gelditzen da
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

    // jokalaria berpizteko prestatzen da, jokalaria geldi mantentzen da, aginduak ezgaitzen dira eta noranzkoa eskumara da (checkpoint batetik eskumara doa jokalaria beti)
    void BerpiztekoPrestatu()
    {
        anim.SetBool("lurrean", true);

        // jokalaria hiltzen dago (lurrera jauzi efektua) edo lehen karga (mugimendua = 0)
        if (!berpizten)
        {
            // jauzi efektua, jokalaria geldi x ardatzean
            if (abiadura.y >= 0)
            {
                SetAbiadura(new Vector2(0, 0));
            }
            else
            {
                abiadura = new Vector2(0, abiadura.y);
            }

            // aginduak eta animazioak ezgaitzen dira
            EgoeraArrunteraItzuli();
        }
        else
        {
            // jokalaria eskumara begira uzten da (checkpoint batetik eskumara doa jokalaria beti)
            NoranzkoaAldatu(1);
        }
    }

    // aldapa irristatu ostean pixka bat irristatu
    void AldapaIrristatu()
    {
        if (Ekintzak.instantzia.GetAldapaIrristatu())
        {
            if (makurtu && anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide") && kudeatzailea.kolpeak.azpian)
            {
                if (kudeatzailea.kolpeak.normala.x != 0 && !kudeatzailea.kolpeak.ezkerra && !kudeatzailea.kolpeak.eskuma)
                {
                    abiadura.x = Mathf.Sign(kudeatzailea.kolpeak.normala.x) * irristatuNeurria;
                }
            }
        }
    }

    // sprite-aren noranzkoa aldatzen da, eraso eta kutxa bultzatzeko puntuak mugitzen dira
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

    // jokalaria airean dagoenean paretara itsatsi daiteke hormaren kontrako noranzkoan salto egiteko edo motelago erortzeko
    void HormaItsatsiKudeaketa()
    {
        int paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1;
        if (kudeatzailea.kolpeak.paretaitsatsi && !kudeatzailea.kolpeak.azpian && abiadura.y < 0)
        {
            paretaItsatsi = true;
            ErasoaEten();

            // jokalaria paretaren kontrako zentzuan begiratzen
            NoranzkoaAldatu(paretaNoranzkoa * -1);

            // ohikoa baino geldoago erortzen da
            if (abiadura.y < paretaIrristatuAbiadura)
            {
                abiadura.y = paretaIrristatuAbiadura;
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

    // jokalariak egin ahal dituen salto ezberdinak
    void SaltoKudeaketa()
    {
        if (!Pause.jokuaGeldituta)
        {
            if (Ekintzak.instantzia.GetSaltoTxikia() || Ekintzak.instantzia.GetSaltoHandia()) // || get irristatusaltoa || paretasaltoa
            {
                if (Input.GetButton("Jump") && !anim.GetBool("eraso") && !ateaZeharkatzen)
                {
                    if (kudeatzailea.kolpeak.azpian && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_sword_ground_attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_bow_ground_attack") &&
                        !anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_bow") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_torch") && !anim.GetCurrentAnimatorStateInfo(0).IsName("player_item_sword"))
                    {
                        //aldapa irristatzen saltoa
                        if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_slope_slide") && !kudeatzailea.kolpeak.ezkerra && !kudeatzailea.kolpeak.eskuma)
                        {
                            abiadura.y = saltoIndarHobea * saltoNeurria;
                            abiadura.x = Mathf.Sign(kudeatzailea.kolpeak.normala.x) * aldapaSaltoa;
                        }
                        // salto normala (desblokeatuta badago) 
                        else
                        {
                            if (makurtu && kudeatzailea.AltzatuNaiteke())
                            {
                                Altxatu();
                                abiadura.y = Ekintzak.instantzia.GetSaltoHandia() ? saltoIndarHobea : saltoIndarHobea / 1.3f;
                            }
                            else if (!makurtu)
                            {
                                abiadura.y = Ekintzak.instantzia.GetSaltoHandia() ? saltoIndarHobea : saltoIndarHobea / 1.3f;
                            }
                            anim.SetBool("eraso", false);
                        }
                    }
                    // pareta saltoa
                    else if (paretaItsatsi)
                    {
                        if (Ekintzak.instantzia.GetHormaSaltoa())
                        {
                            KorrikaBotoiaAskatu();
                            int paretaNoranzkoa = kudeatzailea.kolpeak.ezkerra ? -1 : 1;
                            abiadura.x = -paretaNoranzkoa * paretaSaltoa.x;
                            abiadura.y = paretaSaltoa.y;
                        }
                    }
                }
                // salto botoia askatu altuera maximoa lortu baino lehen
                else if (Input.GetButtonUp("Jump"))
                {
                    if (abiadura.y > 0)
                    {
                        abiadura.y = abiadura.y * .5f;
                    }
                }
            }
        }
    }

    // jokalaria makurtzen da, oso azkar badoa lurretik irristatuko da
    void MakurtuSakatu()
    {
        if (!Pause.jokuaGeldituta)
        {
            // jokalariaren tamaina murrizten da
            if (!makurtu)
            {
                kudeatzailea.GeruzaBertikalaHanditu(false);
            }
            makurtu = true;
            mugimendua = abiaduraMakurtuta;

            // hormaren kontra talka egitean abiadura 0 da
            if (kudeatzailea.kolpeak.ezkerra || kudeatzailea.kolpeak.eskuma)
            {
                abiadura.x = 0;
                aginduHorizontala = 0;
            }
            if (Ekintzak.instantzia.GetAldapaIrristatu())
            {
                if (kudeatzailea.kolpeak.aldapaIgotzen || kudeatzailea.kolpeak.aldapaJaisten)
                {
                    abiadura.x = 0;
                    aginduHorizontala = 0;
                }
            }

            if (Ekintzak.instantzia.GetIrristatu())
            {
                if (!kudeatzailea.kolpeak.aldapaIgotzen && !kudeatzailea.kolpeak.aldapaJaisten)
                {
                    // azkar badoa leunketa denbora gehiago ematen da irristatze efektua ematen
                    if (Mathf.Abs(abiadura.x) > 4 && !kudeatzailea.kolpeak.eskuma && !kudeatzailea.kolpeak.ezkerra)
                    {
                        leunketa = irristapenLeunketa;
                        irristatu = true;
                    }
                    else
                    {
                        leunketa = oinezLeunketa;
                        irristatu = false;
                    }
                }
            }
        }
    }

    // jokalaria altzatzen da gainean oztoporik ez badauka
    void MakurtuAskatu()
    {
        irristatu = false;
        leunketa = korrika ? korrikaLeunketa : oinezLeunketa;
        if (kudeatzailea.AltzatuNaiteke())
        {
            Altxatu();
        }
    }

    // jokalariak okupatzen duen gainazala berriro handitzen da, mugimendua egoera berrira egokitzen da
    void Altxatu()
    {
        makurtu = false;
        mugimendua = korrika ? korrikaAbiadura : abiaduraOinez;
        kudeatzailea.GeruzaBertikalaHanditu(true);
    }

    // zutunik gaudenean kutxa jokalariarekin mugitzen da
    void KutxaBultzatu()
    {
        // makurtuta bagaude kutxa geldo eta animazio gabe mugituko da
        if (!makurtu)
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

    // korrika botoia sakatzean jokalariarena abiadura areagotzen da
    void Korrika()
    {
        // korrika desblokeatuta
        if (Ekintzak.instantzia.GetKorrika())
        {
            // jokalariaren abiadura areagotzen da makurtuta ez badago, leunketa faktorea handiagooa da noranzko aldaketa motelagoa izateko
            if (Input.GetKey(KeyCode.LeftShift) && !makurtu && kudeatzailea.kolpeak.azpian)
            {
                korrika = true;
                mugimendua = korrikaAbiadura;
                leunketa = korrikaLeunketa;
            }
            // korrika botoia askatzean abiadura egoera berrira aldatzen da
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                KorrikaBotoiaAskatu();
            }
        }
    }

    // jokalariaren abiadura egoera berrira aldatzen da
    void KorrikaBotoiaAskatu()
    {
        korrika = false;
        mugimendua = makurtu ? abiaduraMakurtuta : abiaduraOinez;
        leunketa = irristatu ? irristapenLeunketa : oinezLeunketa;
    }

    // jokalaria min hartzean kolpe txiki bat jasotzen du. Eraso erabiltzen du mina hartzean
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

    // jokalaria gela barrurantz doa , aginduak ezgaitzen dira
    void GelaAldatu()
    {
        mugimendua = abiaduraOinez;
        abiadura.x = mugimendua * (eskumarantz ? 1 : -1);

        EgoeraArrunteraItzuli();
    }

    // jokalaria gela berri batera doala eta noranzkoa adierazten da
    public void SetGelaAldaketa(bool noranzkoa, bool mugitu)
    {
        gelaAldatzen = mugitu;
        if (mugitu)
        {
            gelaAldaketa = mugitu;
            eskumarantz = noranzkoa;
        }
        else
        {
            StartCoroutine(GelaAldaketaBukatu());
        }
    }

    // gela aldaketa egiteko oinez doa, denbora pasatu ondoren  gelditzen da
    IEnumerator GelaAldaketaBukatu()
    {
        yield return new WaitForSeconds(aldaketaDenbora);
        gelaAldaketa = false;
    }

    // jokalaria gela berri batera doala eta noranzkoa adierazten da
    public void SetGelaAldaketa(bool noranzkoa)
    {
        eskumarantz = noranzkoa;
        gelaAldaketa = true;
    }

    // jokalariak egin ahal dituen ekintzak ezgaitzen dira
    private void EgoeraArrunteraItzuli()
    {
        ErasoaEten();
        KorrikaBotoiaAskatu();
        kutxaIkutzen = false;
        if (makurtu)
        {
            MakurtuAskatu();
            anim.SetBool("makurtuta", false);
        }
        anim.SetBool("minEman", false);
    }

    // jokalaria eraso dezake baldintza betetzen badira
    public bool ErasoDezaket()
    {
        return !makurtu && !paretaItsatsi && !eskileraIgotzen && !ateaZeharkatzen && !Pause.jokuaGeldituta;
    }

    // erasoa eteten da, animazioak bukatzeko event
    void ErasoaEten()
    {
        anim.SetBool("eraso", false);
    }

    // abiadura aldatzen da, eskileran gaudenean kontrola eskilerak dauka eta atea zabaltzean abiadura 0 jartzeko
    public void SetAbiadura(Vector2 eskileraAbiadura)
    {
        abiadura = eskileraAbiadura;
    }

    // abiadura itzultzen du, eskilerak erabiltzen du.
    public Vector2 GetAbiadura()
    {
        return abiadura;
    }

    // abiadura bertikala aldatu, eskilerak erabiltzen du.
    public void SetAbiaduraBertikala(float yAbiadura)
    {
        abiadura.y = yAbiadura;
    }

    // jokalaria lurrean badago true, eskilerak eta atea behar dute
    public bool GetLurrean()
    {
        return kudeatzailea.kolpeak.azpian;
    }

    // eskileran gaudela ohartarazteko
    public void SetEskileran(bool eskileran)
    {
        eskileraIgotzen = eskileran;
    }

    // vcam hil animazioan gauden jakiteko
    public bool GetHiltzen()
    {
        return hiltzen;
    }

    // atea aurrean gaudela ohartarazteko
    public void setAteAurrean(bool atean)
    {
        ateAurrean = atean;
    }

    // atea zeharkatzen gaudela ohartarazteko
    public void SetAteaZeharkatzen(bool zeharkatzen)
    {
        ateaZeharkatzen = zeharkatzen;
    }

    // atea zeharkatzen gauden, minik ez hartzeko
    public bool GetAteaZeharkatzen()
    {
        return ateaZeharkatzen;
    }

    // min hartu animazioa bukatzeko
    public void MinEmanKendu()
    {
        anim.SetBool("minEman", false);
    }
}