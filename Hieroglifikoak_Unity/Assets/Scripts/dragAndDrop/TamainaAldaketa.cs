using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TamainaAldaketa : MonoBehaviour
{
    public float horizontalOffset;
    public float verticalOffset;
    public float spacing;
    public bool layoutBertikala;

    RectTransform tamaina;
    float hasieraLuzera;
    float hasieraAltuera;
    float luzera;
    float altuera;

    bool hutsunea = false;
    bool aldaketak = false;

    // Use this for initialization
    void Start()
    {
        tamaina = GetComponent<RectTransform>();
        hasieraLuzera = tamaina.sizeDelta.x;
        hasieraAltuera = tamaina.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        // textuaren tamaina ez da aldatzen
        if (GetComponent<Text>() != null)
        {
            luzera = hasieraLuzera;
            altuera = hasieraAltuera;
        }

        hutsunea = false;

        // seme kopurua aldatzean tamaina aldatu behar da
        if (transform.childCount > 0)
        {
            if (layoutBertikala)
            {
                altuera = (transform.childCount - 1) * spacing;
                luzera = transform.GetChild(0).GetComponent<TamainaAldaketa>().GetLuzera();
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name == "New Game Object")
                    {
                        hutsunea = true;
                    }
                    TamainaAldaketa piezaTamaina = transform.GetChild(i).GetComponent<TamainaAldaketa>();
                    altuera += piezaTamaina.GetAltuera();
                    if (piezaTamaina.GetLuzera() > luzera)
                    {
                        luzera = piezaTamaina.GetLuzera();
                    }
                }
            }
            else
            {
                altuera = transform.GetChild(0).GetComponent<TamainaAldaketa>().GetAltuera();
                luzera = (transform.childCount - 1) * spacing;
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name == "New Game Object")
                    {
                        hutsunea = true;
                    }
                    TamainaAldaketa piezaTamaina = transform.GetChild(i).GetComponent<TamainaAldaketa>();
                    luzera += piezaTamaina.GetLuzera();
                    if (piezaTamaina.GetAltuera() > altuera)
                    {
                        altuera = piezaTamaina.GetAltuera();
                    }
                }
            }
            luzera += horizontalOffset;
            altuera += verticalOffset;
        }
        // semerik ez -> hasierako tamainarekin gelditzen da
        else
        {
            luzera = hasieraLuzera;
            altuera = hasieraAltuera;
        }

        // ezin da hasierako tamaina baino txikiagoa izan
        if (luzera < hasieraLuzera)
        {
            luzera = hasieraLuzera;
        }
        // ezin da hasierako tamaina baino txikiagoa izan
        if (altuera < hasieraAltuera)
        {
            altuera = hasieraAltuera;
        }

        if (hutsunea)
        {
            if (!aldaketak)
            {
                aldaketak = true;
                tamaina.sizeDelta = new Vector2(luzera, altuera);
            }
        }
        else
        {
            aldaketak = false;
            tamaina.sizeDelta = new Vector2(luzera, altuera);
        }

        //tamaina.sizeDelta = new Vector2(luzera, altuera);
    }

    public float GetLuzera()
    {
        return tamaina.sizeDelta.x;
        //return luzera;
    }

    public float GetAltuera()
    {
        return tamaina.sizeDelta.y;
        //return altuera;
    }
}
