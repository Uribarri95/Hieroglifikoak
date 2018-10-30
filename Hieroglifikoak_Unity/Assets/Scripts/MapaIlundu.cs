using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapaIlundu : MonoBehaviour {

    public Tilemap darkMap;         // pintako den tilemap-a (zein geruza)
    public Tilemap background;      // zenbat lauki pintako dira 
    public Tile darkTile;           // zeinekin pintako da
    public bool belztu;

    Ilundu ilundu;
    Inbentarioa inbentario;

    float leunketaFaktorea = 1.5f;  // aldaketa abiadura
    float alfaValue;

	// Use this for initialization
	void Start () {
        ilundu = darkMap.GetComponent<Ilundu>();
        inbentario = Inbentarioa.instantzia;

        darkMap.origin = background.origin;
        darkMap.size = background.size;

        foreach (Vector3Int p in darkMap.cellBounds.allPositionsWithin)
        {
            darkMap.SetTile(p, darkTile);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (inbentario.ItemaDaukat("SuArgia"))
        {
            darkMap.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        else
        {
            darkMap.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.None;
        }

        if (belztu && alfaValue <= .93)
        {
            alfaValue += leunketaFaktorea * Time.deltaTime;
            ilundu.SetAlfa(alfaValue);
        }

        if(!belztu && alfaValue > 0)
        {
            alfaValue -= leunketaFaktorea * Time.deltaTime;
            ilundu.SetAlfa(alfaValue);
        }
    }
}
