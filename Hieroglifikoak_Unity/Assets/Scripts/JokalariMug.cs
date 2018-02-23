using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MugKudeatzaile))]
public class JokalariMug : MonoBehaviour {

    float inputAbiadura = 6;
    float grabitatea = -18;
    float saltoIndarra = 8;
    Vector3 abiadura;

    MugKudeatzaile kudeatzailea;

	// Use this for initialization
	void Start () {
        kudeatzailea = GetComponent<MugKudeatzaile> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (kudeatzailea.kolpeak.gainean || kudeatzailea.kolpeak.azpian)
        {
            abiadura.y = 0;
        }
        if (kudeatzailea.kolpeak.eskuma || kudeatzailea.kolpeak.ezkerra)
        {
            abiadura.x = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && kudeatzailea.kolpeak.azpian){
            abiadura.y = saltoIndarra;
        }
        else if (Input.GetButtonUp("Jump") && abiadura.y > 0)
        {
            abiadura.y = abiadura.y * .5f;
        }

        abiadura.x = input.x * inputAbiadura;
        abiadura.y += grabitatea * Time.deltaTime;
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
	}
}
