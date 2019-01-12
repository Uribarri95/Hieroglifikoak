using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropTokia : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    //public Drag.PiezaMota mota; // baldintza -> pieza bakarra
    RectTransform tamaina;
    public float luzera;
    public float piezaAltuera;
    public float altueraMin = 100;
    public float padding = 20;
    public float spacing = 2;
    float altuera;
    float semeKop;

    // Use this for initialization
    void Start()
    {
        semeKop = transform.childCount;
        altuera = padding + (piezaAltuera * semeKop) + (spacing * (semeKop - 1));
        if(altuera < altueraMin)
        {
            altuera = altueraMin;
        }
        tamaina = GetComponent<RectTransform>();
        tamaina.sizeDelta = new Vector2(luzera, altuera);
    }

    // Update is called once per frame
    void Update()
    {
        if(semeKop < transform.childCount)
        {
            semeKop = transform.childCount;
            altuera = padding + (piezaAltuera * semeKop) + (spacing * (semeKop - 1));
        }
        tamaina.sizeDelta = new Vector2(luzera, altuera);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Drag pieza = eventData.pointerDrag.GetComponent<Drag>();
        if(pieza != null)
        {
            // if pieza.mota == mota
            pieza.SetGurasoa(transform);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerDrag == null)
        {
            return;
        }

        Drag pieza = eventData.pointerDrag.GetComponent<Drag>();
        if(pieza!= null)
        {
            pieza.SetPiezaGurasoa(transform);
            pieza.SetPiezaTokiGurasoa(transform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        Drag pieza = eventData.pointerDrag.GetComponent<Drag>();
        pieza.SetPiezaTokiGurasoa(pieza.GetGurasoa().parent);

        if (pieza != null && pieza.GetGurasoa() == transform)
        {
            pieza.SetPiezaGurasoa(transform);
        }
    }
}
