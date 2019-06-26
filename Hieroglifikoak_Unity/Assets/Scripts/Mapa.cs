using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour {

    public GameObject dialogak;
    public GameObject itemak;
    Data data;

    #region Singleton
    public static Mapa instantzia;

    private void Awake()
    {
        if (instantzia != null)
        {
            Debug.LogWarning("Error Singleton: Mapa bat baino gehiago aurkitu da!");
            return;
        }
        instantzia = this;
    }
    #endregion

    // Use this for initialization
    void Start () {
        data = Data.instantzia;
        MapaKargatu();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            print(dialogak.transform.GetChild(0).gameObject.activeSelf);
        }
	}

    public void MapaKargatu()
    {
        Data.Mapa mapaDatuak = data.MapaDatuakKargatu();
        if(mapaDatuak.itemak != null)
        {
            SetItemak(mapaDatuak.itemak);
        }
        if(mapaDatuak.dialogak != null)
        {
            SetDialogak(mapaDatuak.dialogak);
        }
    }

    public void MapaGorde()
    {
        Data.Mapa datuak = new Data.Mapa();
        datuak.itemak = GetItemak();
        datuak.dialogak = GetDialogak();
        data.MapaGorde(datuak);
    }

    public void SetItemak(bool[] itemZerrenda)
    {
        for (int i = 0; i < itemak.transform.childCount; i++)
        {
            if(itemak.transform.GetChild(i).GetComponent<ItemHolder>() != null)
            {
                itemak.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(itemZerrenda[i]);
            }
            else
            {
                itemak.transform.GetChild(i).gameObject.SetActive(itemZerrenda[i]);
            }
        }
    }

    public void SetDialogak(bool[] dialogZerrenda)
    {
        for (int i = 0; i < dialogak.transform.childCount; i++)
        {
            dialogak.transform.GetChild(i).gameObject.SetActive(dialogZerrenda[i]);
        }
    }

    public bool[] GetItemak()
    {
        bool[] itemZerrenda = new bool[itemak.transform.childCount];
        for (int i = 0; i < itemak.transform.childCount; i++)
        {
            if(itemak.transform.GetChild(i).GetComponent<ItemHolder>() != null)
            {
                itemZerrenda[i] = itemak.transform.GetChild(i).transform.GetChild(0).gameObject.activeSelf;
            }
            else
            {
                itemZerrenda[i] = itemak.transform.GetChild(i).gameObject.activeSelf;
            }
        }
        return itemZerrenda;
    }

    public bool[] GetDialogak()
    {
        bool[] dialogZerrenda = new bool[dialogak.transform.childCount];
        for (int i = 0; i < dialogak.transform.childCount; i++)
        {
            dialogZerrenda[i] = dialogak.transform.GetChild(i).gameObject.activeSelf;
        }
        return dialogZerrenda;
    }
}
