﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropTokia : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public Transform goikoPieza;
    public Transform piezaTokia;
    public Transform desagertu;
    public bool piezaBakarra;
    public Drag.mota piezaMota;

    Transform gurasoa;

    public void OnDrop(PointerEventData eventData)
    {
        Drag pieza = eventData.pointerDrag.GetComponent<Drag>();
        if (pieza != null)
        {
            if (piezaMota == pieza.piezaMota || piezaMota == Drag.mota.Denak) // pieza hemen jarri daiteke
            {
                if (piezaBakarra)
                {
                    if (transform.childCount == 1) // badago pieza bat jarrita, aldatu
                    {
                        Transform piezaPos = transform.GetChild(0);
                        if (piezaPos.GetComponent<Drag>() != null)
                        {
                            piezaPos.SetParent(piezaTokia);
                            piezaPos.SetSiblingIndex(piezaTokia.childCount);
                        }
                    }
                    else if (transform.childCount == 2)
                    {
                        Transform piezaPos = transform.GetChild(0);
                        if (piezaPos.GetComponent<Drag>() == null)
                        {
                            piezaPos = transform.GetChild(1);
                        }
                        piezaPos.SetParent(piezaTokia);
                        piezaPos.SetSiblingIndex(piezaTokia.childCount);
                    }
                }
                if (goikoPieza != null && goikoPieza.parent == piezaTokia)
                {
                    pieza.transform.SetParent(pieza.GetPiezaGurasoa());
                    pieza.transform.SetSiblingIndex(pieza.GetHasierkoIndizea());
                }
                else
                {
                    pieza.SetPiezaGurasoa(transform); // pieza hutsunean jarri
                }
            }
            else // pieza toki okerren jarri da, datorren tokira itzuli
            {
                pieza.transform.SetParent(pieza.GetPiezaGurasoa());
                pieza.transform.SetSiblingIndex(pieza.GetHasierkoIndizea());
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        Drag pieza = eventData.pointerDrag.GetComponent<Drag>();
        if (pieza != null)
        {
            if (goikoPieza != null)
            {
                gurasoa = goikoPieza.parent;
            }
            else
            {
                gurasoa = transform;
            }

            if (transform.childCount > 0 && transform.GetChild(0).GetComponent<Drag>() != null)
            {
                if (piezaMota == Drag.mota.Baldintza || piezaMota == Drag.mota.Eragigaia)
                {
                    pieza.SetHutsuneGurasoa(desagertu);
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        Drag pieza = eventData.pointerDrag.GetComponent<Drag>();
        if (pieza != null)
        {
            if (pieza.GetHutsuneGurasoa() == transform)
            {
                if (piezaBakarra)
                {
                    pieza.SetHutsuneGurasoa(desagertu);
                }
                else
                {
                    pieza.SetHutsuneGurasoa(gurasoa);
                }
            }
        }
    }
}