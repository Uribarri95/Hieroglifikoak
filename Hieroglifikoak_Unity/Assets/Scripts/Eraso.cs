using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eraso : MonoBehaviour {

    JokalariKudetzailea jokalariKudetzailea;    // bizitza puntuak 0 direnean hil eta berpizteko
    JokalariMug jokalaria;                      // jokalariaren egoera jakiteko (eraso dezake, atea zeharkatzen dago, hiltzen edo kolpea jasotzean bultzatzeko)
    Animator anim;
    SpriteRenderer sprite;                      // mina hartzean kolore aldakeak egiteko -> gorriz mina eta grisa garaitezina
    Inbentarioa inbentarioa;                    // ekipaturik zer duen eta bizitza puntuak jakiteko
    SpriteMask argia;                           // sua daukagunean ikusten den zatia
    List<Etsaia> etsaiak;                       // etsaiak behin kolpatzeko

    public Transform erasoPuntua;               // gezia agertzen den puntua / erasoaren erradioaren erdigunea
    public GameObject gezia;                    // gezi objektua, arkuarekin botatzeko
    public LayerMask zerDaEtsaia;               // etsaia kolpatzeko
    public LayerMask piztu;                     // sua pizteko

    float denbora;                              // eraso maiztasuna kudeatzeko
    public float denboraTartea = .3f;           // eraso maiztasuna kudeatzeko
    // !!! eraso maiztasuna arkua > ezpata !!!

    bool suArgia;                               // sua itema
    bool ezpata;                                // ezpata itema
    bool arkua;                                 // arkua itema

    public float suMinPuntuak = 5;              // suak egiten duen mina !!! kendu !!!
    public float argiErradioa = 20;             // su erasoaren argi erradioa
    float argiErasoErradioa = .3f;              // suErasoaren erradioa
    float xOffset = -.05f;                      // gezia agertzen den tokia, eraso puntuaren diferentzia
    float yOffset = .075f;                      // gezia agertzen den tokia, eraso puntuaren diferentzia

    float erasoErradioa = .43f;                 // erasoaren zirkunferentzia erradioa
    public float ezpataMinPuntuak = 15;         // ezpata erasoaren min puntuak

    public float geziMinPuntuak = 10;           // geziaren min puntuak

    public Color zuria, grisa;                  // mina hartzean kolore aldaketak
    bool garaiezina;                            // mina hartu ostean min gehiago ez hartzeko
    float kliskatuDenbora = .2f;                // garaitezin denbora

    void Start () {
        jokalariKudetzailea = FindObjectOfType<JokalariKudetzailea>();
        jokalaria = GetComponent<JokalariMug>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        inbentarioa = Inbentarioa.instantzia;
        argia = GetComponentInChildren<SpriteMask>();
        argia.enabled = false;
        etsaiak = new List<Etsaia>();
        garaiezina = false;
    }
	
	void Update () {
        NewItemKonprobatu();
        ItemAldatu();
        ArgiKudeaketa();
        if (inbentarioa.items.Count >= 1)
        {
            ItemEguneratu();
        }
        Erabili();
	}

    // item berria jasotzean animazio txiki bat gertatzen da
    void NewItemKonprobatu()
    {
        switch (inbentarioa.GetNewItem())
        {
            case null:
                return;
            case "SuArgia":
                anim.SetBool("newSua", true);
                anim.SetBool("newItem", true);
                inbentarioa.SetNewItem();
                break;
            case "Ezpata":
                if (sprite.flipX)
                {
                    jokalaria.NoranzkoaAldatu(1);
                }
                anim.SetBool("newEzpata", true);
                anim.SetBool("newItem", true);
                inbentarioa.SetNewItem();
                break;
            case "Arkua":
                anim.SetBool("newArkua", true);
                anim.SetBool("newItem", true);
                inbentarioa.SetNewItem();
                break;
            default:
                Debug.Log("Error: item-a ez dago zerrendan");
                break;
        }
    }

    // inbentarioko item artean aldatzeko
    void ItemAldatu()
    {
        if (inbentarioa.items.Count == 2)
        {
            if (Input.GetButtonDown("SwipeLeft"))
            {
                inbentarioa.SwipeLeft();
            }
        }
        else if (inbentarioa.items.Count == 3)
        {
            if (Input.GetButtonDown("SwipeLeft"))
            {
                inbentarioa.SwipeLeft();
            }
            else if (Input.GetButtonDown("SwipeRight"))
            {
                inbentarioa.SwipeRight();
            }
        }
    }

    // su argia daukagunean argiak aktibatu
    void ArgiKudeaketa()
    {
        if (inbentarioa.items.Count > 0)
        {
            if (inbentarioa.items[0].izena == "SuArgia")
            {
                argia.enabled = true;
                anim.SetLayerWeight(1, 1);
            }
            else
            {
                argia.enabled = false;
                anim.SetLayerWeight(1, 0);
            }
        }
    }

    // Inbentarioan 0 posizioan dagoen itema erabilgarri geratzen da eta besteak ezgaitzen dira
    private void ItemEguneratu()
    {
        switch (inbentarioa.items[0].izena)
        {
            case "SuArgia":
                suArgia = true;
                ezpata = false;
                arkua = false;
                break;
            case "Ezpata":
                suArgia = false;
                ezpata = true;
                arkua = false;
                break;
            case "Arkua":
                suArgia = false;
                ezpata = false;
                arkua = true;
                break;
            default:
                Debug.Log("Error: Zenbakia ez da egokia");
                suArgia = false;
                ezpata = false;
                arkua = false;
                break;
        }
        anim.SetBool("suArgia", suArgia);
        anim.SetBool("ezpata", ezpata);
        anim.SetBool("arkua", arkua);
    }

    // erabilgarri dagoen itema erabiltzen da
    void Erabili()
    {
        if (denbora <= 0)
        {
            if (jokalaria.ErasoDezaket() && (suArgia || ezpata || arkua))
            {
                if (Input.GetButtonDown("Eraso"))
                {
                    if (arkua && inbentarioa.GetGeziKop() <= 0)
                    {
                        return;
                    }
                    else
                    {
                        anim.SetBool("eraso", true);
                        denbora = denboraTartea;
                    }
                }
            }
        }
        else
        {
            denbora -= Time.deltaTime;
        }
    }

    // kolpea jaso eta bereala jokalaria garaiezin bihurtu segundu pare bat, etsaiarekiko distantzia hartu dezan
    // bizitzarik ez badu hil egingo da, bestela koll
    public bool KolpeaJaso(bool eskuma)
    {
        if (!garaiezina && !jokalaria.GetAteaZeharkatzen())
        {
            if (inbentarioa.KolpeaJaso())
            {
                jokalariKudetzailea.JokalariaHil();
                return true;
            }
            else
            {
                //knockback efektua
                jokalaria.KnockBack(eskuma);
                anim.SetBool("minEman", true);
                garaiezina = true;
                StartCoroutine(Garaiezina());
            }
        }
        return false;
    }

    // garaitezin / mina jaso kolore efektua
    IEnumerator Garaiezina()
    {
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(GrisaJarri());
            yield return new WaitForSeconds(kliskatuDenbora);
            StartCoroutine(ZuriJarri());
            yield return new WaitForSeconds(kliskatuDenbora);
        }
        garaiezina = false;
        yield return null;
    }

    // garaitezin kolore efektua
    private IEnumerator GrisaJarri()
    {
        float timer = 0.0f;
        float time = .2f;
        while (timer <= time)
        {
            timer += Time.deltaTime;
            float lerpEhunekoa = timer / time;
            sprite.color = Color.Lerp(zuria, grisa, lerpEhunekoa);
            yield return null;
        }
    }

    // mina jaso kolore efektua
    private IEnumerator ZuriJarri()
    {
        float timer = 0.0f;
        float time = .2f;
        while (timer <= time)
        {
            timer += Time.deltaTime;
            float lerpEhunekoa = timer / time;
            sprite.color = Color.Lerp(grisa, zuria, lerpEhunekoa);
            yield return null;
        }
    }

    // animazioko event jaurtitzen du
    // ezpata edo suArgiarekin erasotzea agintzen da
    public void Erasoa()
    {
        if (ezpata)
        {
            MinPuntuakKendu(ezpataMinPuntuak, erasoErradioa);
        }
        else if (suArgia)
        {
            // su mina kendu !!!
            MinPuntuakKendu(suMinPuntuak, argiErasoErradioa);
            SuaPiztu();
        }
    }

    //min puntuak eta erradioa emanda irisgarri dauden etsaiak kolpatzen ditu
    void MinPuntuakKendu(float minPuntuak, float erradioa)
    {
        Collider2D[] kolpatutakoEtsaiak = Physics2D.OverlapCircleAll(new Vector3(erasoPuntua.position.x + xOffset, erasoPuntua.position.y + yOffset, erasoPuntua.position.z), erradioa, zerDaEtsaia);
        for (int i = 0; i < kolpatutakoEtsaiak.Length; i++)
        {
            etsaiak.Add(kolpatutakoEtsaiak[i].GetComponent<Etsaia>());
            kolpatutakoEtsaiak[i].GetComponent<Etsaia>().KolpeaJaso(minPuntuak);
            kolpatutakoEtsaiak[i].GetComponent<Etsaia>().SetKolpea(true);
        }
    }

    // erradio barruan bagaude suTokia pizten du
    void SuaPiztu()
    {
        Collider2D suaPiztekoTokia = Physics2D.OverlapCircle(new Vector3(erasoPuntua.position.x + xOffset, erasoPuntua.position.y + yOffset, erasoPuntua.position.z), argiErasoErradioa, piztu);
        if (suaPiztekoTokia)
        {
            suaPiztekoTokia.GetComponent<ErreDaiteke>().Piztu();
        }
    }

    // animazioko event jaurtitzen du
    // etsai bakoitza behin bakarrik kolpatuko da
    void KolpeBakarra()
    {
        for (int i = 0; i < etsaiak.Count; i++)
        {
            etsaiak[i].SetKolpea(false);
        }
        etsaiak = new List<Etsaia>();
    }

    // animazioko event jaurtitzen du
    // gezia jaurtitzen da
    public void GeziaJaurti()
    {
        inbentarioa.GeziaJaurti();
        inbentarioa.UIEguneratu();

        GameObject geziaGO = Instantiate(gezia, erasoPuntua.position, erasoPuntua.rotation);
        Gezia gz = geziaGO.GetComponent<Gezia>();
        if(gz != null)
        {
            gz.SetArrowDamage(geziMinPuntuak);
        }
    }

    // animazioko event jaurtitzen du metodoa
    // argi erasoarekin argi zirkunferentzia handitzen da
    void ArgiakHanditu()
    {
        GetComponentInChildren<ArgiEfektua>().ArgiErasoa(argiErradioa);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(erasoPuntua.position, erasoErradioa);
    }
}
