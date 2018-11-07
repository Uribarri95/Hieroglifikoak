using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eraso : MonoBehaviour {

    JokalariMug jokalaria;
    Animator anim;
    Inbentarioa inbentarioa;
    SpriteMask argia;

    public Transform erasoPuntua;
    public GameObject gezia;
    public LayerMask etsaiak;
    public LayerMask piztu;

    float denbora;
    float denboraTartea = .3f;

    bool suArgia;
    bool ezpata;
    bool arkua;

    float suMinPuntuak = 5;
    float argiErradioa = 20;
    float argiErasoErradioa = .3f;
    float xPos = -.05f;
    float yPos = .075f;

    float erasoErradioa = .45f;
    public float ezpataMinPuntuak = 15;

    public float geziMinPuntuak = 10;

    // Use this for initialization
    void Start () {
        jokalaria = GetComponent<JokalariMug>();
        anim = GetComponent<Animator>();
        argia = GetComponentInChildren<SpriteMask>();
        argia.enabled = false;

        inbentarioa = Inbentarioa.instantzia;
    }
	
	// Update is called once per frame
	void Update () {
        NewItemKonprobatu();
        ArgiKudeaketa();
        ItemAldatu();
        if (inbentarioa.items.Count >= 1)
        {
            ItemEguneratu();
        }
        Erabili();
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

    // Inbentarioan 0 posiizon dagoen itema erabilgarri geratzen da eta besteak ezgaitzen dira
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
        Collider2D[] kolpatutakoEtsaiak = Physics2D.OverlapCircleAll(new Vector3(erasoPuntua.position.x + xPos, erasoPuntua.position.y + yPos, erasoPuntua.position.z), erradioa, etsaiak);
        for (int i = 0; i < kolpatutakoEtsaiak.Length; i++)
        {
            kolpatutakoEtsaiak[i].GetComponent<Etsaia>().KolpeaJaso(minPuntuak);
        }
    }

    // erradio barruan bagaude suTokia pizten du
    void SuaPiztu()
    {
        Collider2D[] suaPiztekoTokiak = Physics2D.OverlapCircleAll(new Vector3(erasoPuntua.position.x + xPos, erasoPuntua.position.y + yPos, erasoPuntua.position.z), argiErasoErradioa, piztu);
        for (int i = 0; i < suaPiztekoTokiak.Length; i++)
        {
            suaPiztekoTokiak[i].GetComponent<ErreDaiteke>().Piztu();
        }
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

    //animazioko event jaurtitzen du metodoa
    void ArgiakHanditu()
    {
        GetComponentInChildren<ArgiEfektua>().ArgiErasoa(argiErradioa);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(erasoPuntua.position, erasoErradioa);
    }
}
