using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mugitu : GorputzFisikak {

    public float saltoIndarra = 7f;
    public float abiaduraMax = 7f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	protected override void abiaduraKalkulatu()
    {
        Vector2 mugitu = Vector2.zero;
        mugitu.x = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && lurrean)
        {
            abiadura.y = saltoIndarra;
            zoruNormala.y = 1;
            zoruNormala.x = 0;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (abiadura.y > 0)
            {
                abiadura.y = abiadura.y * .5f;
            }
        }
        helmugaAbiadura = mugitu * abiaduraMax;
    }
}
