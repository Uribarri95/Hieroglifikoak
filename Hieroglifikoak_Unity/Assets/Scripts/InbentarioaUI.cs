using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InbentarioaUI : MonoBehaviour {

    Inbentarioa inbentario;
    ItemTokia[] itemTokiak;

	// Use this for initialization
	void Start () {
        inbentario = Inbentarioa.instantzia;
        inbentario.itemJasoDeitu += InbentarioUIEguneratu;
        itemTokiak = GetComponentsInChildren<ItemTokia>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InbentarioUIEguneratu()
    {
        for (int i = 0; i < inbentario.items.Count; i++)
        {
            itemTokiak[i].AddItem(inbentario.items[i].irudia);
        }
        /*
        for (int i = 0; i < itemTokiak.Length; i++)
        {
            if(i < inbentario.items.Count)
            {
                itemTokiak[i].AddItem(inbentario.items[i]);
            }
            else
            {
                break;
            }
        }*/
    }
}
