using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiezaInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler/*, IDragHandler*/ {

    public ArgibideakAldatu argibideak;
    [TextArea(3, 10)]
    public string deskribapena;

    public void DeskribapenaIkusi()
    {
        argibideak.textuaAldatu(deskribapena);
    }

    /*public void OnDrag(PointerEventData eventData)
    {
        if(eventData.pointerDrag.GetComponent<Drag>() != null)
        {
            argibideak.ArgibideakJarri();
        }
    }*/

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.GetComponent<PiezaInfo>() != null)
        {
            DeskribapenaIkusi();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bool bestePiezaBatDago = false;
        if (!eventData.dragging)
        {
            List<GameObject> saguAzpian = eventData.hovered;
            for (int i = saguAzpian.Count - 1; i >= 0; i--)
            {
                if(saguAzpian[i] != null)
                {
                    if (saguAzpian[i].GetComponent<PiezaInfo>() != null)
                    {
                        if (saguAzpian[i] != gameObject)
                        {
                            bestePiezaBatDago = true;
                            saguAzpian[i].GetComponent<PiezaInfo>().DeskribapenaIkusi();
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            bestePiezaBatDago = true;
        }
        if (!bestePiezaBatDago)
        {
            argibideak.ArgibideakJarri();
        }
    }
}
