using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutxaJarri : MonoBehaviour {

    public bool kutxaEgokia;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains(name))
        {
            kutxaEgokia = true;
        }
    }

    public bool GetKutxaEgokia()
    {
        return kutxaEgokia;
    }
}
