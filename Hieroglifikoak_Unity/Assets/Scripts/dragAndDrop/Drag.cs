using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    public Transform lehenPanela;
    public Sprite outline;
    Transform piezaGurasoa;
    Transform hutsuneGurasoa;
    GameObject piezaHutsunea;

    public float hutsuneAltuera;
    public float hutsuneLuzera;

    public void OnBeginDrag(PointerEventData eventData)
    {
        piezaHutsunea = new GameObject();
        piezaHutsunea.transform.SetParent(transform.parent);

        LayoutElement layouta = piezaHutsunea.AddComponent<LayoutElement>();
        layouta.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        layouta.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        //layouta.preferredWidth = hutsuneLuzera;
        //layouta.preferredHeight = hutsuneAltuera;
        layouta.flexibleWidth = 0;
        layouta.flexibleHeight = 0;

        Image ertzak = piezaHutsunea.AddComponent<Image>();
        ertzak.sprite = outline;
        ertzak.color = GetComponent<Image>().color;
        ertzak.type = Image.Type.Sliced;

        piezaHutsunea.transform.SetSiblingIndex(transform.GetSiblingIndex());

        piezaGurasoa = transform.parent;
        hutsuneGurasoa = transform.parent;
        transform.SetParent(lehenPanela);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        if(eventData.pointerEnter.GetComponent<DropTokia>() != null)
        {
            hutsuneGurasoa = eventData.pointerEnter.transform;
        }

        if (piezaHutsunea.transform.parent != hutsuneGurasoa)
        {
            piezaHutsunea.transform.SetParent(hutsuneGurasoa);
        }

        int hutsunePos = hutsuneGurasoa.childCount;
        for (int i = 0; i < hutsuneGurasoa.childCount; i++)
        {
            if(transform.position.y > hutsuneGurasoa.GetChild(i).position.y)
            {
                hutsunePos = i;
                if (piezaHutsunea.transform.GetSiblingIndex() < hutsunePos)
                {
                    hutsunePos--;
                }
                break;
            }
        }
        piezaHutsunea.transform.SetSiblingIndex(hutsunePos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(piezaGurasoa);
        transform.SetSiblingIndex(piezaHutsunea.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(piezaHutsunea);
    }

    // !! hau ez da erabiltzen
    public Transform GetPiezaGurasoa()
    {
        return piezaGurasoa;
    }

    public void SetPiezaGurasoa(Transform gurasoa)
    {
        piezaGurasoa = gurasoa;
    }

    public Transform GetHutsuneGurasoa()
    {
        return hutsuneGurasoa;
    }

    public void SetHutsuneGurasoa(Transform gurasoa)
    {
        hutsuneGurasoa = gurasoa;
    }
}
