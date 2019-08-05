using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bukatu : MonoBehaviour {

    public FadeManager fademanager;
    public float denbora;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine("BukaeraEszenatokia");
        }
    }

    IEnumerator BukaeraEszenatokia()
    {
        //JokalariKudetzailea.instantzia.DatuakGorde();
        yield return new WaitForSeconds(denbora);
        fademanager.BukaeraEszenatokia();
    }
}
