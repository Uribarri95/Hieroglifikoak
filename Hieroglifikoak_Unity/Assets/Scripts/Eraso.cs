using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eraso : MonoBehaviour {

    JokalariMug jokalaria;
    Animator anim;
    Inbentarioa inbentarioa;

    public Transform erasoPuntua;
    public GameObject gezia;

    float denbora;
    public float denboraTartea;

    bool suArgia;
    bool ezpata;
    bool arkua;

    public float suMinPuntuak = 5;
    public float argiErradioa = 3;

    public LayerMask etsaiak;
    public float erasoErradioa = .45f;
    public float ezpataMinPuntuak = 40;

    public float geziMinPuntuak = 10;

    // Use this for initialization
    void Start () {
        jokalaria = GetComponent<JokalariMug>();
        anim = GetComponent<Animator>();

        inbentarioa = Inbentarioa.instantzia;
    }
	
	// Update is called once per frame
	void Update () {
        ItemAldatu();
        if (inbentarioa.items.Count >= 1)
        {
            ItemEguneratu();
        }
        erabili();
	}

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

    void erabili()
    {
        if (denbora <= 0)
        {
            if (jokalaria.ErasoDezaket() && (/*suArgia || */ezpata || arkua))
            {
                if (Input.GetButtonDown("Eraso"))
                {
                    anim.SetBool("eraso", true);
                    denbora = denboraTartea;
                }
            }
        } else
        {
            denbora -= Time.deltaTime;
        }

        if (ezpata)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_sword_ground_attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_sword_jump_attack"))
            {
                Collider2D[] kolpatutakoEtsaiak = Physics2D.OverlapCircleAll(erasoPuntua.position, erasoErradioa, etsaiak);
                for (int i = 0; i < kolpatutakoEtsaiak.Length; i++)
                {
                    kolpatutakoEtsaiak[i].GetComponent<Etsaia>().KolpeaJaso(ezpataMinPuntuak);
                }
            }
        } /*else if (suArgia)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("player_torch_ground_attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("player_torch_jump_attack"))
            {
                Collider2D[] kolpatutakoEtsaiak = Physics2D.OverlapCircleAll(erasoPuntua.position, erasoErradioa, etsaiak);
                for (int i = 0; i < kolpatutakoEtsaiak.Length; i++)
                {
                    kolpatutakoEtsaiak[i].GetComponent<Etsaia>().KolpeaJaso(suMinPuntuak);
                }
            }

            // argi erradioa handitu
            // sua piztu
        }*/
    }

    //animazioko event jaurtitzen du metodoa
    public void GeziaJaurti()
    {
        GameObject geziaGO = Instantiate(gezia, erasoPuntua.position, erasoPuntua.rotation);
        Gezia gz = geziaGO.GetComponent<Gezia>();
        if(gz != null)
        {
            gz.setArrowDamage(geziMinPuntuak);
        }
    }
}
