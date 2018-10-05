using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutxaMugitu : MonoBehaviour {

    /*Vector2 abiadura;
    float grabitatea = -20;
    KutxaMugKud kudeatzailea; /// !!! kutxaren mugimendu kudetzailea, jokalariaren berdina, aldatu eta kode gehiagarria ezabatu

    // Use this for initialization
    void Start()
    {
        kudeatzailea = GetComponent<MugKudeatzaile>();
        abiadura = new Vector2(0, 0);
    }

    void Update()
    {
        if (kudeatzailea.kolpeak.azpian)
            abiadura.y = 0;
        else
            abiadura.y += grabitatea + Time.deltaTime;
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
    }*/

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
 