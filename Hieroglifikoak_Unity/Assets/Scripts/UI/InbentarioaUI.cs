using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InbentarioaUI : MonoBehaviour {

    Inbentarioa inbentario;         // inbentarioa
    ItemTokia[] itemTokiak;         // centre, left, right -->> orden horretan

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
            if(inbentario.items[i].izena.Contains("Arkua"))             // if i == inbentario.items.Count -1 --> arkua orain lortu da
            {
                itemTokiak[i].ErakutsiGeziKop(inbentario.GetGeziKop());
            }
            else
            {
                itemTokiak[i].EzkutatuGeziKop();
            }
        }
    }
}
