using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutxaJarri : MonoBehaviour {

    // kutxa egokia denean reset multzotik kendu -> parent = transform
    public bool kutxaEgokia;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains(name)) // -> if name.contains gorria && collision.contains gorria || ...
        {
            kutxaEgokia = true;
        }
    }

    public bool GetKutxaEgokia()
    {
        return kutxaEgokia;
    }
}
