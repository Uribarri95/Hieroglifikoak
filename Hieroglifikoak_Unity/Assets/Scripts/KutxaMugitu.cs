using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutxaMugitu : MonoBehaviour {

    Vector2 abiadura;
    float grabitatea = -10;
    KutxaMugKud kudeatzailea; /// !!! kutxaren mugimendu kudetzailea, jokalariaren berdina, aldatu eta kode gehiagarria ezabatu

    // Use this for initialization
    void Start()
    {
        kudeatzailea = GetComponent<KutxaMugKud>();
        abiadura = new Vector2(0, 0);
    }

    void Update()
    {
        if (kudeatzailea.kolpeak.azpian)
        {
            abiadura.y = 0;
        } else
        {
            abiadura.y += grabitatea * Time.deltaTime;
        }
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
    }
}
 