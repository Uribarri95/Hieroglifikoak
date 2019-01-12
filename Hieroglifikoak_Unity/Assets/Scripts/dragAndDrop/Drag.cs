using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    //public enum PiezaMota { Eragiketa, Baldintza } // baldintza -> pieza bakarra
    //public PiezaMota mota;

    GameObject piezaTokia;
    Transform gurasoa;
    Transform piezaGurasoa;

    public void OnBeginDrag(PointerEventData eventData)
    {
        piezaTokia = new GameObject();
        piezaTokia.transform.SetParent(transform.parent);

        LayoutElement layouta = piezaTokia.AddComponent<LayoutElement>();
        layouta.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        layouta.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        layouta.flexibleWidth = 0;
        layouta.flexibleHeight = 0;

        piezaTokia.transform.SetSiblingIndex(transform.GetSiblingIndex());

        gurasoa = transform.parent;
        piezaGurasoa = transform.parent;
        transform.SetParent(gurasoa.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        int piezaPos = piezaGurasoa.childCount;
        for (int i = 0; i < piezaGurasoa.childCount; i++)
        {
            if(transform.position.y > piezaGurasoa.GetChild(i).position.y)
            {
                piezaPos = i;
                if(piezaTokia.transform.GetSiblingIndex() < piezaPos)
                {
                    piezaPos--;
                }
                break;
            }
        }
        piezaTokia.transform.SetSiblingIndex(piezaPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(gurasoa);
        transform.SetSiblingIndex(piezaTokia.transform.GetSiblingIndex());

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(piezaTokia);
    }

    public void PiezaEzabatu()
    {
        Destroy(piezaTokia);
    }

    public Transform GetGurasoa()
    {
        return gurasoa;
    }

    public void SetGurasoa(Transform guraso)
    {
        gurasoa = guraso;
    }

    public Transform GetPiezaGurasoa()
    {
        return piezaGurasoa;
    }

    public void SetPiezaGurasoa(Transform guraso)
    {
        piezaGurasoa = guraso;
    }

    public void SetPiezaTokiGurasoa(Transform guraso)
    {
        piezaTokia.transform.SetParent(guraso);
    }
}
