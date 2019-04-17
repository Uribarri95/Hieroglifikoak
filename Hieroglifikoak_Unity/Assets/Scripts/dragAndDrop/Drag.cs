using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    public enum mota { Normal, Baldintza, Eragigaia, ForIterator, Denak, Izenburua, Parametroa, Aldagaia};
    public mota piezaMota;
    public Transform mugimenduPanela;
    public Sprite outline;
    public Color kolorea;

    Transform piezaGurasoa;
    Transform hutsuneGurasoa;
    GameObject piezaHutsunea;
    //float hutsuneAltuera = 10;
    int index;

    Transform hasierakoGurasoa;
    int hasierakoPos;

    private void Start()
    {
        hasierakoGurasoa = transform.parent;
        hasierakoPos = transform.GetSiblingIndex();
    }

    public void Reset()
    {
        transform.SetParent(hasierakoGurasoa);
        transform.SetSiblingIndex(hasierakoPos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(piezaMota == mota.Izenburua)
        {
            return;
        }
        piezaHutsunea = new GameObject();
        piezaHutsunea.transform.SetParent(transform.parent, false);

        RectTransform tamaina = piezaHutsunea.AddComponent<RectTransform>();
        float luzera = GetComponent<RectTransform>().sizeDelta.x;
        float altuera = GetComponent<RectTransform>().sizeDelta.y;
        //float altuera = hutsuneAltuera;
        tamaina.sizeDelta = new Vector2(luzera, altuera);

        Image ertzak = piezaHutsunea.AddComponent<Image>();
        ertzak.sprite = outline;
        ertzak.color = kolorea;
        ertzak.type = Image.Type.Sliced;

        TamainaAldaketa piezaTamaina = piezaHutsunea.AddComponent<TamainaAldaketa>();
        piezaTamaina.layoutBertikala = GetComponent<TamainaAldaketa>().layoutBertikala;

        index = transform.GetSiblingIndex();
        piezaHutsunea.transform.SetSiblingIndex(index);

        piezaGurasoa = transform.parent;
        hutsuneGurasoa = transform.parent;
        transform.SetParent(mugimenduPanela);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (piezaMota == mota.Izenburua)
        {
            return;
        }
        float posX = eventData.position.x + (GetComponent<TamainaAldaketa>().GetLuzera() / 3);
        float posY = eventData.position.y - (GetComponent<TamainaAldaketa>().GetAltuera() / 3);
        transform.position = new Vector2(posX, posY);

        if (eventData.pointerEnter.GetComponent<DropTokia>() != null)
        {
            hutsuneGurasoa = eventData.pointerEnter.transform;

            mota containerMota = eventData.pointerEnter.GetComponent<DropTokia>().piezaMota;
            if (containerMota == piezaMota || containerMota == mota.Denak)
            {
                KoloreaAldatu(true);
            }
            else
            {
                KoloreaAldatu(false);
            }
        }

        if (piezaHutsunea.transform.parent != hutsuneGurasoa)
        {
            piezaHutsunea.transform.SetParent(hutsuneGurasoa);
        }

        int hutsunePos = hutsuneGurasoa.childCount;
        for (int i = 0; i < hutsuneGurasoa.childCount; i++)
        {
            if (eventData.position.y > hutsuneGurasoa.GetChild(i).position.y)
            {
                Drag izenburua = hutsuneGurasoa.GetChild(i).GetComponent<Drag>();
                if (izenburua == null || (izenburua != null && izenburua.piezaMota != mota.Izenburua))
                {
                    hutsunePos = i;
                    if (piezaHutsunea.transform.GetSiblingIndex() < hutsunePos)
                    {
                        hutsunePos--;
                    }
                    break;
                }
            }
        }
        piezaHutsunea.transform.SetSiblingIndex(hutsunePos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (piezaMota == mota.Izenburua)
        {
            return;
        }

        transform.SetParent(piezaGurasoa);
        if (transform.parent == hutsuneGurasoa)
        {
            transform.SetSiblingIndex(piezaHutsunea.transform.GetSiblingIndex());
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(piezaHutsunea);
    }

    void KoloreaAldatu(bool koloreZuzena)
    {
        Image ertzak = piezaHutsunea.GetComponent<Image>();
        if (koloreZuzena)
        {
            ertzak.sprite = outline;
            ertzak.color = kolorea;
        }
        else
        {
            ertzak.sprite = outline;
            ertzak.color = Color.red;
        }
    }

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

    public int GetHasierkoIndizea()
    {
        return index;
    }
}
