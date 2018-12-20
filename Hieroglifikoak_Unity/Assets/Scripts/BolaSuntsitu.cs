using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaSuntsitu : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bola")
        {
            collision.transform.GetComponent<KakalardoBola>().BolaSuntsitu();
        }
    }
}
