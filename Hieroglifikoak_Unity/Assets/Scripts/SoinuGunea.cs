using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoinuGunea : MonoBehaviour {

    bool entzutenDa = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            entzutenDa = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            entzutenDa = false;
        }
    }

    public bool EntzunDaiteke()
    {
        return entzutenDa;
    }
}
