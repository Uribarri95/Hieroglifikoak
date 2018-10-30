using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ilundu : MonoBehaviour {

    Tilemap darkMap;

	// Use this for initialization
	void Start () {
        darkMap = GetComponent<Tilemap>();
	}

    public void SetAlfa(float alfa)
    {
        darkMap.color = new Color(0, 0, 0, alfa);
    }
}
