using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MugKudeatzaile))]
public class JokalariMug : MonoBehaviour {

    public float mugimenduAbiadura = 6;
    public float grabitatea = -50;
    public float saltoIndarra = 20;

    float currentVelocity;
    Vector2 abiadura;

    MugKudeatzaile kudeatzailea;

	// Use this for initialization
	void Start () {
        kudeatzailea = GetComponent<MugKudeatzaile> ();
	}
	
	// Update is called once per frame
	void Update () {
        Aginduak();
	}

    //Teklatutik jasotako eginduak kudeatu. Mugimendu bertikalari grabitate indarra aplikatzen zaio
    void Aginduak()
    {
        if (kudeatzailea.kolpeak.gainean || kudeatzailea.kolpeak.azpian)
        {
            abiadura.y = 0;
        }

        if (Input.GetButtonDown("Jump") && kudeatzailea.kolpeak.azpian)
        {
            abiadura.y = saltoIndarra;
        }
        else if (Input.GetButtonUp("Jump") && abiadura.y > 0)
        {
            abiadura.y = abiadura.y * .5f;
        }

        float input = Input.GetAxisRaw("Horizontal");
        abiadura.x = Mathf.SmoothDamp(abiadura.x, input * mugimenduAbiadura, ref currentVelocity, .1f); //SmoothDamp abiadura aldaketa leuntzen du
        abiadura.y += grabitatea * Time.deltaTime;
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
    }
}
