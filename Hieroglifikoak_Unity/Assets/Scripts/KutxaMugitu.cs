using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutxaMugitu : MonoBehaviour {

    float currentVelocity;
    Vector2 abiadura;
    float xAbiadura = 3;
    JokalariMug jokalaria;
    MugKudeatzaile kudeatzailea; /// !!! kutxaren mugimendu kudetzailea, jokalariaren berdina, aldatu eta kode gehiagarria ezabatu
    // !!! Jokalariak abiadura bidali jokalariaren instantzia eduki beharrean

    // Use this for initialization
    void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
        kudeatzailea = GetComponent<MugKudeatzaile>();
        abiadura = new Vector2(0, 0);
    }

    void Update()
    {
        if (jokalaria.kutxaIkutzen)
            abiadura.x = jokalaria.aginduHorizontala * xAbiadura;
        else
            abiadura.x = 0;
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
    }
}
