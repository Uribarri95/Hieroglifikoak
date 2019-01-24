using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TamainaAldaketa : MonoBehaviour
{
    public float hPadding;
    public float vPadding;
    public float spacing;
    public bool layoutBertikala;

    RectTransform tamaina;
    float hasieraLuzera;
    float hasieraAltuera;
    float luzera;
    float altuera;

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
        if (GetComponent<Text>() != null)
        {
            luzera = hasieraLuzera;
            altuera = hasieraAltuera;
        }

        if (transform.childCount > 0)
        {
            if (layoutBertikala)
            {
                altuera = (transform.childCount - 1) * spacing;
                luzera = transform.GetChild(0).GetComponent<TamainaAldaketa>().GetLuzera();
                for (int i = 0; i < transform.childCount; i++)
                {
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
                    TamainaAldaketa piezaTamaina = transform.GetChild(i).GetComponent<TamainaAldaketa>();
                    luzera += piezaTamaina.GetLuzera();
                    if (piezaTamaina.GetAltuera() > altuera)
                    {
                        altuera = piezaTamaina.GetAltuera();
                    }
                }
            }
            luzera += hPadding;
            altuera += vPadding;
        }
        else
        {
            luzera = hasieraLuzera;
            altuera = hasieraAltuera;
        }

        if (luzera < hasieraLuzera)
        {
            luzera = hasieraLuzera;
        }
        if (altuera < hasieraAltuera)
        {
            altuera = hasieraAltuera;
        }

        tamaina.sizeDelta = new Vector2(luzera, altuera);
    }

    public float GetLuzera()
    {
        return luzera;
    }

    public float GetAltuera()
    {
        return altuera;
    }
}
