using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eraso : MonoBehaviour {

    JokalariMug jokalaria;
    Animator anim;
    SpriteRenderer sprite;
    Inbentarioa inbentarioa;
    SpriteMask argia;
    List<Etsaia> etsaiak;

    public Transform erasoPuntua;
    public GameObject gezia;
    public LayerMask zerDaEtsaia;
    public LayerMask piztu;

    float denbora;
    float denboraTartea = .3f;

    bool suArgia;
    bool ezpata;
    bool arkua;

    public float suMinPuntuak = 5;
    float argiErradioa = 20;
    float argiErasoErradioa = .3f;
    float xPos = -.05f;
    float yPos = .075f;

    float erasoErradioa = .45f;
    public float ezpataMinPuntuak = 15;

    public float geziMinPuntuak = 10;

    public Color zuria, grisa;
    bool garaiezina;
    float garaiezinDenbora = 1.5f;
    float kliskatuDenbora = .1f;

    // Use this for initialization
    void Start () {
        jokalaria = GetComponent<JokalariMug>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        inbentarioa = Inbentarioa.instantzia;
        argia = GetComponentInChildren<SpriteMask>();
        argia.enabled = false;
        etsaiak = new List<Etsaia>();

        garaiezina = false;
    }
	
	// Update is called once per frame
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

    // inbentarioko itemen artean aldatzeko
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
    public bool KolpeaJaso(bool eskuma)
    {
        if (!garaiezina)
        {
            if (inbentarioa.KolpeaJaso())
            {
                return true;
            }
            else
            {
                //knockback efektua
                jokalaria.KnockBack(eskuma);
                anim.SetBool("minaHartu", true);
                garaiezina = true;
                StartCoroutine(Garaiezina());
            }
        }
        return false;
    }

    IEnumerator Garaiezina()
    {
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(GrisaJarri());
            yield return new WaitForSeconds(.2f);
            StartCoroutine(ZuriJarri());
            yield return new WaitForSeconds(.2f);
        }
        garaiezina = false;
        yield return null;
    }

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

    public void MinaHartuKendu()
    {
        anim.SetBool("minaHartu", false);
    }

    // animazioko event jaurtitzen du metodoa
    // ezpata edo suArgiarekin erasotzea agintzen da
    public void Erasoa()
    {
        if (ezpata)
        {
            MinPuntuakKendu(ezpataMinPuntuak, erasoErradioa);
        }
        else if (suArgia)
        {
            MinPuntuakKendu(suMinPuntuak, argiErasoErradioa);
            SuaPiztu();
        }
    }

    //min puntuak eta erradioa emanda irisgarri dauden etsaiak kolpatzen ditu
    void MinPuntuakKendu(float minPuntuak, float erradioa)
    {
        Collider2D[] kolpatutakoEtsaiak = Physics2D.OverlapCircleAll(new Vector3(erasoPuntua.position.x + xPos, erasoPuntua.position.y + yPos, erasoPuntua.position.z), erradioa, zerDaEtsaia);
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
        Collider2D suaPiztekoTokia = Physics2D.OverlapCircle(new Vector3(erasoPuntua.position.x + xPos, erasoPuntua.position.y + yPos, erasoPuntua.position.z), argiErasoErradioa, piztu);
        if (suaPiztekoTokia)
        {
            suaPiztekoTokia.GetComponent<ErreDaiteke>().Piztu();
        }
    }

    // animazioko event jaurtitzen du metodoa
    // etsai bakoitza behin bakarrik kolpatuko da
    void KolpeBakarra()
    {
        for (int i = 0; i < etsaiak.Count; i++)
        {
            etsaiak[i].SetKolpea(false);
        }
        etsaiak = new List<Etsaia>();
    }

    //animazioko event jaurtitzen du metodoa
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

    //animazioko event jaurtitzen du metodoa
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
