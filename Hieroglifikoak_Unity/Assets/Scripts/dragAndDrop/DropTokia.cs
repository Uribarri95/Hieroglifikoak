using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropTokia : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    Transform gurasoa;
    public bool baldintza;

    public void OnDrop(PointerEventData eventData)
    {
        Drag pieza = eventData.pointerDrag.GetComponent<Drag>();
        if (pieza != null)
        {
            pieza.SetPiezaGurasoa(transform);
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
            if (baldintza)
            {
                gurasoa = transform.parent.parent;
            }
            else
            {
                gurasoa = transform.parent;
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
        if (pieza != null && pieza.GetHutsuneGurasoa() == transform)
        {
            pieza.SetHutsuneGurasoa(gurasoa);
        }
    }
}
