using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutxaMugitu : MonoBehaviour {

    float currentVelocity;
    Vector2 abiadura;
    float xAbiadura = 4;
    JokalariMug jokalaria;
    MugKudeatzaile kudeatzailea;

    // Use this for initialization
    void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
        kudeatzailea = GetComponent<MugKudeatzaile>();
        abiadura = new Vector2(0, 0);
    }

    void Update()
    {
        if (jokalaria.kutxaIkutzen && !jokalaria.GetMakurtu())
        {
            abiadura.x = jokalaria.aginduHorizontala * xAbiadura;
            abiadura.x = Mathf.SmoothDamp(abiadura.x, abiadura.x, ref currentVelocity, .1f);
        }
        else
        {
            abiadura.x = 0f;
        }
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
    }
}
