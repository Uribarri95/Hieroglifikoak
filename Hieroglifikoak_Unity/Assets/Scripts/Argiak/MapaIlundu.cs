using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapaIlundu : MonoBehaviour {

    public Tilemap darkMap;         // pintako den tilemap-a (zein geruza)
    public Tilemap background;      // zenbat lauki pintako dira 
    public Tile darkTile;           // zeinekin pintako da

    ErreDaiteke[] suTokiak;         // gela berriz pizteko sutokiak
    Ilundu ilundu;                  // gela iluntzeko metodoa
    public bool belztu;             // gune argi eta ilunen artean argia kontrolatzeko
    bool gelaArgituta;              // behin gela argituta dagoela ez dago iluntasunik

    public float alfaMax = .98f;    // iluntasun kantitate maximoa
    float alfaValue;                // momentuko iluntasun kantitatea
    float leunketaFaktorea = 1f;    // argi/ilun aldaketa abiadura

	// Use this for initialization
	void Start () {
        belztu = false;
        gelaArgituta = false;
        suTokiak = GetComponentsInChildren<ErreDaiteke>();

        ilundu = darkMap.GetComponent<Ilundu>();

        darkMap.origin = background.origin;
        darkMap.size = background.size;

        foreach (Vector3Int p in darkMap.cellBounds.allPositionsWithin)
        {
            darkMap.SetTile(p, darkTile);
        }

        darkMap.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
	
	// Update is called once per frame
	void Update () {
        GelaArgitu();
        
        if (belztu && alfaValue <= alfaMax)
        {
            alfaValue += leunketaFaktorea * 2 * Time.deltaTime;
            ilundu.SetAlfa(alfaValue);
        }

        if(!belztu && alfaValue > 0)
        {
            alfaValue -= leunketaFaktorea * Time.deltaTime;
            ilundu.SetAlfa(alfaValue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !gelaArgituta)
        {
            belztu = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !gelaArgituta)
        {
            belztu = false;
        }
    }

    public void GelaArgitu()
    {
        bool denakPiztuta = true;
        for (int i = 0; i < suTokiak.Length; i++)
        {
            if (!suTokiak[i].GetPiztuta())
            {
                denakPiztuta = false;
                return;
            }
        }
        if (denakPiztuta)
        {
            gelaArgituta = true;
            belztu = false;
        }
    }
}
