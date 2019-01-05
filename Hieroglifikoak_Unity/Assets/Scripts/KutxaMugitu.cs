using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutxaMugitu : MonoBehaviour {

    bool bultzatzen;
    Vector2 abiadura;
    float grabitatea = -10;
    KutxaMugKud kudeatzailea;

    // Use this for initialization
    void Start()
    {
        kudeatzailea = GetComponent<KutxaMugKud>();
        abiadura = new Vector2(0, 0);
    }

    void Update()
    {
        if (!bultzatzen)
        {
            abiadura.x = 0;
        }
        if (kudeatzailea.kolpeak.azpian)
        {
            abiadura.y = 0;
        } else
        {
            abiadura.y += grabitatea * Time.deltaTime;
        }
        kudeatzailea.Mugitu(abiadura * Time.deltaTime);
        SetBultzatzen(false);
    }

    public void SetAbiadura(Vector2 v)
    {
        abiadura.x = v.x;
    }

    public void SetBultzatzen(bool bultza)
    {
        bultzatzen = bultza;
    }
}
 